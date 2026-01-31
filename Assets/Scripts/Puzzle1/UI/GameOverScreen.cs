using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : SingletonPersistent<GameOverScreen>
{
    private void Start()
    {
        gameObject.SetActive(false);
    }
    public void Setup()
    {
        Time.timeScale = 0f;
        gameObject.SetActive(true);
    }

    public void RestartButton()
    {
        Time.timeScale = 1f;

        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }
}
