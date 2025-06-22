using UnityEngine;

public class ObstacleMove : MonoBehaviour
{
    void Update()
    {
        transform.Translate(Vector3.back * GameControllerScript.Instance.obstaclesCurrentSpeed * Time.deltaTime, Space.World);
    }
}
