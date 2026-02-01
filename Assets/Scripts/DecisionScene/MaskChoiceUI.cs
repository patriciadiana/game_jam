using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    [SerializeField] private Sprite mask;
    [SerializeField] private Sprite destory_mask;
    [SerializeField] private Image background;

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

        background.sprite = mask;
        keepMaskText.SetActive(true);
        mainMenuButton.SetActive(true);
    }

    public void OnDestroyMaskPressed()
    {
        HideChoiceUI();
        background.sprite = destory_mask;
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
