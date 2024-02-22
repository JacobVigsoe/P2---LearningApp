using UnityEngine;

public class DragRotate : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 startMousePosition;
    private Vector3 startRotation;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject == gameObject)
            {
                isDragging = true;
                startMousePosition = Input.mousePosition;
                startRotation = transform.eulerAngles;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        if (isDragging)
        {
            Vector3 currentMousePosition = Input.mousePosition;
            Vector3 mouseDelta = currentMousePosition - startMousePosition;

            float rotationSpeed = 0.5f; // Adjust as needed
            Vector3 newRotation = startRotation + new Vector3(-mouseDelta.y, mouseDelta.x, 0f) * rotationSpeed;

            transform.eulerAngles = newRotation;
        }
    }
}
