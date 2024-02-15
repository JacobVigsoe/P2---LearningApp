using UnityEngine;

public class PlanetGenerator : MonoBehaviour
{
    public int latitudeSegments = 24; // Number of segments along the latitude
    public int longitudeSegments = 48; // Number of segments along the longitude
    public float radius = 5f; // Radius of the planet
    public Material greenMaterial; // Material for the green sections
    public Material blueMaterial; // Material for the blue sections

    void Start()
    {
        GeneratePlanet();
    }

    void GeneratePlanet()
    {
        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();

        // Create a new mesh
        Mesh mesh = new Mesh();
        meshFilter.mesh = mesh;

        // Vertices array
        Vector3[] vertices = new Vector3[(latitudeSegments + 1) * (longitudeSegments + 1)];

        // UVs array
        Vector2[] uv = new Vector2[vertices.Length];

        // Normals array
        Vector3[] normals = new Vector3[vertices.Length];

        // Triangles array
        int[] triangles = new int[latitudeSegments * longitudeSegments * 6];

        // Generate vertices
        for (int lat = 0; lat <= latitudeSegments; lat++)
        {
            float theta = lat * Mathf.PI / latitudeSegments;
            float sinTheta = Mathf.Sin(theta);
            float cosTheta = Mathf.Cos(theta);

            for (int lon = 0; lon <= longitudeSegments; lon++)
            {
                float phi = lon * 2f * Mathf.PI / longitudeSegments;
                float sinPhi = Mathf.Sin(phi);
                float cosPhi = Mathf.Cos(phi);

                Vector3 vertex = new Vector3(cosPhi * sinTheta, cosTheta, sinPhi * sinTheta) * radius;
                vertices[lat * (longitudeSegments + 1) + lon] = vertex;
                normals[lat * (longitudeSegments + 1) + lon] = vertex.normalized;
                uv[lat * (longitudeSegments + 1) + lon] = new Vector2((float)lon / longitudeSegments, (float)lat / latitudeSegments);
            }
        }

        // Generate triangles
        int index = 0;
        for (int lat = 0; lat < latitudeSegments; lat++)
        {
            for (int lon = 0; lon < longitudeSegments; lon++)
            {
                int current = lat * (longitudeSegments + 1) + lon;
                int next = current + longitudeSegments + 1;

                triangles[index++] = current;
                triangles[index++] = next + 1;
                triangles[index++] = next;

                triangles[index++] = current;
                triangles[index++] = current + 1;
                triangles[index++] = next + 1;
            }
        }

        // Assign mesh data
        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.uv = uv;
        mesh.triangles = triangles;

        // Assign materials to the mesh renderer
        Material[] materials = new Material[2];
        materials[0] = greenMaterial;
        materials[1] = blueMaterial;
        meshRenderer.materials = materials;
    }
}
