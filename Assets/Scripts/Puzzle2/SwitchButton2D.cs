using UnityEngine;

public class SwitchButton2D : MonoBehaviour
{
    [Header("Platform To Activate")]
    public GameObject platformToActivate;

    [Header("One Time Use?")]
    public bool onlyOnce = true;

    [Header("Sprite Change")]
    public Sprite switchedSprite; // assign the pressed version here

    private bool activated = false;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        // Get the SpriteRenderer on this object
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (activated && onlyOnce) return;

        if (other.CompareTag("Projectile"))
        {
            ActivateSwitch();
        }
    }

    private void ActivateSwitch()
    {
        activated = true;

        Debug.Log("Switch Activated!");

        // Activate platform
        if (platformToActivate != null)
            platformToActivate.SetActive(true);

        // Change sprite to switched version
        if (spriteRenderer != null && switchedSprite != null)
        {
            spriteRenderer.sprite = switchedSprite;
        }
    }
}
