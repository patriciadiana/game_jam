using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameOverScreen gameOverScreen;
    public bool isGameOver { get; private set; } = false;

    public void GameLost()
    {
        if (isGameOver) return;

        isGameOver = true;
        Debug.Log("Game Over!");

        if (gameOverScreen != null)
            gameOverScreen.Setup();
    }
}
