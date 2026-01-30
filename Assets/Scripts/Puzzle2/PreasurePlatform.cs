using UnityEngine;

public class PressurePlatform : MonoBehaviour
{
    [Header("Door To Deactivate")]
    public GameObject doorToDeactivate;

    private int pressCount = 0;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!IsValid(collision.gameObject)) return;

        pressCount++;
        UpdateDoor();
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!IsValid(collision.gameObject)) return;

        pressCount = Mathf.Max(0, pressCount - 1);
        UpdateDoor();
    }

    private bool IsValid(GameObject go)
        => go.CompareTag("Projectile") || go.CompareTag("Player");

    private void UpdateDoor()
    {
        if (doorToDeactivate != null)
            doorToDeactivate.SetActive(pressCount == 0); // pressed => false, not pressed => true
    }
}
