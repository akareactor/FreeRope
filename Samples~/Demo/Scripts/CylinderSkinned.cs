using UnityEngine;

public class CylinderSkinned : MonoBehaviour {

    public Transform pointA;
    public Transform pointB;
    public int radialSegments = 16;
    public int heightSegments = 1;
    public float radius = 0.5f;

    private void Start () {
        GenerateCylinder();
    }

    void Update () {
        UpdateBones();
    }

    private void UpdateBones () {
        //        for (int i = 0; i < points.Length; i++) {
        //            skinnedMeshRenderer.bones[i].position = points[i].position;
        //        }
    }

    private void GenerateCylinder()
    {
        if (pointA == null || pointB == null)
        {
            Debug.LogError("PointA and PointB must be assigned.");
            return;
        }

        Vector3 direction = pointB.position - pointA.position;
        float height = direction.magnitude;
        direction.Normalize();

        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[(radialSegments + 1) * (heightSegments + 1)];
        int[] triangles = new int[radialSegments * heightSegments * 6];
        Vector2[] uv = new Vector2[vertices.Length];
        BoneWeight[] boneWeights = new BoneWeight[vertices.Length];
        Transform[] bones = new Transform[heightSegments + 1];
        Matrix4x4[] bindPoses = new Matrix4x4[heightSegments + 1];

        float angleStep = 2 * Mathf.PI / radialSegments;
        float heightStep = height / heightSegments;

        int vertIndex = 0;
        int triIndex = 0;

        for (int i = 0; i <= heightSegments; i++)
        {
            // Создаем кости для каждого сегмента высоты
            GameObject boneObject = new GameObject("Bone" + i);
            boneObject.transform.parent = transform;
            boneObject.transform.localPosition = new Vector3(0, 0, i * heightStep);
            bones[i] = boneObject.transform;
            bindPoses[i] = bones[i].worldToLocalMatrix * transform.localToWorldMatrix;

            for (int j = 0; j <= radialSegments; j++)
            {
                float angle = j * angleStep;
                float x = Mathf.Cos(angle) * radius;
                float y = Mathf.Sin(angle) * radius;
                float z = i * heightStep;
                vertices[vertIndex] = new Vector3(x, y, z);

                uv[vertIndex] = new Vector2((float)j / radialSegments, (float)i / heightSegments);

                // Назначаем веса костям
                boneWeights[vertIndex].boneIndex0 = i;
                boneWeights[vertIndex].weight0 = 1.0f;

                if (i < heightSegments && j < radialSegments)
                {
                    int current = vertIndex;
                    int next = vertIndex + radialSegments + 1;

                    triangles[triIndex++] = current;
                    triangles[triIndex++] = current + 1;
                    triangles[triIndex++] = next;

                    triangles[triIndex++] = current + 1;
                    triangles[triIndex++] = next + 1;
                    triangles[triIndex++] = next;
                }

                vertIndex++;
            }
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;
        mesh.boneWeights = boneWeights;
        mesh.bindposes = bindPoses;
        mesh.RecalculateNormals();

        MeshFilter meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = mesh;

        SkinnedMeshRenderer skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
        skinnedMeshRenderer.sharedMesh = mesh;
        skinnedMeshRenderer.bones = bones;
        skinnedMeshRenderer.rootBone = transform;

        // Поворот цилиндра по направлению
        transform.position = pointA.position;
        transform.rotation = Quaternion.LookRotation(direction);

        // Масштабирование цилиндра по его длине
        transform.localScale = new Vector3(1, 1, height);
    }

}


