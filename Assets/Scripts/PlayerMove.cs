using System.Collections;
using UnityEngine;

/// <summary>
/// Responsible for moving the player automatically and
/// receiving input.
/// </summary>

[RequireComponent(typeof(Rigidbody))]
public class PlayerMove : MonoBehaviour
{
    /// <summary>
    /// A reference to the Rigidbody component
    /// </summary>
    private Rigidbody rb;

    /// <summary>
    /// The force of the player jump
    /// </summary>
    [SerializeField] private float jumpForce = 10;

    [Tooltip("How fast the player moves left/right")]
    public float dodgeSpeed = 5;

    public enum MobileMovement
    {
        Accelerometer, ScreenTouch
    }
    [Tooltip("What horizontal movement type should be used")]
    public MobileMovement mobileMove = MobileMovement.Accelerometer;

    [Header("Swipe Properties")]
    [Tooltip("How far must the player swipe before we will execute the action (in inches)")]
    public float minSwipeDistance = 0.25f;
    /// <summary> 
    /// Used to hold the value that converts 
    /// minSwipeDistance to pixels 
    /// </summary> 
    private float minSwipeDistancePixels;
    /// <summary> 
    /// Stores the starting position of mobile touch 
    /// events 
    /// </summary> 
    private Vector2 touchStart;

    [SerializeField] private bool isGrounded = false;
    [SerializeField] private bool queuedJump = false;
    [SerializeField] private float queueJumpTime = 0.2f;
    float groundDetectionSphereRadius;
    Vector3 groundDetectionSpherePos;

    private bool isAlive = true;

    [SerializeField] private float accelerometerJumpThreshold = 3;
    private bool canStartQueueJumpCoroutine = true;
    [SerializeField] private float extraDownForce = 0.5f;

    void Start()
    {
        //Get access to the Rigidbody component
        rb = GetComponent<Rigidbody>();

        minSwipeDistancePixels = minSwipeDistance * Screen.dpi;

        groundDetectionSphereRadius = GetComponent<CapsuleCollider>().radius * 0.9f;
    }

    void FixedUpdate()
    {
        //Check if we're moving to the side
        var horizontalSpeed = Input.GetAxis("Horizontal") * dodgeSpeed;
        isGrounded = Physics.CheckSphere(groundDetectionSpherePos, groundDetectionSphereRadius, ~LayerMask.NameToLayer("Ground"));
        // Check if we are running either in the Unity editor or in a standalone build
#if UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_EDITOR
        //If the mouse is being held down
        if (Input.GetMouseButton(0))
        {
            var screenPos = Input.mousePosition;
            horizontalSpeed = CalculateMovement(screenPos);
        }

        // Check if we are running on mobile devices
#elif UNITY_IOS || UNITY_ANDROID
            
        switch (mobileMove) 
        { 
            case MobileMovement.Accelerometer: 
                /* Move player based on accelerometer 
                direction */
                horizontalSpeed = Input.acceleration.x * dodgeSpeed; 
                break; 
            case MobileMovement.ScreenTouch: 
                /* Check if Input registered more than 
                zero touches */
                if (Input.touchCount > 0) 
                { 
                    /* Store the first touch detected */
                    var firstTouch = Input.touches[0]; 
                    var screenPos = firstTouch.position; 
                    horizontalSpeed = CalculateMovement(screenPos);
                } 
                break;
        }

#endif


        rb.AddForce(horizontalSpeed, 0, 0, ForceMode.Force);

        if (rb.linearVelocity.y < 0)
        {
            rb.AddForce(0, -extraDownForce, 0);
        }
    }

    /// <summary> 
    /// Update is called once per frame 
    /// </summary> 
    private void Update()
    {
        groundDetectionSpherePos = transform.position + Vector3.down * transform.localScale.y * 0.6f;
        /* Check if we are running either in the Unity editor or in a 
        * standalone build.*/
#if UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_EDITOR
        /* If the mouse is tapped */
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 screenPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }
        if (Input.GetKeyDown(KeyCode.Space)&& canStartQueueJumpCoroutine && !isGrounded)
        {
            StartCoroutine(queueJump());
        }
        if ((Input.GetKeyDown(KeyCode.Space) || queuedJump) && isGrounded)
        {
            Jump();
        }
        /* Check if we are running on a mobile device */
#elif UNITY_IOS || UNITY_ANDROID
        /* Check if Input has registered more than 
        zero touches */
        switch(mobileMove)
        {
            case MobileMovement.Accelerometer:
                bool tryJump = (Input.acceleration.z >= accelerometerJumpThreshold)?true:false;
                if (tryJump && canStartQueueJumpCoroutine && !isGrounded)
                {
                    StartCoroutine(queueJump());
                }
                if((tryJump || queuedJump) && isGrounded)
                {
                    Jump();
                }
                break;
            case MobileMovement.ScreenTouch:
                if (Input.touchCount > 0) 
                { 
                    /* Store the first touch detected */
                    Touch touch = Input.touches[0]; 
                    SwipeJump(touch);
                }
                break;
        }
#endif
    }

    /// <summary>
    /// Will figure out where to move the player horizontaly
    /// </summary>
    /// <param name="screenPos">The position the player has touched/clicked on in screen space</param>
    /// <returns> The direction to move in the x axis </returns>
    private float CalculateMovement(Vector3 screenPos)
    {
        // Get a reference to the camera for converting between spaces
        var cam = Camera.main;

        var viewPos = cam.ScreenToViewportPoint(screenPos);

        float xMove = 0;

        // If we press the right side of the screen
        if (viewPos.x < 0.5f)
        {
            xMove = -1;
        }
        else
        {
            //Otherwise, we are on the left
            xMove = 1;
        }

        //Replace horizontalSpeed with our own value
        return xMove * dodgeSpeed;
    }


    /// <summary> 
    /// Player will jump if swiped up
    /// </summary> 
    /// <param name="touch">Current touch event</param>
    private void SwipeJump(Touch touch)
    {
        /* Check if the touch just started */
        if (touch.phase == TouchPhase.Began)
        {
            /* If so, set touchStart */
            touchStart = touch.position;
        }
        /* If the touch has ended */
        else if (touch.phase == TouchPhase.Ended)
        {
            /* Get the position the touch ended at */
            Vector2 touchEnd = touch.position;
            /* Calculate the difference between the beginning and end of the touch on the y 
            axis. */
            float y = touchEnd.y - touchStart.y;

            /* If not moving far enough, don't do the 
            jump */
            if (Mathf.Abs(y) < minSwipeDistancePixels)
            {
                return;
            }

            /* If moved negatively in the y axis, return */
            if (y <= 0)
            {
                return;
            }

            if (isGrounded && queuedJump)
            {
                Jump();
                return;
            }
            if (!isGrounded && canStartQueueJumpCoroutine)
            {
                StartCoroutine(queueJump());
            }
            if (isGrounded)
            {
                Jump();
            }

        }
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, 0);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        queuedJump = false;
    }

    IEnumerator queueJump()
    {
        queuedJump = true;
        canStartQueueJumpCoroutine = false;
        yield return new WaitForSeconds(queueJumpTime);
        queuedJump = false;
        canStartQueueJumpCoroutine = true;
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundDetectionSpherePos, groundDetectionSphereRadius);
    }

    public bool IsPlayerAlive()
    {
        return isAlive;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Obstacle")
        {
            isAlive = false;
        }
    }
}
