using UnityEngine;

public class Spikes : MonoBehaviour
{

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("am murit");

            collision.gameObject.SetActive(false);
            GameManager gm = FindFirstObjectByType<GameManager>();
            if (gm != null)
                gm.GameLost();
            else
                Debug.Log("miau");
        }
    }
}
