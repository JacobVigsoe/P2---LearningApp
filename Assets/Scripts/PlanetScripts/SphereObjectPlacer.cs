using UnityEngine;

public class SphereObjectPlacer : MonoBehaviour
{
    public GameObject objectPrefab; // Prefab of the object to be placed
    public int numberOfObjects = 50; // Number of objects to be placed
    public float sphereRadius = 5f; // Radius of the sphere where objects will be placed

    void Start()
    {
        PlaceObjectsOnSphere();
    }

    void PlaceObjectsOnSphere()
    {
        for (int i = 0; i < numberOfObjects; i++)
        {
            // Calculate random point on the surface of the sphere using spherical coordinates
            float u = Random.Range(0f, 1f);
            float v = Random.Range(0f, 1f);
            float theta = 2 * Mathf.PI * u;
            float phi = Mathf.Acos(2 * v - 1);

            // Convert spherical coordinates to Cartesian coordinates
            float x = sphereRadius * Mathf.Sin(phi) * Mathf.Cos(theta);
            float y = sphereRadius * Mathf.Sin(phi) * Mathf.Sin(theta);
            float z = sphereRadius * Mathf.Cos(phi);

            // Normalize the position vector to ensure it's exactly on the surface of the sphere
            Vector3 position = new Vector3(x, y, z).normalized * sphereRadius;

            // Calculate rotation to orient the bottom of the prefab towards the center of the sphere
            Quaternion rotation = Quaternion.LookRotation(transform.position - position, Vector3.up);

            // Instantiate object at the calculated position with correct rotation
            GameObject obj = Instantiate(objectPrefab, position, rotation);
            obj.transform.SetParent(transform); // Set the parent of the object to this GameObject
        }
    }
}
