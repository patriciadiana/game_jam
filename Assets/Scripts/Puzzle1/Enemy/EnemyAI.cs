using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Vision Cone")]
    public Transform visionCone;
    public Color patrolColor = new Color(1f, 1f, 1f, 0.5f);
    public Color alertColor = new Color(1f, 0f, 0f, 0.6f);   

    private SpriteRenderer visionConeRenderer;
    enum EnemyState
    {
        Patrol,
        Chase
    }

    private EnemyState currentState = EnemyState.Patrol;

    [Header("Movement")]
    public float moveSpeed = 3f;
    public float chaseSpeed = 3.5f;

    private Rigidbody2D rb;
    private Vector2 moveDirection;

    [Header("Target")]
    public Transform target;

    [Header("Lantern Vision")]
    public float viewDistance = 5f;
    public float viewAngle = 45f;

    [Header("Patrol")]
    public float patrolWidth = 4f;
    public float patrolHeight = 4f;
    public float patrolChangeTime = 3f;

    [Header("Animation")]
    private Animator animator;

    private Vector2 startPosition;
    private float patrolTimer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        startPosition = transform.position;

        if (visionCone)
        {
            visionConeRenderer = visionCone.GetComponent<SpriteRenderer>();
            visionConeRenderer.color = patrolColor;
        }

        ChooseNewPatrolDirection();
    }

    private void Update()
    {
        switch (currentState)
        {
            case EnemyState.Patrol:
                PatrolTimerUpdate();
                LanternDetection();
                break;

            case EnemyState.Chase:
                CheckLostPlayer();
                break;
        }

        UpdateVisionCone();
        UpdateAnimator();
    }

    void UpdateAnimator()
    {
        if (!animator) return;

        Vector2 velocity = rb.linearVelocity;

        bool isWalking = velocity.sqrMagnitude > 0.01f;

        animator.SetBool("isWalking", isWalking);

        if (isWalking)
        {
            Vector2 dir = velocity.normalized;
            animator.SetFloat("InputX", dir.x);
            animator.SetFloat("InputY", dir.y);
        }
    }


    private void FixedUpdate()
    {
        switch (currentState)
        {
            case EnemyState.Patrol:
                PatrolMovement();
                break;

            case EnemyState.Chase:
                ChasePlayer();
                break;
        }
    }

    void ChooseNewPatrolDirection()
    {
        Vector2[] eightDirections = {
            Vector2.up,
            (Vector2.up + Vector2.right).normalized,
            Vector2.right,
            (Vector2.down + Vector2.right).normalized,
            Vector2.down,
            (Vector2.down + Vector2.left).normalized,
            Vector2.left,
            (Vector2.up + Vector2.left).normalized
        };

        moveDirection = eightDirections[Random.Range(0, eightDirections.Length)];
        patrolTimer = patrolChangeTime;
    }

    void PatrolTimerUpdate()
    {
        patrolTimer -= Time.deltaTime;

        if (patrolTimer <= 0f)
            ChooseNewPatrolDirection();
    }

    void PatrolMovement()
    {
        Vector2 currentPos = rb.position;

        bool withinWidth = Mathf.Abs(currentPos.x - startPosition.x) <= patrolWidth / 2f;
        bool withinHeight = Mathf.Abs(currentPos.y - startPosition.y) <= patrolHeight / 2f;

        if (withinWidth && withinHeight)
        {
            rb.linearVelocity = moveDirection * moveSpeed;
        }
        else
        {
            Vector2 toCenter = (startPosition - currentPos).normalized;

            if (!withinWidth && withinHeight)
                toCenter = new Vector2(Mathf.Sign(toCenter.x), 0);
            else if (withinWidth && !withinHeight)
                toCenter = new Vector2(0, Mathf.Sign(toCenter.y));

            moveDirection = toCenter;
            rb.linearVelocity = toCenter * moveSpeed;
        }
    }

    void ChasePlayer()
    {
        if (!target) return;

        Vector2 dir = ((Vector2)target.position - rb.position).normalized;
        moveDirection = dir;
        rb.linearVelocity = dir * chaseSpeed;
    }

    void CheckLostPlayer()
    {
        if (!target)
        {
            ReturnToPatrol();
            return;
        }

        Vector2 directionToTarget = target.position - transform.position;
        float distanceToTarget = directionToTarget.magnitude;

        if (distanceToTarget > viewDistance)
        {
            ReturnToPatrol();
            return;
        }

        float angleToTarget = Vector2.Angle(moveDirection, directionToTarget);

        if (angleToTarget > viewAngle / 2f)
            ReturnToPatrol();
    }

    void LanternDetection()
    {
        if (!target) return;

        Vector2 directionToTarget = target.position - transform.position;
        float distanceToTarget = directionToTarget.magnitude;

        if (distanceToTarget > viewDistance) return;

        Vector2 facingDirection = rb.linearVelocity.normalized;
        if (facingDirection == Vector2.zero)
            facingDirection = moveDirection;

        float angleToTarget = Vector2.Angle(facingDirection, directionToTarget);

        if (angleToTarget <= viewAngle / 2f)
        {
            currentState = EnemyState.Chase;

            if (visionConeRenderer)
                visionConeRenderer.color = alertColor;

            if (FindFirstObjectByType<GameManager>() != null)
                FindFirstObjectByType<GameManager>().GameLost();
        }
    }

    void ReturnToPatrol()
    {
        currentState = EnemyState.Patrol;
        ChooseNewPatrolDirection();

        if (visionConeRenderer)
            visionConeRenderer.color = patrolColor;
    }

    void UpdateVisionCone()
    {
        if (!visionCone) return;

        Vector2 facingDirection = rb.linearVelocity.normalized;

        if (facingDirection == Vector2.zero)
            facingDirection = moveDirection;

        if (facingDirection == Vector2.zero)
            return;

        float angle = Mathf.Atan2(facingDirection.y, facingDirection.x) * Mathf.Rad2Deg;
        visionCone.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}
