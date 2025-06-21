using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControllerScript : MonoBehaviour
{
    private float score = 0;
    [SerializeField] private GameObject playerRef;
    private PlayerMove playerScript;
    [SerializeField] private GameObject timeScoreTextObj;
    private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject pauseButton;

    void Start()
    {
        Time.timeScale = 1;
        playerScript = playerRef.GetComponent<PlayerMove>();
        scoreText = timeScoreTextObj.GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        //Player is dead
        if (!playerScript.IsPlayerAlive())
        {
            GameOver();
        }

        InvokeRepeating("UpdateUI", 0, 0.1f);
    }

    private void GameOver()
    {
        CancelInvoke("UpdateUI");
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
        pausePanel.SetActive(true);
        pauseButton.SetActive(false);
    }

    public void Unpause()
    {
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
            playerScript.mobileMove = PlayerMove.MobileMovement.Accelerometer;
        else
            playerScript.mobileMove = PlayerMove.MobileMovement.ScreenTouch;
    }
}
