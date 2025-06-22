using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameControllerScript : MonoBehaviour
{
    public static GameControllerScript Instance;
    private float score = 0;
    [SerializeField] private float obstaclesInitialSpeed = 5;
    [SerializeField] public float difficultyIncreasePerSec = 0.02f;
    public float obstaclesCurrentSpeed;
    [SerializeField] private GameObject playerRef;
    private PlayerMove playerScript;
    [SerializeField] private GameObject timeScoreTextObj;
    private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject pauseButton;
    [SerializeField] private Toggle accelerometerToggle;
    

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }

    void Start()
    {
        Time.timeScale = 1;
        playerScript = playerRef.GetComponent<PlayerMove>();
        scoreText = timeScoreTextObj.GetComponent<TextMeshProUGUI>();
        obstaclesCurrentSpeed = obstaclesInitialSpeed;

    }

    void Update()
    {
        accelerometerToggle.isOn = persistOptionsBetweenRestarts.Instance.accelerometerState;
        if (accelerometerToggle.isOn)
        {
            playerScript.mobileMove = PlayerMove.MobileMovement.Accelerometer;
        }
        else
        {
            playerScript.mobileMove = PlayerMove.MobileMovement.ScreenTouch;
        }

        //Player is dead
        if (!playerScript.IsPlayerAlive())
        {
            GameOver();
        }

        UpdateUI();
    }

    private void GameOver()
    {
        Time.timeScale = 0;

        timeScoreTextObj.SetActive(false);
        gameOverPanel.SetActive(true);

        finalScoreText.text = "Score: " + score;
    }

    private void UpdateUI()
    {
        score = Mathf.Round(Time.timeSinceLevelLoad);
        scoreText.text = "Time score:\n" + score + "s";
    }

    public void Pause()
    {
        Time.timeScale = 0;
        playerScript.enabled = false;
        pausePanel.SetActive(true);
        pauseButton.SetActive(false);
    }

    public void Unpause()
    {
        playerScript.enabled = true;
        Time.timeScale = 1;
        pausePanel.SetActive(false);
        pauseButton.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Menu(string menuSceneName)
    {
        SceneManager.LoadScene(menuSceneName);
    }

    public void AccelerometerToggle(bool selected)
    {
        if (selected)
        {
            playerScript.mobileMove = PlayerMove.MobileMovement.Accelerometer;
            persistOptionsBetweenRestarts.Instance.accelerometerState = true;
        }
        else
        {
            playerScript.mobileMove = PlayerMove.MobileMovement.ScreenTouch;
            persistOptionsBetweenRestarts.Instance.accelerometerState = false;
        }
    }
}
