using UnityEngine;

public class pillarsSpin : MonoBehaviour
{
    [SerializeField] private float spinSpeed = 5;
    private Vector3 rotationAxis = new Vector3(0, 1, 0);

    void Update()
    {
        transform.Rotate(rotationAxis*spinSpeed*Time.deltaTime);
    }
}
