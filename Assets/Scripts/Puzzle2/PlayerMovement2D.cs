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
    public float flipDeadzone = 0.01f;

    [Header("Animation")]
    public Animator animator; // drag player animator here (or leave empty)

    private Rigidbody2D rb;
    private float moveInput;
    private bool isGrounded;

    private bool isFacingRight;
    private float baseScaleX;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (!animator) animator = GetComponentInChildren<Animator>();

        baseScaleX = Mathf.Abs(transform.localScale.x);
        isFacingRight = startFacingRight;

        //ApplyFacing();
        UpdateAnimatorFacing();
    }

    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");

        // Flip logic
        if (moveInput > flipDeadzone && !isFacingRight)
        {
            isFacingRight = true;
            //ApplyFacing();
            UpdateAnimatorFacing();
        }
        else if (moveInput < -flipDeadzone && isFacingRight)
        {
            isFacingRight = false;
            //ApplyFacing();
            UpdateAnimatorFacing();
        }

        // Ground check
        if (groundCheck != null)
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        // Jump cut
        if (Input.GetButtonUp("Jump") && rb.linearVelocity.y > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * jumpCutMultiplier);
        }

        // Animator params
        if (animator)
        {
            float speedAbs = Mathf.Abs(moveInput);
            animator.SetFloat("Speed", speedAbs);
            animator.SetBool("FacingRight", isFacingRight);
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }

    private void UpdateAnimatorFacing()
    {
        if (!animator) return;
        animator.SetBool("FacingRight", isFacingRight);
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
