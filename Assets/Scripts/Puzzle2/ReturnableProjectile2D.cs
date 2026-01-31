using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class ReturnableProjectile2D : MonoBehaviour
{
    [Header("Return")]
    public float returnSpeed = 18f;
    public float arriveDistance = 0.08f;

    [Header("Physics Feel")]
    public float GravityScale = 1f;

    private Rigidbody2D rb;
    private Collider2D col;

    private Transform holdPoint;
    private bool isHeld = true;
    private bool isReturning = false;

    private Action onReturned;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    void FixedUpdate()
    {
        if (!isReturning || holdPoint == null) return;

        Vector2 current = rb.position;
        Vector2 target = holdPoint.position;

        Vector2 next = Vector2.MoveTowards(current, target, returnSpeed * Time.fixedDeltaTime);
        rb.MovePosition(next);

        if (Vector2.Distance(next, target) <= arriveDistance)
        {
            FinishReturn();
        }
    }

    public void SetHoldPoint(Transform hp)
    {
        holdPoint = hp;
    }

    public void AttachToHoldPoint()
    {
        if (holdPoint == null)
        {
            Debug.LogError("ReturnableProjectile2D: HoldPoint is null. Did you call SetHoldPoint?");
            return;
        }

        isHeld = true;
        isReturning = false;

        col.enabled = false;

        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.isKinematic = true;
        rb.gravityScale = 0f;

        transform.SetParent(holdPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    public void Shoot(Vector2 initialVelocity)
    {
        if (!isHeld || isReturning) return;

        transform.SetParent(null);

        rb.isKinematic = false;
        rb.gravityScale = GravityScale;
        col.enabled = true;

        rb.linearVelocity = initialVelocity;

        isHeld = false;
        isReturning = false;
    }

    public void BeginReturn(Action onReturnedCallback)
    {
        if (isHeld) return;
        if (isReturning) return;

        if (holdPoint == null)
        {
            Debug.LogError("ReturnableProjectile2D: HoldPoint is null. Can't return.");
            return;
        }

        onReturned = onReturnedCallback;

        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.gravityScale = 0f;

        col.enabled = false;

        isReturning = true;
    }

    private void FinishReturn()
    {
        isReturning = false;

        AttachToHoldPoint();

        onReturned?.Invoke();
        onReturned = null;
    }
}
