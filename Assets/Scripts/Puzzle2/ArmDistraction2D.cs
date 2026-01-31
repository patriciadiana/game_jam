using UnityEngine;

public class ArmDistraction2D : MonoBehaviour
{
    [Header("Ground detection")]
    public LayerMask groundLayer;

    [Header("Enemy hearing range")]
    public float notifyRadius = 20f;

    private bool hasNotifiedThisThrow = false;

    public void ResetForNewThrow()
    {
        hasNotifiedThisThrow = false;
    }

    public void ResetOnReturn()
    {
        hasNotifiedThisThrow = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasNotifiedThisThrow) return;

        if (((1 << collision.gameObject.layer) & groundLayer) == 0) return;

        hasNotifiedThisThrow = true;

        Vector2 point = collision.GetContact(0).point;

        var enemy = FindClosestEnemy(point);
        if (enemy != null)
            enemy.DistractTo(point);
    }

    private EnemyPatrolChaseDistract2D FindClosestEnemy(Vector2 point)
    {
        var enemies = FindObjectsByType<EnemyPatrolChaseDistract2D>(FindObjectsSortMode.None);

        EnemyPatrolChaseDistract2D best = null;
        float bestDist = float.MaxValue;

        foreach (var e in enemies)
        {
            float d = Vector2.Distance(point, e.transform.position);
            if (d <= notifyRadius && d < bestDist)
            {
                bestDist = d;
                best = e;
            }
        }

        return best;
    }
}
