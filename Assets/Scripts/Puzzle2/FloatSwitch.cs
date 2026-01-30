using UnityEngine;

public class FloatSwitch : MonoBehaviour
{
    [Header("Platform to Rise")]
    public Transform platformToRise;

    [Header("Movement")]
    public float riseHeight = 3f;
    public float moveSpeed = 2f;

    private int pressCount = 0;

    private Vector3 startPos;
    private Vector3 upPos;

    private void Awake()
    {
        if (platformToRise != null)
        {
            startPos = platformToRise.position;
            upPos = startPos + Vector3.up * riseHeight;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!IsValid(collision.gameObject)) return;
        pressCount++;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!IsValid(collision.gameObject)) return;
        pressCount = Mathf.Max(0, pressCount - 1);
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

}