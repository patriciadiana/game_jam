using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody2D rb;
    private Vector2 input;

    private Animator animator;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        input.Normalize();

        bool isWalking = input.sqrMagnitude > 0; 
        animator.SetBool("isWalking", isWalking);
        animator.SetFloat("InputX", input.x);
        animator.SetFloat("InputY", input.y);

        if (isWalking)
        {
            animator.SetFloat("LastInputX", input.x);
            animator.SetFloat("LastInputY", input.y);
        }
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = input * speed;
    }
}
