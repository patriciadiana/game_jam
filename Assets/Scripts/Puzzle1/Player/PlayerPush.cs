using UnityEngine;

public class PlayerPush : MonoBehaviour
{
    [SerializeField] private float pushForce = 5f;

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Pushable"))
        {
            Rigidbody2D pushableRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (pushableRb != null && pushableRb.bodyType == RigidbodyType2D.Kinematic)
            {
                Vector2 pushDir = collision.contacts[0].normal * -1;

                pushableRb.MovePosition(pushableRb.position + pushDir * pushForce * Time.fixedDeltaTime);
            }
        }
    }
}