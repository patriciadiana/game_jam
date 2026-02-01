using UnityEngine;
using UnityEngine.SceneManagement;

public class MaskChoiceUI : MonoBehaviour
{
    [Header("Buttons")]
    public GameObject keepMaskButton;
    public GameObject destroyMaskButton;

    [Header("Question Text")]
    public GameObject questionText;

    [Header("Result Texts")]
    public GameObject keepMaskText;
    public GameObject destroyMaskText;

    [Header("Main Menu Button")]
    public GameObject mainMenuButton;

    void Start()
    {
        // Initial state
        keepMaskText.SetActive(false);
        destroyMaskText.SetActive(false);
        mainMenuButton.SetActive(false);
    }

    public void OnKeepMaskPressed()
    {
        HideChoiceUI();

        keepMaskText.SetActive(true);
        mainMenuButton.SetActive(true);
    }

    public void OnDestroyMaskPressed()
    {
        HideChoiceUI();

        destroyMaskText.SetActive(true);
        mainMenuButton.SetActive(true);
    }

    private void HideChoiceUI()
    {
        keepMaskButton.SetActive(false);
        destroyMaskButton.SetActive(false);
        questionText.SetActive(false);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
