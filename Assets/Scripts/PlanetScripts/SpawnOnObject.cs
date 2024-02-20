using UnityEngine;

[System.Serializable]
public class TreePrefabDensity
{
    public GameObject treePrefab;
    [Range(0f, 1f)]
    public float densityPercentage;
}

public class SpawnOnObject : MonoBehaviour
{
    public Mesh mesh;
    public int numberOfTreesToSpawn = 10; // Adjust this value to control the number of trees to spawn
    public TreePrefabDensity[] treePrefabs; // Array of tree prefabs with associated density percentages
    public float minTreeSize = 0.5f;
    public float maxTreeSize = 2f;

    void Start()
    {
        SpawnTrees();
    }

    void SpawnTrees()
    {
        for (int i = 0; i < numberOfTreesToSpawn; i++)
        {
            // Get a random point on the surface of each face of the mesh
            Vector3 spawnPosition = RandomPointOnMeshFace(mesh);

            // Get the rotation to orient the tree away from the center of the sphere
            Quaternion spawnRotation = Quaternion.FromToRotation(Vector3.up, spawnPosition.normalized);

            // Spawn a random tree at the generated position with the calculated rotation
            SpawnRandomTree(spawnPosition, spawnRotation);
        }
    }

    Vector3 RandomPointOnMeshFace(Mesh mesh)
    {
        // Randomly select a triangle (face) from the mesh
        int randomTriangleIndex = Random.Range(0, mesh.triangles.Length / 3); // Each triangle has 3 indices

        // Get the vertices of the selected triangle
        int index0 = mesh.triangles[randomTriangleIndex * 3];
        int index1 = mesh.triangles[randomTriangleIndex * 3 + 1];
        int index2 = mesh.triangles[randomTriangleIndex * 3 + 2];
        Vector3 vertex0 = mesh.vertices[index0];
        Vector3 vertex1 = mesh.vertices[index1];
        Vector3 vertex2 = mesh.vertices[index2];

        // Calculate random barycentric coordinates within the triangle
        float r1 = Random.Range(0f, 1f);
        float r2 = Random.Range(0f, 1f);

        // Ensure that the random point is inside the triangle
        if (r1 + r2 >= 1f)
        {
            r1 = 1f - r1;
            r2 = 1f - r2;
        }

        // Calculate the point on the triangle using barycentric coordinates
        Vector3 spawnPosition = vertex0 + r1 * (vertex1 - vertex0) + r2 * (vertex2 - vertex0);

        // Transform the point from local to world space
        Vector3 spawnPositionWorld = transform.TransformPoint(spawnPosition);

        return spawnPositionWorld;
    }

    void SpawnRandomTree(Vector3 position, Quaternion rotation)
    {
        // Randomly select a tree prefab based on its density percentage
        GameObject selectedPrefab = SelectRandomTreePrefab();

        // Generate random scale factors for the tree within the specified range
        float scaleFactor = Random.Range(minTreeSize, maxTreeSize);

        // Instantiate the randomly chosen tree prefab at the specified position with the calculated rotation
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
}
