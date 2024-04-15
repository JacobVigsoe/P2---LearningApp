using TMPro;
using UnityEngine;

public class ScrollSnap : MonoBehaviour
{
    public RectTransform panel; // To hold the scroll panel
    public RectTransform center; // To hold the center point for comparison
    public TMP_Text[] texts; // Array of TextMeshPro text elements within the scroll view
    public float[] distance; // Distance of each text element from the center vertically
    public bool dragging = false; // Flag to check if the user is dragging
    public float padding = 80f; // Padding applied to the viewport's Vertical Layout Group
    public float snapSpeed = 10f; // Snapping speed

    [SerializeField] private int interval = 1; // Interval between values

    void Start()
    {
        int textLength = texts.Length;
        distance = new float[textLength];

        // Assign values to the text elements based on the interval
        for (int i = 0; i < textLength; i++)
        {
            int value = i * interval; // Starting from 0 with the specified interval
            texts[i].text = value.ToString();
        }
    }

    void Update()
    {
        if (!dragging)
        {
            // Calculate distances of each text element from the center vertically
            for (int i = 0; i < texts.Length; i++)
            {
                distance[i] = Mathf.Abs(center.position.y - texts[i].transform.position.y);
            }

            // Find the closest text element
            float minDistance = Mathf.Min(distance);
            int minTextIndex = 0;
            for (int i = 0; i < distance.Length; i++)
            {
                if (distance[i] == minDistance)
                {
                    minTextIndex = i;
                    break;
                }
            }

            // Snap to the closest text element with padding adjustment and snapping speed
            LerpToText(minTextIndex, padding, snapSpeed);
        }
    }

    void LerpToText(int textIndex, float padding, float speed)
    {
        // Calculate the target position including padding adjustment
        float targetY = -texts[textIndex].rectTransform.anchoredPosition.y - padding;

        // Lerp towards the target position with specified speed
        float newY = Mathf.Lerp(panel.anchoredPosition.y, targetY, speed * Time.deltaTime);
        Vector2 newPosition = new Vector2(panel.anchoredPosition.x, newY);
        panel.anchoredPosition = newPosition;
    }

    public void StartDrag()
    {
        dragging = true;
    }

    public void EndDrag()
    {
        dragging = false;
    }

    // Method to get the value of the centered text element
    public int GetCenteredValue()
    {
        int minTextIndex = 0;
        float minDistance = Mathf.Min(distance);
        for (int i = 0; i < distance.Length; i++)
        {
            if (distance[i] == minDistance)
            {
                minTextIndex = i;
                break;
            }
        }
        return minTextIndex * interval;
    }
}
