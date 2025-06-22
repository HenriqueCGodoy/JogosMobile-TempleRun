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
    [SerializeField] private float maxDifficulty = 5f;
    private float CurrentDifficulty = 1f;
    public float obstaclesCurrentSpeed;
    [SerializeField] private GameObject playerRef;
    private PlayerMove playerScript;
    private spawnerScript spawnerScript;
    [SerializeField] private GameObject timeScoreTextObj;
    private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject pauseButton;
    [SerializeField] private Toggle accelerometerToggle;
    private float timeSum = 0f;

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
        spawnerScript = GetComponent<spawnerScript>();
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

        timeSum += Time.deltaTime;
        if (timeSum >= 1)
        {
            timeSum = 0f;
            score += 1;
            if (CurrentDifficulty <= maxDifficulty)
            {
                CurrentDifficulty += difficultyIncreasePerSec;
                DifficultyIncreased();
            }
        }
    }

    private void DifficultyIncreased()
    {
        obstaclesCurrentSpeed = obstaclesInitialSpeed * CurrentDifficulty;
        spawnerScript.spawnDelayTime = spawnerScript.initialSpawnDelayTime * (1/CurrentDifficulty);
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
