using UnityEngine;

public class destroyObstacle : MonoBehaviour
{
    private float destroyPosition = 0;
    void Update()
    {
        if (transform.position.z <= destroyPosition)
            Destroy(gameObject);
    }

    public void SetDestroyPosition(float zPosition)
    {
        destroyPosition = zPosition;
    }
}
