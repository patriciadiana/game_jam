using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    private void Start()
    {
        gameObject.SetActive(false);
        GameManager.Instance.RegisterGameOverScreen(this);
    }

    public void Setup()
    {
        Time.timeScale = 0f;
        gameObject.SetActive(true);
    }

    public void RestartButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
