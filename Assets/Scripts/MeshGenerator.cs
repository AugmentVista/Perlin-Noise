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

    public int textureWidth = 1024;
    public int textureHeight = 1024;

    [Header("Noise Settings")]
    public int octaves = 4;
    [Range(0, 1)]
    public float persistence = 0.5f; // Amplitude multiplier per octave
    public float baseFrequency = 0.2f; // Starting frequency
    public float baseAmplitude = 1.0f; // Starting amplitude

    public float yIntensity;

    private Vector2 offset;

    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        StartCoroutine(CreateShape());
    }

    private void Update()
    {
        CreateShape();
        UpdateMesh();
    }

    IEnumerator CreateShape()
    {
        vertices = new Vector3[(gridX + 1) * (gridZ + 1)];

        for (int i = 0, z = 0; z <= gridZ; z++)
        {
            for (int x = 0; x <= gridX; x++)
            {
                float y = GetNoiseSample(x, z); // y is between 0 - 1 as a result of float noiseHeight / maxHeight
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
            }
            vertex++;
            yield return new WaitForSeconds(.01f);
        }

        for (int i = 0, z = 0; z <= gridZ; z++)
        {
            for (int x = 0; x <= gridX; x++)
            {
                i++;
            }
        }
    }

    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;


        mesh.RecalculateNormals();
    }

    float GetNoiseSample(int x, int z)
    {
        float amplitude = baseAmplitude;
        float frequency = baseFrequency;
        float noiseHeight = 0f;

        float maxPossibleHeight = 0f;
        for (int i = 0; i < octaves; i++)
        {
            maxPossibleHeight += amplitude;
            amplitude *= persistence;
        }

        amplitude = baseAmplitude;

        for (int i = 0; i < octaves; i++)
        {
            float sampleX = (x + offset.x) * frequency;
            float sampleZ = (z + offset.y) * frequency;

            float perlinValue = Mathf.PerlinNoise(sampleX, sampleZ);
            noiseHeight += perlinValue * amplitude;

            amplitude *= persistence;
            frequency *= 2;
        }

        // Normalize the result to [0, 1]
        noiseHeight /= maxPossibleHeight;

        return noiseHeight * yIntensity;
    }
}
