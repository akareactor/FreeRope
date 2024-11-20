using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KulibinSpace.FreeRope {

    public class PrimitiveGenerator {

        // цилиндр рисуется в локальных координатах объекта и ориентирован по оси Z
        
        public static void GenerateCylinder (GameObject go, int radialSegments, int heightSegments, float radius, float length) {
            MeshFilter meshFilter = go.GetComponent<MeshFilter>();
            Mesh mesh = new Mesh();

            int vertexCount = (radialSegments + 1) * (heightSegments + 1);
            Vector3[] vertices = new Vector3[vertexCount];
            int[] triangles = new int[radialSegments * heightSegments * 6];
            Vector2[] uv = new Vector2[vertexCount];

            int vertIndex = 0;
            int triIndex = 0;

            // Generate vertices
            for (int z = 0; z <= heightSegments; z++) {
                float zPos = (float)z / heightSegments * length;
                for (int i = 0; i <= radialSegments; i++) {
                    float angle = (float)i / radialSegments * Mathf.PI * 2;
                    float x = Mathf.Cos(angle) * radius;
                    float y = Mathf.Sin(angle) * radius;
                    vertices[vertIndex] = new Vector3(x, y, zPos);
                    uv[vertIndex] = new Vector2((float)i / radialSegments, (float)z / heightSegments);
                    vertIndex++;
                }
            }

            // Generate triangles
            for (int z = 0; z < heightSegments; z++) {
                for (int i = 0; i < radialSegments; i++) {
                    int current = z * (radialSegments + 1) + i;
                    int next = current + radialSegments + 1;

                    triangles[triIndex++] = current;
                    triangles[triIndex++] = current + 1;
                    triangles[triIndex++] = next;

                    triangles[triIndex++] = current + 1;
                    triangles[triIndex++] = next + 1;
                    triangles[triIndex++] = next;
                }
            }

            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uv;

            mesh.RecalculateNormals();
            meshFilter.mesh = mesh;
        }

    }

}
