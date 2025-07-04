using TMPro;
using UnityEngine;

public class ShowAccelerometervalues : MonoBehaviour
{
    private float accelerometerXValue;
    private TextMeshProUGUI accelerometerXText;
    [SerializeField] private PlayerMove playerRef;

    void Start()
    {
        accelerometerXValue = 0;
        accelerometerXText = gameObject.GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        accelerometerXValue = Input.acceleration.x;
        accelerometerXText.text = "X: " + playerRef.horizontalSpeed;
    }


}
