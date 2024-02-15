using UnityEngine;

public class CubeSphereGenerator : MonoBehaviour
{
    public int numberOfCubes = 20; // Adjust the number of cubes as needed
    public float sphereRadius = 5f; // Adjust the radius of the sphere

    void Start()
    {
        GenerateCubes();
    }

    void GenerateCubes()
    {
        for (int i = 0; i < numberOfCubes; i++)
        {
            float angle = Random.Range(0f, Mathf.PI * 2f); // Random angle around the sphere
            float height = Random.Range(-1f, 1f); // Random height along the y-axis
            Vector3 spherePoint = new Vector3(Mathf.Cos(angle) * sphereRadius, height * sphereRadius, Mathf.Sin(angle) * sphereRadius); // Calculate position on sphere

            GameObject cube = new GameObject("Cube " + i); // Create a new GameObject for the cube
            cube.transform.position = spherePoint; // Set cube's position to the calculated point on the sphere
            cube.AddComponent<MeshFilter>(); // Add MeshFilter component
            cube.AddComponent<MeshRenderer>(); // Add MeshRenderer component

            Mesh mesh = new Mesh();
            mesh.vertices = GenerateCubeVertices(); // Generate cube vertices
            mesh.triangles = GenerateCubeTriangles(); // Generate cube triangles
            mesh.RecalculateNormals(); // Recalculate normals for lighting

            cube.GetComponent<MeshFilter>().mesh = mesh; // Assign mesh to MeshFilter component
        }
    }

    Vector3[] GenerateCubeVertices()
    {
        Vector3[] vertices = new Vector3[8];
        // Define vertices of a cube
        vertices[0] = new Vector3(-0.5f, -0.5f, -0.5f);
        vertices[1] = new Vector3(0.5f, -0.5f, -0.5f);
        vertices[2] = new Vector3(0.5f, -0.5f, 0.5f);
        vertices[3] = new Vector3(-0.5f, -0.5f, 0.5f);
        vertices[4] = new Vector3(-0.5f, 0.5f, -0.5f);
        vertices[5] = new Vector3(0.5f, 0.5f, -0.5f);
        vertices[6] = new Vector3(0.5f, 0.5f, 0.5f);
        vertices[7] = new Vector3(-0.5f, 0.5f, 0.5f);
        return vertices;
    }

    int[] GenerateCubeTriangles()
    {
        int[] triangles = new int[36];
        // Define triangles of a cube using vertex indices
        triangles[0] = 0; triangles[1] = 1; triangles[2] = 4;
        triangles[3] = 1; triangles[4] = 5; triangles[5] = 4;
        triangles[6] = 1; triangles[7] = 2; triangles[8] = 5;
        triangles[9] = 2; triangles[10] = 6; triangles[11] = 5;
        triangles[12] = 2; triangles[13] = 3; triangles[14] = 6;
        triangles[15] = 3; triangles[16] = 7; triangles[17] = 6;
        triangles[18] = 3; triangles[19] = 0; triangles[20] = 7;
        triangles[21] = 0; triangles[22] = 4; triangles[23] = 7;
        triangles[24] = 4; triangles[25] = 5; triangles[26] = 6;
        triangles[27] = 4; triangles[28] = 6; triangles[29] = 7;
        triangles[30] = 0; triangles[31] = 1; triangles[32] = 2;
        triangles[33] = 0; triangles[34] = 2; triangles[35] = 3;
        return triangles;
    }
}
