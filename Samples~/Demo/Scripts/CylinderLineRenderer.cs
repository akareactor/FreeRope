using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class CylinderLineRenderer : MonoBehaviour {
    public Transform[] points;
    public float radius = 0.1f;
    public int segments = 8;

    private LineRenderer lineRenderer;

    void Start () {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = points.Length;
        UpdateLineRenderer();
    }

    void Update () {
        UpdateLineRenderer();
    }

    private void UpdateLineRenderer () {
        for (int i = 0; i < points.Length; i++) {
            lineRenderer.SetPosition(i, points[i].position);
        }

        Mesh mesh = GenerateCylinderMesh();
        if (mesh != null) {
            MeshFilter meshFilter = GetComponent<MeshFilter>();
            if (meshFilter == null) {
                meshFilter = gameObject.AddComponent<MeshFilter>();
            }
            meshFilter.mesh = mesh;
            MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
            if (meshRenderer == null) {
                meshRenderer = gameObject.AddComponent<MeshRenderer>();
            }
        }
    }

    private Mesh GenerateCylinderMesh () {
        if (points.Length < 2) {
            Debug.LogWarning("Need at least two points to generate a mesh.");
            return null;
        }

        Mesh mesh = new Mesh();
        Vector3[] vertices = new Vector3[points.Length * (segments + 1)];
        int[] triangles = new int[(points.Length - 1) * segments * 6];

        for (int i = 0; i < points.Length; i++) {
            Vector3 center = points[i].position;
            for (int j = 0; j <= segments; j++) {
                float angle = 2 * Mathf.PI * j / segments;
                Vector3 offset = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
                vertices[i * (segments + 1) + j] = center + offset;
            }
        }

        int triangleIndex = 0;
        for (int i = 0; i < points.Length - 1; i++) {
            for (int j = 0; j < segments; j++) {
                int start = i * (segments + 1);
                triangles[triangleIndex++] = start + j;
                triangles[triangleIndex++] = start + j + segments + 1;
                triangles[triangleIndex++] = start + j + 1;

                triangles[triangleIndex++] = start + j + 1;
                triangles[triangleIndex++] = start + j + segments + 1;
                triangles[triangleIndex++] = start + j + segments + 2;
            }
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        return mesh;
    }
}
