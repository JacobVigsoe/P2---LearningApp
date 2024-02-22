using UnityEngine;


public class ProceduralPlanet : MonoBehaviour
{
    public float orbitSpeed = 1f;

    public int subdivisions = 20;
    public float radius = 5f;
    public Material blueMaterial;
    public Material greenMaterial;
    public float threshold = 0.5f; // Threshold for mesh visibility
    public TreePrefabDensity[] treePrefabs; // Array of tree prefabs with associated density percentages
    public float minTreeSize = 0.5f;
    public float maxTreeSize = 2f;

    public float noiseScale = 2f; // Adjust this value to control the size of deleted mesh chunks

    void Start()
    {
        CreateSphere();
    }

    void Update()
    {
        // Orbit around a central point (you may need to adjust the orbit axis and center point)
        transform.RotateAround(Vector3.zero, Vector3.up, orbitSpeed * Time.deltaTime);
    }
    void CreateSphere()
    {
        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();

        Mesh mesh = new Mesh();
        meshFilter.mesh = mesh;

        Vector3[] vertices = new Vector3[(subdivisions + 1) * (subdivisions + 1)];
        Color[] colors = new Color[vertices.Length];

        int i = 0;
        for (int y = 0; y <= subdivisions; y++)
        {
            for (int x = 0; x <= subdivisions; x++)
            {
                float xPos = (float)x / subdivisions;
                float yPos = (float)y / subdivisions;
                float xAngle = xPos * Mathf.PI * 2f;
                float yAngle = yPos * Mathf.PI;

                float xSin = Mathf.Sin(xAngle);
                float ySin = Mathf.Sin(yAngle);
                float xCos = Mathf.Cos(xAngle);
                float yCos = Mathf.Cos(yAngle);

                float xValue = xCos * ySin;
                float yValue = yCos;
                float zValue = xSin * ySin;

                vertices[i] = new Vector3(xValue, yValue, zValue) * radius;
                colors[i] = (y % 2 == 0) ? Color.blue : Color.green;
                i++;
            }
        }

        mesh.vertices = vertices;

        System.Random random = new System.Random(); // Create a random number generator
        int[] triangles = new int[subdivisions * subdivisions * 6];
        int vert = 0;
        int tris = 0;

        for (int y = 0; y < subdivisions; y++)
        {
            for (int x = 0; x < subdivisions; x++)
            {
                float noiseValue = Mathf.PerlinNoise((float)x / subdivisions * noiseScale, (float)y / subdivisions * noiseScale); // Sample Perlin noise
                if (noiseValue > threshold || random.NextDouble() > 1) // Determine whether to include the triangle based on Perlin noise and a random chance
                {
                    // Reverse winding order to make triangles face outward
                    triangles[tris + 0] = vert + 0;
                    triangles[tris + 1] = vert + 1;
                    triangles[tris + 2] = vert + subdivisions + 1;
                    triangles[tris + 3] = vert + 1;
                    triangles[tris + 4] = vert + subdivisions + 2;
                    triangles[tris + 5] = vert + subdivisions + 1;

                    tris += 6;

                    // Spawn tree objects based on density
                    if (Random.value < GetTotalDensity())
                    {
                        SpawnRandomTree(vertices[vert]);
                    }
                }

                vert++;
            }
            vert++;
        }

        // Resize the triangles array to fit the actual number of triangles created
        int[] resizedTriangles = new int[tris];
        for (int t = 0; t < tris; t++)
        {
            resizedTriangles[t] = triangles[t];
        }

        mesh.triangles = resizedTriangles;
        mesh.colors = colors;

        mesh.RecalculateNormals();

        meshRenderer.materials = new Material[] { blueMaterial, greenMaterial };
    }

    void SpawnRandomTree(Vector3 position)
    {
        // Calculate the rotation to point towards the center of the sphere
        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, position.normalized);

        // Randomly select a tree prefab based on its density percentage
        GameObject selectedPrefab = SelectRandomTreePrefab();

        // Generate random scale factors for the tree within the specified range
        float scaleFactor = Random.Range(minTreeSize, maxTreeSize);

        // Instantiate the randomly chosen tree prefab with the calculated rotation
        GameObject treeInstance = Instantiate(selectedPrefab, position, rotation);

        // Apply the random scale factors to the tree instance
        treeInstance.transform.localScale *= scaleFactor;
    }

    GameObject SelectRandomTreePrefab()
    {
        float randomValue = Random.value;
        float cumulativeDensity = 0f;

        foreach (TreePrefabDensity treePrefab in treePrefabs)
        {
            cumulativeDensity += treePrefab.densityPercentage;
            if (randomValue <= cumulativeDensity)
            {
                return treePrefab.treePrefab;
            }
        }

        // Fallback: Return the last tree prefab if selection fails
        return treePrefabs[treePrefabs.Length - 1].treePrefab;
    }

    float GetTotalDensity()
    {
        float totalDensity = 0f;
        foreach (TreePrefabDensity treePrefab in treePrefabs)
        {
            totalDensity += treePrefab.densityPercentage;
        }
        return totalDensity;
    }
}
