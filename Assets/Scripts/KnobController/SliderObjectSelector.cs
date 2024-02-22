using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ObjectThreshold
{
    public GameObject objectToShow;
    public int threshold;
}

public class SliderObjectSelector : MonoBehaviour
{
    public Slider slider;
    public ObjectThreshold[] objectThresholds;

    private void Start()
    {
        // Hide all objects initially
        HideAllObjects();

        // Subscribe to the slider's OnValueChanged event
        slider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    private void OnSliderValueChanged(float value)
    {
        int sliderValue = Mathf.RoundToInt(value);

        // Hide all objects
        HideAllObjects();

        // Show the object corresponding to the current slider value
        foreach (ObjectThreshold threshold in objectThresholds)
        {
            if (sliderValue == threshold.threshold)
            {
                threshold.objectToShow.SetActive(true);
                return; // Exit the loop once the correct object is found and shown
            }
        }
    }

    private void HideAllObjects()
    {
        foreach (ObjectThreshold threshold in objectThresholds)
        {
            threshold.objectToShow.SetActive(false);
        }
    }
}
