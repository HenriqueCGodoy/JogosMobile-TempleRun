using UnityEngine;

public class ObstacleMove : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5;

    void Update()
    {
        if (GameControllerScript.Instance == null)
            transform.Translate(Vector3.back * moveSpeed * Time.deltaTime, Space.World);
        else
            transform.Translate(Vector3.back * GameControllerScript.Instance.obstaclesCurrentSpeed * Time.deltaTime, Space.World);
    }
}
