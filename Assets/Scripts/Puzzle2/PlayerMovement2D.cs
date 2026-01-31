using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement2D : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 6f;

    [Header("Jump")]
    public float jumpForce = 12f;
    public float jumpCutMultiplier = 0.5f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.15f;
    public LayerMask groundLayer;

    [Header("Facing")]
    public bool startFacingRight = true;
    public float flipDeadzone = 0.01f; // prevents jitter when input is ~0

    private Rigidbody2D rb;
    private float moveInput;
    private bool isGrounded;

    private bool isFacingRight;
    private float baseScaleX;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        // store original scale so we don't mess up your sizing
        baseScaleX = Mathf.Abs(transform.localScale.x);
        isFacingRight = startFacingRight;

        ApplyFacing();
    }

    void Update()
    {
        // Horizontal movement (A/D or arrows)
        moveInput = Input.GetAxisRaw("Horizontal");

        // Flip based on input direction
        if (moveInput > flipDeadzone && !isFacingRight)
        {
            isFacingRight = true;
            ApplyFacing();
        }
        else if (moveInput < -flipDeadzone && isFacingRight)
        {
            isFacingRight = false;
            ApplyFacing();
        }

        // Ground check
        if (groundCheck != null)
            isGrounded = Physics2D.OverlapCircle(
                groundCheck.position,
                groundCheckRadius,
                groundLayer
            );

        // Jump start
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        // Jump cut
        if (Input.GetButtonUp("Jump") && rb.linearVelocity.y > 0)
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                rb.linearVelocity.y * jumpCutMultiplier
            );
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }

    private void ApplyFacing()
    {
        Vector3 s = transform.localScale;
        s.x = baseScaleX * (isFacingRight ? 1f : -1f);
        transform.localScale = s;
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
