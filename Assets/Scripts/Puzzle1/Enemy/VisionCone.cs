using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class VisionCone : MonoBehaviour
{
    public float viewDistance = 5f;
    public float viewAngle = 45f;
    public int segments = 10;

    [HideInInspector] public Vector2 facingDirection = Vector2.up;

    private Mesh mesh;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    void Start()
    {
        InitializeMesh();
    }

    void Update()
    {
        DrawCone();
    }

    void InitializeMesh()
    {
        // Get or add required components
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();

        // Create a new mesh
        mesh = new Mesh();
        mesh.name = "VisionConeMesh";

        // Assign the mesh to the filter
        meshFilter.mesh = mesh;

        // Set up the material
        if (meshRenderer.material == null)
        {
            meshRenderer.material = new Material(Shader.Find("Unlit/Color"));
            meshRenderer.material.color = new Color(1f, 0f, 0f, 0.5f); // Semi-transparent red
        }
    }

    void DrawCone()
    {
        if (mesh == null)
        {
            InitializeMesh();
            return;
        }

        int vertexCount = segments + 2;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[segments * 3];

        // Center vertex at local zero
        vertices[0] = Vector3.zero;

        // Calculate facing angle
        float facingAngle = Mathf.Atan2(facingDirection.y, facingDirection.x) * Mathf.Rad2Deg;

        float angleStep = viewAngle / segments;
        float startAngle = facingAngle - viewAngle / 2f;

        // Create vertices around the cone
        for (int i = 0; i <= segments; i++)
        {
            float angle = startAngle + i * angleStep;
            float rad = Mathf.Deg2Rad * angle;
            vertices[i + 1] = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0) * viewDistance;
        }

        // Create triangles
        for (int i = 0; i < segments; i++)
        {
            triangles[i * 3] = 0;
            triangles[i * 3 + 1] = i + 1;
            triangles[i * 3 + 2] = i + 2;
        }

        // Apply to mesh
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        // Add normals for proper rendering
        Vector3[] normals = new Vector3[vertexCount];
        for (int i = 0; i < vertexCount; i++)
        {
            normals[i] = Vector3.forward; // Pointing forward for 2D
        }
        mesh.normals = normals;

        mesh.RecalculateBounds();
        mesh.Optimize();
    }

    void OnValidate()
    {
        // Redraw when values change in the inspector
        if (mesh != null)
        {
            DrawCone();
        }
    }
}