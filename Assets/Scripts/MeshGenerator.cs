using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{
    private Mesh mesh;

    private Vector3[] vertices;

    private int[] triangles;

    public int gridX;

    public int gridZ;

    public float xIntensity;

    public float zIntensity;

    public float yIntensity;


    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

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
                float y = Mathf.PerlinNoise(x * xIntensity, z * zIntensity) * yIntensity;
                vertices[i] = new Vector3(x,y,z);
                i++;
            }
        }

        triangles = new int[gridX * gridZ * 6];

        int vertex = 0;
        int tris = 0;

        for (int z = 0; z < gridZ; z++)
        {
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
                yield return new WaitForSeconds(.0001f);
            }
            vertex++;
        }
    }

    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
    }


}
