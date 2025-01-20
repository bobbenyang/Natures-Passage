using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class EnemyVisionWithColor : MonoBehaviour
{
    public Transform player;
    public Transform startingPoint; // Reference to the starting area
    public float detectionRadius = 5f; // Radius of the enemy's vision
    public float detectionAngle = 45f; // Angle of the vision cone

    public Color visionColor = new Color(1f, 0f, 0f, 0.5f); // Red color with transparency

    private Mesh visionMesh;

    void Start()
    {
        // Create a new mesh for the vision cone
        visionMesh = new Mesh();
        GetComponent<MeshFilter>().mesh = visionMesh;

        // Set up the material
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        renderer.material = new Material(Shader.Find("Unlit/Color"))
        {
            color = visionColor
        };
    }

    void Update()
    {
        DrawVisionCone();
        DetectPlayer();
    }

    void DetectPlayer()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        // Check if the player is within the radius
        if (distanceToPlayer <= detectionRadius)
        {
            // Check if the player is within the angle
            float angle = Vector3.Angle(transform.forward, directionToPlayer);
            if (angle <= detectionAngle / 2)
            {
                Debug.Log("Player Detected! Teleporting...");
                TeleportPlayer();
            }
        }
    }

    void TeleportPlayer()
    {
        player.position = startingPoint.position;
    }

    void DrawVisionCone()
    {
        int segments = 50; // Number of segments to make the cone smooth
        Vector3[] vertices = new Vector3[segments + 2];
        int[] triangles = new int[segments * 3];

        // Set the first vertex at the origin (enemy's position)
        vertices[0] = Vector3.zero;

        float angleStep = detectionAngle / segments;
        Quaternion rotation = Quaternion.Euler(0, -detectionAngle / 2, 0);

        // Create vertices
        for (int i = 0; i <= segments; i++)
        {
            Vector3 vertex = rotation * Vector3.forward * detectionRadius;
            vertices[i + 1] = vertex;

            if (i < segments)
            {
                // Create triangles
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }

            rotation *= Quaternion.Euler(0, angleStep, 0);
        }

        // Apply vertices and triangles to the mesh
        visionMesh.Clear();
        visionMesh.vertices = vertices;
        visionMesh.triangles = triangles;
        visionMesh.RecalculateNormals();
    }

    // Optional: Visualize the vision radius in the Scene view
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
