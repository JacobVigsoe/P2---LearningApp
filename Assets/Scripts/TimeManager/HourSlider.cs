using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HourScrollBar : MonoBehaviour
{
    public Scrollbar hourScrollBar; // Reference to the UI Scrollbar for controlling the hours
    public TMP_Text hourText; // Reference to the TMP Text component to display the hour value

    void Start()
    {
        // Initialize the hour text with the initial scrollbar value
        UpdateHourText(hourScrollBar.value);
    }

    void Update()
    {
        // Update the hour text continuously based on the current scrollbar value
        UpdateHourText(hourScrollBar.value);
    }

    // Method to update the hour text
    void UpdateHourText(float value)
    {
        // Convert the scrollbar value to an integer representing hours
        int hours = Mathf.RoundToInt(value * 24); // Assuming the scrollbar value ranges from 0 to 1

        // Update the TMP Text component with the current hour value
        hourText.text = hours.ToString();

    }
}
