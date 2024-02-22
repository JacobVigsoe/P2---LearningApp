using UnityEngine;

public class KnobController : MonoBehaviour
{
    public GameObject knobPrefab;
    public Vector3 circleCenter;
    public float circleRadius;
    public int numberOfKnobs = 10; // Adjust as needed
    public float knobSize = 1f; // Default knob size
    public float zPosition = 0f; // Default Z position

    private bool isDragging = false;
    private Vector3 previousMousePosition;

    private void Start()
    {
        SpawnKnobs();
    }

    private void Update()
    {
        if (isDragging)
        {
            Vector3 currentMousePosition = Input.mousePosition;
            Vector3 mouseDelta = currentMousePosition - previousMousePosition;
            float angleDelta = mouseDelta.x * 0.1f; // Adjust sensitivity as needed

            transform.Rotate(Vector3.forward, angleDelta);

            previousMousePosition = currentMousePosition;
        }
    }

    private void SpawnKnobs()
    {
        float angleIncrement = 360f / numberOfKnobs;

        for (int i = 0; i < numberOfKnobs; i++)
        {
            float angle = i * angleIncrement * Mathf.Deg2Rad;
            Vector3 spawnPosition = circleCenter + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * circleRadius;
            spawnPosition.z = zPosition; // Set Z position
            GameObject newKnob = Instantiate(knobPrefab, spawnPosition, Quaternion.identity);
            newKnob.transform.localScale = new Vector3(knobSize, knobSize, 1f); // Set knob size
            newKnob.transform.parent = transform; // Set the knob's parent to keep the hierarchy clean
        }
    }

    private void OnMouseDown()
    {
        isDragging = true;
        previousMousePosition = Input.mousePosition;
    }

    private void OnMouseUp()
    {
        isDragging = false;
    }
}
