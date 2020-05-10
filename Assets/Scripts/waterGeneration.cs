using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class waterGeneration : MonoBehaviour
{
    Mesh mesh;//==> Mesh itself
    Vector3[] vertices; //==> Points of triangles
    int[] triangles; //==> Triangles that make a mesh

    public bool animate = true;

    public float xOffset = 0;
    public float zOffset = 0;

    public int xSize; //==> Width of the mesh
    public int zSize; // ==> Length of the mesh
    public float amplitude;
    public float frequency;
    public float persistance;
    public int octaves;

    float minHeight;
    float maxHeight;


    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        createShape();
        updateMesh();
    }

    void Update()
    {
  
       xOffset = Time.time; // ==> Makes the waves by offseting the mesh in time intervals
       zOffset = Time.time;
       updateMesh();

    }

    void createShape()
    {
        vertices = new Vector3[xSize * zSize * 6];
        triangles = new int[xSize * zSize * 6];

        int index = 0;
        int indexTriangles = 0;
        for (int i = 0; i < zSize; i++)
        {
            for (int j = 0; j < xSize; j++)
            {

                float y1 = calculatePerlinOstaves(j, i, octaves, persistance);
                float y2 = calculatePerlinOstaves(j + 1, i, octaves, persistance);
                float y3 = calculatePerlinOstaves(j, i + 1, octaves, persistance);
                float y4 = calculatePerlinOstaves(j + 1, i + 1, octaves, persistance);

                vertices[index] = new Vector3(j, y1, i);
                vertices[index + 1] = new Vector3(j, y3, i + 1);
                vertices[index + 2] = new Vector3(j + 1, y2, i);
                vertices[index + 3] = new Vector3(j + 1, y2, i);
                vertices[index + 4] = new Vector3(j, y3, i + 1);
                vertices[index + 5] = new Vector3(j + 1, y4, i + 1);

                triangles[index] = index;
                triangles[index + 1] = index + 1;
                triangles[index + 2] = index + 2;
                triangles[index + 3] = index + 3;
                triangles[index + 4] = index + 4;
                triangles[index + 5] = index + 5;

                index += 6;
                indexTriangles += 6;
            }
        }
    }


    float calculatePerlinOstaves(int x, int z, int octaves, float persistance)
    {
        // same as in terrain generation
        float total = 0;
        float amplitudeX = amplitude;
        float frequencyX = frequency;
        for (int i = 0; i < octaves; i++)
        {
            total += Mathf.PerlinNoise(x * frequencyX + xOffset, z * frequencyX + zOffset) * amplitudeX;
            amplitudeX *= persistance;
            frequencyX *= 2;
        }
        return total;
    }


    void updateMesh()
    {
        //updates mesh height -> makes the waves
        xOffset += 10;
        zOffset += 10;

        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i].y = calculatePerlinOstaves((int)vertices[i].x, (int)vertices[i].z, 1, 1f);
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

    }
}
