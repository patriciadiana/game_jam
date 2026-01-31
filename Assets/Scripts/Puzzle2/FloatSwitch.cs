using UnityEngine;

public class FloatSwitch : MonoBehaviour
{
    [Header("Platform to Rise")]
    public Transform platformToRise;

    [Header("Movement")]
    public float riseHeight = 3f;
    public float moveSpeed = 2f;

    [Header("Sprite Change")]
    public Sprite unpressedSprite;   // normal look
    public Sprite pressedSprite;     // pressed look

    private SpriteRenderer spriteRenderer;
    private int pressCount = 0;

    private Vector3 startPos;
    private Vector3 upPos;

    private void Awake()
    {
        // Get sprite renderer from THIS switch object
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (platformToRise != null)
        {
            startPos = platformToRise.position;
            upPos = startPos + Vector3.up * riseHeight;
        }

        // Start in unpressed state
        SetPressed(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!IsValid(collision.gameObject)) return;

        pressCount++;
        UpdateSwitchSprite();
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!IsValid(collision.gameObject)) return;

        pressCount = Mathf.Max(0, pressCount - 1);
        UpdateSwitchSprite();
    }

    private void Update()
    {
        if (platformToRise == null) return;

        bool pressed = pressCount > 0;
        Vector3 target = pressed ? upPos : startPos;

        platformToRise.position = Vector3.MoveTowards(
            platformToRise.position,
            target,
            moveSpeed * Time.deltaTime
        );
    }

    private bool IsValid(GameObject go)
        => go.CompareTag("Projectile") || go.CompareTag("Player");

    // -----------------------------
    // Sprite Switching Logic
    // -----------------------------
    private void UpdateSwitchSprite()
    {
        bool pressed = pressCount > 0;
        SetPressed(pressed);
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
