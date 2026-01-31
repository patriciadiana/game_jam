using UnityEngine;

public class DistractionProjectile2D : MonoBehaviour
{
    [Header("When this hits ground, enemy will investigate")]
    public LayerMask groundLayer;

    // Optional: set this if you have multiple enemies and want a specific one
    public EnemyPatrolChaseDistract2D targetEnemy;

    private bool triggered;

    private void Start()
    {
        if (!targetEnemy)
            targetEnemy = FindFirstObjectByType<EnemyPatrolChaseDistract2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (triggered) return;

        // If the thing we hit is ground (by layer mask)
        if (((1 << collision.gameObject.layer) & groundLayer) == 0) return;

        triggered = true;

        // Use collision contact point (more accurate than object center)
        Vector2 point = collision.GetContact(0).point;

        if (targetEnemy)
            targetEnemy.DistractTo(point);
    }
}
