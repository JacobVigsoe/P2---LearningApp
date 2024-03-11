using UnityEngine;
using UnityEngine.UI;

public class ColorPicker : MonoBehaviour
{
    public GameObject colorPickerPanel; // Reference to the color picker panel
    public Button colorPickerButton; // Reference to the color picker button
    public Button[] colorButtons; // Array of colored buttons in the color picker panel
    public Color selectedColor; // Selected color
    public Color defaultColor = Color.white; // Default color if none is selected
    void Start()
    {
        selectedColor = defaultColor;
        // Hide the color picker panel initially
        colorPickerPanel.SetActive(false);

        // Add a click listener to the color picker button
        colorPickerButton.onClick.AddListener(ShowColorPicker);

        // Add click listeners to the colored buttons
        foreach (Button button in colorButtons)
        {
            button.onClick.AddListener(() => SelectColor(button.image.color));
        }
    }

    public void resetColorPalette()
    {
        selectedColor = defaultColor;
    }
    void ShowColorPicker()
    {
        // Show the color picker panel
        colorPickerPanel.SetActive(true);
    }

    void SelectColor(Color color)
    {
        // Set the selected color
        selectedColor = color;

        // Hide the color picker panel
        colorPickerPanel.SetActive(false);
    }

    // Getter method to retrieve the selected color
    public Color SelectedColor
    {
        get { return selectedColor; }
    }
}
