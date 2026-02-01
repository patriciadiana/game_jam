using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Interaction Settings")]
    public float interactRange = 1.5f;
    public KeyCode interactKey = KeyCode.E;

    [Header("UI or Object to Show")]
    public GameObject objectToShow;

    private bool isNearMask = false;

    private void Update()
    {
        CheckProximity();

        if (isNearMask && Input.GetKeyDown(interactKey))
        {
            Debug.Log("Bravo frate treci in etapa urmatoare");

            int currentIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentIndex + 1);
        }
    }

    void CheckProximity()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, interactRange);

        var hide = true;
        isNearMask = false;

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Mask"))
            {
                isNearMask = true;

                if (objectToShow != null)
                {
                    objectToShow.SetActive(true);
                }

                return;
            }
            else
            {
                if (hit.CompareTag("Clues") || hit.CompareTag("Seif"))
                {
                    hide = true;
                    if (objectToShow != null)
                    {
                        objectToShow.SetActive(true);
                    }

                    return;
                }
                else
                {
                    hide = false;
                }
            }
        }

        if (!isNearMask && objectToShow != null || hide == false)
        {
            objectToShow.SetActive(false);
        }
    }
}
