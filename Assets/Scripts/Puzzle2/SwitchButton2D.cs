using UnityEngine;

public class SwitchButton2D : MonoBehaviour
{
    [Header("Platform To Activate")]
    public GameObject platformToActivate;

    [Header("One Time Use?")]
    public bool onlyOnce = true;

    private bool activated = false;

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

        if (platformToActivate != null)
            platformToActivate.SetActive(true);

    }
}
