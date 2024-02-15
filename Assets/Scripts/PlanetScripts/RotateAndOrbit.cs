using UnityEngine;

public class RotateAndOrbit : MonoBehaviour
{
    public float rotationSpeed = 10f;
    public float orbitSpeed = 1f;

    void Update()
    {
        // Rotate the planet
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

        // Orbit around a central point (you may need to adjust the orbit axis and center point)
        transform.RotateAround(Vector3.zero, Vector3.up, orbitSpeed * Time.deltaTime);
    }
}
