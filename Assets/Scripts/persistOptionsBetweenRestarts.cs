using UnityEngine;
using UnityEngine.UI;

public class persistOptionsBetweenRestarts : MonoBehaviour
{
    public bool accelerometerState = true;
    public static persistOptionsBetweenRestarts Instance;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
    
    }
}
