using UnityEngine;

public class ObstacleMove : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5;

    void Update()
    {
        transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);
    }
}
