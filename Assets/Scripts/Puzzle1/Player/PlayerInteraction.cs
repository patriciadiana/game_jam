using UnityEngine;

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
        }
    }

    void CheckProximity()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, interactRange);

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
        }

        if (!isNearMask && objectToShow != null)
        {
            objectToShow.SetActive(false);
        }
    }
}
