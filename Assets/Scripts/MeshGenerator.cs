using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{
    Mesh mesh;

    Vector3[] vertices;

    int[] triangles;

    int gridX = 20;

    int gridZ = 20;


    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        CreateShape();
        StartCoroutine(CreateShape());
    }

    private void Update()
    {
        UpdateMesh();
    }

    IEnumerator CreateShape()
    {
        vertices = new Vector3[(gridX + 1) * (gridZ + 1)];

        for (int i = 0, z = 0; z <= gridZ; z++)
        {
            for (int x = 0; x <= gridX; x++)
            {
                vertices[i] = new Vector3(x,0,z);
                i++;
            }
        }

        triangles = new int[gridX * gridZ * 6];

        int vertex = 0;
        int tris = 0;
        for (int x = 0; x < gridX; x++)
        {
            triangles[tris + 0] = vertex + 0;
            triangles[tris + 1] = vertex + gridX + 1;
            triangles[tris + 2] = vertex + 1;
            triangles[tris + 3] = vertex + 1;
            triangles[tris + 4] = vertex + gridX + 1;
            triangles[tris + 5] = vertex + gridX + 2;

            vertex++;
            tris += 6;
            yield return new WaitForSeconds(.1f);
        }
    }

    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
    }

    private void OnDrawGizmos()
    {
        if (vertices == null)
            return;

        for (int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i], .1f);
        }
    }


}
