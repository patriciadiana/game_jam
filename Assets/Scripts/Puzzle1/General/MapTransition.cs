using UnityEngine;
using Unity.Cinemachine;

public class MapTransition : MonoBehaviour
{
    [SerializeField] private PolygonCollider2D mapBoundary;
    [SerializeField] private float additivePos = 2f;      

    private CinemachineConfiner2D confiner;

    public enum Direction { Down, Left }
    public Direction direction;

    private void Awake()
    {
        confiner = FindFirstObjectByType<CinemachineConfiner2D>();
        if (confiner == null)
            Debug.LogError("No CinemachineConfiner2D found in the scene!");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (confiner != null && mapBoundary != null)
            {
                confiner.BoundingShape2D = mapBoundary;
            }

            Vector3 newPos = collision.transform.position;

            switch (direction)
            {
                case Direction.Down:
                    newPos.y -= additivePos;
                    break;
                case Direction.Left:
                    newPos.x -= additivePos;
                    break;
            }

            collision.transform.position = newPos;
        }
    }
}
