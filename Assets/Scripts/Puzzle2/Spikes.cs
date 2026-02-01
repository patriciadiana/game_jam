using UnityEngine;

public class Spikes : MonoBehaviour
{

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.SetActive(false);
            GameManager gm = FindFirstObjectByType<GameManager>();
            if (gm != null)
                gm.GameOver();
        }
    }
}
