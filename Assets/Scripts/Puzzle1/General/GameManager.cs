using UnityEngine;

public class GameManager : SingletonPersistent<GameManager>
{
    public GameOverScreen gameOverScreen;
    public bool isGameOver { get; private set; } = false;

    public void GameLost()
    {
        if (isGameOver) return;

        isGameOver = true;
        Debug.Log("Game Over!");

        // Find GameObject by tag
        GameObject taggedObject = GameObject.FindGameObjectWithTag("GameOverScreen");

        if (taggedObject != null)
        {
            Debug.Log($"Found GameObject with tag 'GameOverScreen': {taggedObject.name}");

            // Get the GameOverScreen component from it
            GameOverScreen screen = taggedObject.GetComponent<GameOverScreen>();

            if (screen != null)
            {
                screen.Setup();
            }
            else
            {
                Debug.LogError($"GameObject '{taggedObject.name}' has tag 'GameOverScreen' but no GameOverScreen script component!");
            }
        }
        else
        {
            Debug.LogError("No GameObject found with tag 'GameOverScreen'!");
        }
    }
}
