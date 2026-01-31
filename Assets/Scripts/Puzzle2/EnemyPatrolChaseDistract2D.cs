using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyPatrolChaseDistract2D : MonoBehaviour
{
    public enum State { Patrol, Chase, Investigate, Wait }

    [Header("References")]
    public Transform player;
    public LayerMask groundLayer;
    public LayerMask obstacleLayer; // walls/ground that can block sight (optional)

    [Header("Movement")]
    public float patrolSpeed = 2f;
    public float chaseSpeed = 3.5f;

    [Header("Edge & Wall Check")]
    public Transform groundCheck;      // place near enemy feet, slightly forward
    public float groundCheckDistance = 0.35f;
    public Transform wallCheck;        // place at enemy "nose"
    public float wallCheckDistance = 0.2f;

    [Header("Vision")]
    public float visionDistance = 6f;
    public float visionHeightTolerance = 1.2f; // ignore player too far above/below

    [Header("Distraction")]
    public float investigateStopDistance = 0.2f;
    public float waitAtDistractionSeconds = 3f;

    [Header("Touch Player")]
    public bool disablePlayerOnTouch = true;

    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private State state = State.Patrol;

    private int facing = 1; // 1 = right, -1 = left
    private Vector2 investigateTarget;
    private Coroutine waitRoutine;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    void Start()
    {
        if (!player)
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p) player = p.transform;
        }
    }

    void FixedUpdate()
    {
        // Highest priority: if we are investigating/waiting, ignore chase.
        // (Feel free to change later if you want chase to override distraction.)
        if (state == State.Patrol || state == State.Chase)
        {
            bool seesPlayer = CanSeePlayer();
            if (seesPlayer) state = State.Chase;
            else if (state == State.Chase) state = State.Patrol;
        }

        switch (state)
        {
            case State.Patrol:
                Patrol();
                break;

            case State.Chase:
                Chase();
                break;

            case State.Investigate:
                InvestigateMove();
                break;

            case State.Wait:
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
                break;
        }
    }

    // ------------------------
    // Patrol
    // ------------------------
    void Patrol()
    {
        if (ShouldTurnAround())
            Flip();

        rb.linearVelocity = new Vector2(facing * patrolSpeed, rb.linearVelocity.y);
    }

    bool ShouldTurnAround()
    {
        // 1) Edge check: is there ground ahead?
        bool hasGroundAhead = true;
        if (groundCheck)
        {
            var hit = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);
            hasGroundAhead = hit.collider != null;
        }

        // 2) Wall check: is there a wall in front?
        bool wallAhead = false;
        if (wallCheck)
        {
            var dir = new Vector2(facing, 0);
            var hit = Physics2D.Raycast(wallCheck.position, dir, wallCheckDistance, groundLayer);
            wallAhead = hit.collider != null;
        }

        return !hasGroundAhead || wallAhead;
    }

    void Flip()
    {
        facing *= -1;

        if (sprite)
            sprite.flipX = facing < 0;

        // If you prefer flipping the whole transform scale instead:
        // var s = transform.localScale;
        // s.x = Mathf.Abs(s.x) * facing;
        //transform.localScale = s;
    }

    // ------------------------
    // Vision & Chase
    // ------------------------
    bool CanSeePlayer()
    {
        if (!player) return false;

        // only see in facing direction
        float dx = player.position.x - transform.position.x;
        if (Mathf.Sign(dx) != Mathf.Sign(facing)) return false;

        // ignore if player is too far vertically (helps in platformers)
        float dy = Mathf.Abs(player.position.y - transform.position.y);
        if (dy > visionHeightTolerance) return false;

        float dist = Mathf.Abs(dx);
        if (dist > visionDistance) return false;

        // optional line of sight check (obstacles blocking)
        Vector2 origin = (Vector2)transform.position;
        Vector2 target = (Vector2)player.position;
        Vector2 dir = (target - origin).normalized;

        // If you don't care about obstacles, you can just return true here.
        var hit = Physics2D.Raycast(origin, dir, visionDistance, obstacleLayer);
        if (hit.collider != null)
        {
            // If we hit something before reaching the player, LOS is blocked.
            // (Make sure your obstacles are in obstacleLayer.)
            return false;
        }

        return true;
    }

    void Chase()
    {
        if (!player)
        {
            state = State.Patrol;
            return;
        }

        // Face towards player while chasing
        float dx = player.position.x - transform.position.x;
        if (dx != 0 && Mathf.Sign(dx) != Mathf.Sign(facing))
            Flip();

        // Move toward player (still respects edge/wall turn-around to avoid falling)
        if (ShouldTurnAround())
        {
            // If you'd rather have the enemy fall while chasing, remove this.
            Flip();
        }

        rb.linearVelocity = new Vector2(facing * chaseSpeed, rb.linearVelocity.y);
    }

    // ------------------------
    // Distraction / Investigate
    // ------------------------
    public void DistractTo(Vector2 worldPosition)
    {
        // Cancel any waiting coroutine
        if (waitRoutine != null)
        {
            StopCoroutine(waitRoutine);
            waitRoutine = null;
        }

        investigateTarget = worldPosition;
        state = State.Investigate;
    }

    void InvestigateMove()
    {
        float dx = investigateTarget.x - transform.position.x;

        if (Mathf.Abs(dx) <= investigateStopDistance)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            state = State.Wait;

            waitRoutine = StartCoroutine(WaitThenPatrol());
            return;
        }

        // Face the target
        int desiredFacing = dx >= 0 ? 1 : -1;
        if (desiredFacing != facing) Flip();

        // Move toward target but don't fall off edges
        if (ShouldTurnAround())
        {
            // If target is across a gap, we stop at the edge
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            state = State.Wait;
            waitRoutine = StartCoroutine(WaitThenPatrol());
            return;
        }

        rb.linearVelocity = new Vector2(facing * patrolSpeed, rb.linearVelocity.y);
    }

    IEnumerator WaitThenPatrol()
    {
        yield return new WaitForSeconds(waitAtDistractionSeconds);
        state = State.Patrol;
        waitRoutine = null;
    }

    // ------------------------
    // Player touch
    // ------------------------
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!disablePlayerOnTouch) return;

        if (collision.collider.CompareTag("Player"))
        {
            collision.collider.gameObject.SetActive(false); // “disappear”
        }
    }

    // Debug rays in Scene view
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        if (groundCheck)
        {
            Gizmos.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * groundCheckDistance);
        }

        if (wallCheck)
        {
            Vector3 dir = new Vector3(facing == 0 ? 1 : facing, 0, 0);
            Gizmos.DrawLine(wallCheck.position, wallCheck.position + dir * wallCheckDistance);
        }

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(facing == 0 ? 1 : facing, 0, 0) * visionDistance);
    }
}
