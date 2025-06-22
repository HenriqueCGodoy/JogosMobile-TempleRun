using UnityEngine;

public class upAndDown : MonoBehaviour
{
    [SerializeField] private float oscilationSpeed = 15;
    [SerializeField] private float minHeight;
    [SerializeField] private float maxHeight;
    private int direction = 1;

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y >= maxHeight)
        {
            direction = -1;
        }
        else if (transform.position.y <= minHeight)
        {
            direction = 1;
        }

        transform.Translate(Vector3.up * direction * oscilationSpeed * Time.deltaTime, Space.World);
    }
}
