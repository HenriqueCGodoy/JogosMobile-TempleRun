using UnityEngine;

/// <summary>
/// Responsible for moving the player automatically and
/// receiving input.
/// </summary>

[RequireComponent(typeof(Rigidbody))]
public class PlayerBehaviour : MonoBehaviour
{
    /// <summary>
    /// A reference to the Rigidbody component
    /// </summary>
    private Rigidbody rb;

    [Tooltip("How fast the ball moves left/right")]
    public float dodgeSpeed = 5;

    [Tooltip("How fast the ball moves forwards automatically")]
    [Range(0, 10)]
    public float rollSpeed = 5;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Get access to the Rigidbody component
        rb = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// FixedUpdate is a prime place to put physics calculations
    /// happening over a period of time.
    /// </summary>
    void FixedUpdate()
    {
        //Check if we're moving to the side
        var horizontalSpeed = Input.GetAxis("Horizontal") * dodgeSpeed;
        
        // Check if we are running either in the Unity editor or in a standalone build
        #if UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_EDITOR
            //If the mouse is being held down (or the screen is pressed on mobile)
            if (Input.GetMouseButton(0))
            {
                var screenPos = Input.mousePosition;
                horizontalSpeed = CalculateMovement(screenPos);
            }
        // Check if we are running on mobile devices
        #elif UNITY_IOS || UNITY_ANDROID
            //Check if the input has registered more than 0 touches
            if (Input.touchCount > 0)
            {
                //Store the first touch detected
                var firstTouch = Input.touches[0];
                var screenPos = firstTouch.position;
                horizontalSpeed = CalculateMovement(screenPos);
            }
        #endif
        
        
        rb.AddForce(horizontalSpeed, 0, rollSpeed);
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
}
