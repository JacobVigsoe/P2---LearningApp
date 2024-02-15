using UnityEngine;

public class ProceduralSphereColoring : MonoBehaviour
{
    public Material greenMaterial; // Material representing green color
    public Material blueMaterial;  // Material representing blue color

    void Start()
    {
        ColorSphere();
    }

    void ColorSphere()
    {
        Renderer renderer = GetComponent<Renderer>();

        // Get the mesh of the sphere
        Mesh mesh = GetComponent<MeshFilter>().mesh;

        // Get the vertices of the mesh
        Vector3[] vertices = mesh.vertices;

        // Get the number of vertices
        int numVertices = vertices.Length;

        // Create an array to store the colors of the vertices
        Color[] colors = new Color[numVertices];

        // Iterate through each vertex
        for (int i = 0; i < numVertices; i++)
        {
            // Calculate the y-coordinate of the current vertex
            float yPos = vertices[i].y;

            // Set the color of the current vertex based on its y-coordinate
            colors[i] = yPos > 0 ? greenMaterial.color : blueMaterial.color;
        }

        // Assign the colors to the mesh
        mesh.colors = colors;

        // Assign the materials to the renderer
        Material[] materials = new Material[2] { greenMaterial, blueMaterial };
        renderer.materials = materials;
    }
}
