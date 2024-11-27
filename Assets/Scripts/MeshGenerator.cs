using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{
    private Mesh mesh;

    public Gradient gradient;

    private Vector3[] vertices;
    Vector2[] uvs;
    Vector2 offset;

    Color[] colors;

    private int[] triangles;

    public int gridX;
    public int gridZ;

    public int textureWidth = 1024;
    public int textureHeight = 1024;

    private float noise01Scale = 0.2f;
    private float noise01Amp = 0.2f;

    private float noise02Scale = 0.4f;
    private float noise02Amp = 0.4f;

    private float noise03Scale = 0.6f;
    private float noise03Amp = 0.6f;

    [Header("Noise Settings")]
    public int octaves = 4;
    [Range(0, 1)]
    public float persistence = 0.5f; // Amplitude multiplier per octave
    public float baseFrequency = 0.2f; // Starting frequency
    public float baseAmplitude = 1.0f; // Starting amplitude
    public int seed = 0;

    public float yIntensity;

    float minTerrainHeight;
    float maxTerrainHeight;
    float maxPossibleHeight = 0f;


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
                float y = GetNoiseSample(x, z); 
                vertices[i] = new Vector3(x,y,z);

                maxTerrainHeight = maxPossibleHeight;

                if (y > maxTerrainHeight)
                    maxTerrainHeight = y;

                if (y < minTerrainHeight)
                    minTerrainHeight = y;

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

        colors = new Color[vertices.Length];

        for (int i = 0, z = 0; z <= gridZ; z++)
        {
            for (int x = 0; x <= gridX; x++)
            {
                float height = Mathf.InverseLerp(minTerrainHeight, maxTerrainHeight, vertices[i].y);
                colors[i] = gradient.Evaluate(height);
                i++;
            }
        }
    }

    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.colors = colors;

        mesh.RecalculateNormals();
    }

    float GetNoiseSample(int x, int z)
    {
        float amplitude = baseAmplitude;
        float frequency = baseFrequency;
        float noiseHeight = 0f;

        // Calculate the maximum possible height for normalization
        
        for (int i = 0; i < octaves; i++)
        {
            maxPossibleHeight += amplitude;
            amplitude *= persistence;
        }

        amplitude = baseAmplitude;
        frequency = baseFrequency;

        for (int i = 0; i < octaves; i++)
        {
            float sampleX = (x + offset.x) * frequency;
            float sampleZ = (z + offset.y) * frequency;

            float perlinValue = Mathf.PerlinNoise(sampleX, sampleZ);
            noiseHeight += perlinValue * amplitude;

            amplitude *= persistence; // Decrease amplitude for next octave
            frequency *= 2; // Increase frequency for next octave
        }

        // Normalize the result to [0, 1]
        noiseHeight /= maxPossibleHeight;

        return noiseHeight * yIntensity;
    }


}
