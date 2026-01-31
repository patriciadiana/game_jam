using UnityEngine;

public class PlayerPush : MonoBehaviour
{
    [SerializeField] private float pushSpeed = 2f;

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Pushable"))
            return;

        Rigidbody2D boxRb = collision.gameObject.GetComponent<Rigidbody2D>();
        if (!boxRb || boxRb.bodyType != RigidbodyType2D.Kinematic)
            return;

        Vector2 pushDir = -collision.contacts[0].normal;

        Vector2 newPos = boxRb.position + pushDir * pushSpeed * Time.fixedDeltaTime;
        boxRb.MovePosition(newPos);
    }
}
