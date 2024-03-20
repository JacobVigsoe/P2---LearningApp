using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class KnobController : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    private Vector2 lastMousePos;
    private Vector2 objectCenter;
    private float timer = 0f;
    [SerializeField] private float AddTimeAmount = 1f;
    [SerializeField] private float timerChangeRate = 0.5f; // Adjust this value to change the rate of timer change

    public TextMeshProUGUI timerText; // Reference to the TextMeshPro object

    public void OnPointerDown(PointerEventData eventData)
    {
        lastMousePos = eventData.position;
        objectCenter = RectTransformUtility.WorldToScreenPoint(eventData.pressEventCamera, transform.position);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 currentMousePos = eventData.position;

        // Calculate angles
        Vector2 lastDir = lastMousePos - objectCenter;
        Vector2 currentDir = currentMousePos - objectCenter;
        float angleChange = Vector2.SignedAngle(lastDir, currentDir);


        if (angleChange > 0 && timer > 0)
        {
            // Counter Clockwise rotation
            timer -= AddTimeAmount * timerChangeRate; //fjerner 1 fra timeren basered på hvordan man roterer objectet
        }


        if (angleChange < 0)
        {
            // Clockwise rotation
            timer += AddTimeAmount * timerChangeRate; // adder 1 til timeren basered på hvordan man roterer objectet
        }

        // Update TextMeshPro text
        if (timerText != null)
        {
            float hours = Mathf.Floor(timer / 3600);
            float remainingMinutes = Mathf.Floor((timer % 3600) / 60);
            timerText.text = string.Format("{0:00}:{1:00}", hours, remainingMinutes);
        }

        // Apply rotation
        transform.Rotate(Vector3.forward, angleChange);

        // Update last mouse position
        lastMousePos = currentMousePos;
    }
}
