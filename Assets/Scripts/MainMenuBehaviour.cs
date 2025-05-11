using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuBehaviour : MonoBehaviour
{
    /// <summary>
    /// will load a new scene upon being called
    /// </summary>
    /// <param name="levelName">The name of the level we want to go to</param>
    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }
}
