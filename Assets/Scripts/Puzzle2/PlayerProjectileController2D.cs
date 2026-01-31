using UnityEngine;

public class PlayerProjectileController2D : MonoBehaviour
{
    [Header("References")]
    public Transform holdPoint;
    public ReturnableProjectile2D projectile;
    public LineRenderer lineRenderer;

    [Header("Throw")]
    public float throwSpeed = 14f;
    public int trajectoryPoints = 25;
    public float timeStep = 0.05f;
    public LayerMask collisionMask;

    [Header("Charge Throw")]
    public float minThrowSpeed = 6f;
    public float maxThrowSpeed = 18f;
    public float chargeSpeed = 12f;

    private Camera cam;
    private bool canShoot = true;
    private float currentThrowSpeed;
    private bool isCharging;

    void Awake()
    {
        cam = Camera.main;

        if (lineRenderer != null)
        {
            lineRenderer.enabled = false;
            lineRenderer.positionCount = 0;
        }

        if (projectile != null && holdPoint != null)
        {
            projectile.SetHoldPoint(holdPoint);
            projectile.AttachToHoldPoint();
        }
    }

    void Update()
    {
        if (projectile == null || holdPoint == null) return;

        if (canShoot && Input.GetMouseButton(1))
        {
            ShowTrajectory(currentThrowSpeed > 0 ? currentThrowSpeed : minThrowSpeed);
        }
        else
        {
            HideTrajectory();
        }

        if (canShoot && Input.GetMouseButtonDown(0))
        {
            isCharging = true;
            currentThrowSpeed = minThrowSpeed;
        }

        if (canShoot && isCharging && Input.GetMouseButton(0))
        {
            currentThrowSpeed += chargeSpeed * Time.deltaTime;
            currentThrowSpeed = Mathf.Clamp(currentThrowSpeed, minThrowSpeed, maxThrowSpeed);
        }

        if (canShoot && isCharging && Input.GetMouseButtonUp(0))
        {
            Vector2 dir = GetAimDirection();
            projectile.Shoot(dir * currentThrowSpeed);

            isCharging = false;
            currentThrowSpeed = 0f;
            canShoot = false;
            HideTrajectory();
        }

        if (!canShoot && Input.GetKeyDown(KeyCode.E))
        {
            projectile.BeginReturn(OnProjectileReturned);
        }
    }

    private void OnProjectileReturned()
    {
        canShoot = true;
    }

    private Vector2 GetAimDirection()
    {
        Vector3 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0f;

        Vector2 from = holdPoint.position;
        Vector2 to = mouseWorld;

        Vector2 dir = (to - from).normalized;
        if (dir.sqrMagnitude < 0.0001f) dir = Vector2.right;

        return dir;
    }

    private void ShowTrajectory(float speed)
    {
        if (lineRenderer == null) return;

        lineRenderer.enabled = true;
        lineRenderer.positionCount = trajectoryPoints;

        Vector2 startPos = holdPoint.position;
        Vector2 startVel = GetAimDirection() * speed;
        Vector2 gravity = Physics2D.gravity * projectile.GravityScale;

        Vector2 prevPos = startPos;

        for (int i = 0; i < trajectoryPoints; i++)
        {
            float t = i * timeStep;

            Vector2 pos = startPos + startVel * t + 0.5f * gravity * (t * t);

            lineRenderer.SetPosition(i, pos);

            if (i > 0)
            {
                Vector2 segment = pos - prevPos;
                float dist = segment.magnitude;

                if (dist > 0.0001f)
                {
                    RaycastHit2D hit = Physics2D.Raycast(prevPos, segment.normalized, dist, collisionMask);
                    if (hit.collider != null)
                    {
                        lineRenderer.SetPosition(i, hit.point);

                        for (int j = i + 1; j < trajectoryPoints; j++)
                            lineRenderer.SetPosition(j, hit.point);

                        break;
                    }
                }
            }

            prevPos = pos;
        }
    }

    private void HideTrajectory()
    {
        if (lineRenderer == null) return;
        lineRenderer.enabled = false;
        lineRenderer.positionCount = 0;
    }
}
