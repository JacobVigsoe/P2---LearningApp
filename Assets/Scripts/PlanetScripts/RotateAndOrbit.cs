using UnityEngine;

public class RotateAndOrbit : MonoBehaviour
{
    public float orbitSpeed = 1f;

    void Update()
    {
        // Orbit around a central point (you may need to adjust the orbit axis and center point)
        transform.RotateAround(Vector3.up, orbitSpeed * Time.deltaTime);
    }
}
