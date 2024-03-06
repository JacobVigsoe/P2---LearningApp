using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class TreePrefabDensity
{
    public GameObject treePrefab;
    [Range(0f, 1f)]
    public float densityPercentage;
}

public class SpawnOnObject : MonoBehaviour
{
    public GameObject objectToSpawnUnder; // Reference to the object to spawn trees under
    public int numberOfTreesToSpawn = 10; // Adjust this value to control the number of trees to spawn
    public TreePrefabDensity[] treePrefabs; // Array of tree prefabs with associated density percentages
    public float minTreeSize = 0.5f;
    public float maxTreeSize = 2f;

    public GameObject alignmentReferenceObject; // Reference object for alignment

    public GameObject HousePrefab;

    public XPStats xpstats;

    void Start()
    {
        SpawnTrees();
        
    }

    public void SpawnTrees()
    {
        if (objectToSpawnUnder == null)
        {
            Debug.LogError("Please assign the object to spawn trees under in the Inspector.");
            return;
        }

        Mesh mesh = objectToSpawnUnder.GetComponent<MeshFilter>().sharedMesh;
        if (mesh == null)
        {
            Debug.LogError("The selected object does not have a MeshFilter component.");
            return;
        }

        for (int i = 0; i < numberOfTreesToSpawn; i++)
        {
            // Get a random point within a triangle on the surface of the mesh
            Vector3 spawnPosition = RandomPointInTriangle(mesh);

            // Calculate the rotation to orient the tree relative to the alignment object
            Quaternion spawnRotation;
            if (alignmentReferenceObject != null)
            {
                Vector3 direction = spawnPosition - alignmentReferenceObject.transform.position;
                spawnRotation = Quaternion.FromToRotation(Vector3.up, direction.normalized);
            }
            else
            {
                // If no alignment object is specified, orient the tree away from the center of the sphere
                spawnRotation = Quaternion.FromToRotation(Vector3.up, spawnPosition.normalized);
            }

            // Spawn a random tree at the generated position with the calculated rotation
            SpawnRandomTree(spawnPosition, spawnRotation);
        }
    }


    Vector3 RandomPointInTriangle(Mesh mesh)
    {
        // Randomly select a triangle index
        int randomTriangleIndex = Random.Range(0, mesh.triangles.Length / 3); // Mesh.triangles contains vertex indices of triangles

        // Get the vertex indices of the selected triangle
        int vertexIndex1 = mesh.triangles[randomTriangleIndex * 3];
        int vertexIndex2 = mesh.triangles[randomTriangleIndex * 3 + 1];
        int vertexIndex3 = mesh.triangles[randomTriangleIndex * 3 + 2];

        // Get the positions of the vertices of the selected triangle
        Vector3 vertex1 = mesh.vertices[vertexIndex1];
        Vector3 vertex2 = mesh.vertices[vertexIndex2];
        Vector3 vertex3 = mesh.vertices[vertexIndex3];

        // Calculate the center of the triangle
        Vector3 triangleCenter = (vertex1 + vertex2 + vertex3) / 3f;

        // Generate random barycentric coordinates (u, v, w) within the triangle
        float u = Random.Range(0f, 1f);
        float v = Random.Range(0f, 1f - u);
        float w = 1 - u - v;

        // Calculate the point within the triangle using barycentric coordinates
        Vector3 spawnPosition = u * vertex1 + v * vertex2 + w * vertex3;

        // Move the spawn position towards the center of the triangle
        float displacementFactor = 1f; // Adjust this value to control the displacement amount
        spawnPosition = Vector3.Lerp(spawnPosition, triangleCenter, displacementFactor);

        // Transform the spawn position relative to the reference object's zero position
        if (alignmentReferenceObject != null)
        {
            spawnPosition = alignmentReferenceObject.transform.TransformPoint(spawnPosition);
        }

        return spawnPosition;
    }





    void SpawnRandomTree(Vector3 position, Quaternion rotation)
    {
        if (xpstats != null && xpstats.CurrentLevel == 5)
        {
            Debug.Log("Level 5 reached");
            GameObject housePrefab = HousePrefab;

            // Generate random scale factors for the tree within the specified range
            float scaleFactor = Random.Range(minTreeSize, maxTreeSize);

            // Instantiate the randomly chosen tree prefab at the specified position with the calculated rotation
            GameObject treeInstance = Instantiate(housePrefab, position, rotation);

            // Apply the random scale factors to the tree instance
            treeInstance.transform.localScale *= scaleFactor;

            // Set the parent of the spawned tree to the objectToSpawnUnder object
            treeInstance.transform.parent = objectToSpawnUnder.transform;
        }
        else
        {
            // Randomly select a tree prefab based on its density percentage
            GameObject selectedPrefab = SelectRandomTreePrefab();

            // Generate random scale factors for the tree within the specified range
            float scaleFactor = Random.Range(minTreeSize, maxTreeSize);

            // Instantiate the randomly chosen tree prefab at the specified position with the calculated rotation
            GameObject treeInstance = Instantiate(selectedPrefab, position, rotation);

            // Apply the random scale factors to the tree instance
            treeInstance.transform.localScale *= scaleFactor;

            // Set the parent of the spawned tree to the objectToSpawnUnder object
            treeInstance.transform.parent = objectToSpawnUnder.transform;
        }
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
