using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    private Mesh mesh;

    public float Fov { get; set; }
    public float ViewDistance { get; set; }
    public Vector3 Origin => Vector3.zero;

    private void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    private void Update()
    {
        int rayCount = 50;
        float angleIncrease = Fov / rayCount;

        Vector3[] vertices = new Vector3[rayCount + 1 + 1];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[rayCount * 3];

        vertices[0] = Origin;

        int vertexIndex = 1;
        int triangleIndex = 0;
        for (int i = 0; i <= rayCount; i++)
        {
            Vector3 vertex;

            Vector3 direction = Quaternion.AngleAxis(-(Fov / 2) + (angleIncrease * i), Vector3.up) * transform.forward;

            Physics.Raycast(transform.position, direction, out RaycastHit raycastHit, ViewDistance, layerMask);

            if (raycastHit.collider == null)
            {
                // No hit
                Vector3 vertexDirection = Quaternion.AngleAxis(-(Fov / 2) + (angleIncrease * i), Vector3.up) * Vector3.forward;
                vertex = Origin + vertexDirection * ViewDistance;
            }
            else
            {
                // Hit object
                vertex = transform.InverseTransformPoint(raycastHit.point);
            }
            vertices[vertexIndex] = vertex;

            if (i > 0)
            {
                triangles[triangleIndex + 0] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex;

                triangleIndex += 3;
            }

            vertexIndex++;
        }


        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        mesh.bounds = new Bounds(Origin, Vector3.one * 1000f);
    }
}
