using TMPro; // Importing the TextMeshPro namespace to use TMP_Text.
using UnityEngine; // Importing the UnityEngine namespace to use MonoBehaviour and other Unity features.

public class ScrollSnap : MonoBehaviour // Defining the ScrollSnap class, which inherits from MonoBehaviour.
{
    public RectTransform panel; // Public field to hold the reference to the scroll panel.
    public RectTransform center; // Public field to hold the reference to the center point for comparison.
    public TMP_Text[] texts; // Public array to hold the TextMeshPro text elements within the scroll view.
    public float[] distance; // Public array to store the distance of each text element from the center vertically.
    public bool dragging = false; // Public flag to check if the user is dragging the scroll view.
    public float padding = 80f; // Public field to set the padding applied to the viewport's Vertical Layout Group.
    public float snapSpeed = 10f; // Public field to set the snapping speed.

    [SerializeField] private int interval = 1; // Private field to set the interval between values, exposed in the inspector.

    void Start()
    {
        int textLength = texts.Length; // Get the length of the texts array.
        distance = new float[textLength]; // Initialize the distance array with the same length as the texts array.

        // Assign values to the text elements based on the interval.
        for (int i = 0; i < textLength; i++)
        {
            int value = i * interval; // Calculate the value for each text element based on its index and the interval.
            texts[i].text = value.ToString(); // Set the text of each TMP_Text element to the calculated value.
        }
    }

    void Update()
    {
        if (!dragging) // If the user is not dragging the scroll view.
        {
            // Calculate distances of each text element from the center vertically.
            for (int i = 0; i < texts.Length; i++)
            {
                distance[i] = Mathf.Abs(center.position.y - texts[i].transform.position.y); // Calculate the absolute vertical distance from the center.
            }

            // Find the closest text element.
            float minDistance = Mathf.Min(distance); // Find the minimum distance value.
            int minTextIndex = 0; // Initialize the index of the closest text element.
            for (int i = 0; i < distance.Length; i++)
            {
                if (distance[i] == minDistance) // If the distance of the current text element is equal to the minimum distance.
                {
                    minTextIndex = i; // Update the index of the closest text element.
                    break; // Exit the loop once the closest element is found.
                }
            }

            // Snap to the closest text element with padding adjustment and snapping speed.
            LerpToText(minTextIndex, padding, snapSpeed); // Call the LerpToText method to snap to the closest text element.
        }
    }

    void LerpToText(int textIndex, float padding, float speed)
    {
        // Calculate the target position including padding adjustment.
        float targetY = -texts[textIndex].rectTransform.anchoredPosition.y - padding; // Calculate the target Y position considering the padding.

        // Lerp towards the target position with specified speed.
        float newY = Mathf.Lerp(panel.anchoredPosition.y, targetY, speed * Time.deltaTime); // Linearly interpolate between the current and target Y positions.
        Vector2 newPosition = new Vector2(panel.anchoredPosition.x, newY); // Create a new Vector2 position with the interpolated Y value.
        panel.anchoredPosition = newPosition; // Update the panel's anchored position to the new position.
    }

    public void StartDrag()
    {
        dragging = true; // Set dragging to true when the user starts dragging.
    }

    public void EndDrag()
    {
        dragging = false; // Set dragging to false when the user stops dragging.
    }

    // Method to get the value of the centered text element.
    public int GetCenteredValue()
    {
        int minTextIndex = 0; // Initialize the index of the closest text element.
        float minDistance = Mathf.Min(distance); // Find the minimum distance value.
        for (int i = 0; i < distance.Length; i++)
        {
            if (distance[i] == minDistance) // If the distance of the current text element is equal to the minimum distance.
            {
                minTextIndex = i; // Update the index of the closest text element.
                break; // Exit the loop once the closest element is found.
            }
        }
        return minTextIndex * interval; // Return the value of the centered text element, calculated based on the interval.
    }
}
