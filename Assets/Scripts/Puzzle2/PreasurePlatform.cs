using UnityEngine;

public class PressurePlatform : MonoBehaviour
{
    [Header("Door To Deactivate")]
    public GameObject doorToDeactivate;

    [Header("Sprite Change")]
    public Sprite unpressedSprite;   // default look
    public Sprite pressedSprite;     // pressed look

    private SpriteRenderer spriteRenderer;
    private int pressCount = 0;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Start in unpressed state
        SetPressed(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!IsValid(collision.gameObject)) return;

        pressCount++;
        UpdatePlatform();
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!IsValid(collision.gameObject)) return;

        pressCount = Mathf.Max(0, pressCount - 1);
        UpdatePlatform();
    }

    private bool IsValid(GameObject go)
        => go.CompareTag("Projectile") || go.CompareTag("Player");

    private void UpdatePlatform()
    {
        bool isPressed = pressCount > 0;

        // Door logic: pressed => deactivate door
        if (doorToDeactivate != null)
            doorToDeactivate.SetActive(!isPressed);

        // Sprite logic
        SetPressed(isPressed);
    }

    private void SetPressed(bool pressed)
    {
        if (spriteRenderer == null) return;

        if (pressed && pressedSprite != null)
            spriteRenderer.sprite = pressedSprite;
        else if (!pressed && unpressedSprite != null)
            spriteRenderer.sprite = unpressedSprite;
    }
}
