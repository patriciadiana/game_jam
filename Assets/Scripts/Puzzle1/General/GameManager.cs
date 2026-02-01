using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private GameOverScreen gameOverScreen;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void RegisterGameOverScreen(GameOverScreen screen)
    {
        gameOverScreen = screen;
    }

    public void GameOver()
    {
        if (gameOverScreen != null)
        {
            gameOverScreen.Setup();
        }
    }
}
