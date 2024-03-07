using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using UnityEngine.Events;

public class TaskPrefab : MonoBehaviour
{
    public TMP_Text taskNameText;
    public Button deleteButton; // Reference to the delete button
    public Slider timerSlider; // Reference to the slider component
    private float tempSliderValue; // Variable to temporarily store the slider value

    //Animations
    public RectTransform targetRectTransform;
    public Vector3 TargetScale;
    public Vector2 OffsetRight;
    public float AnimSpeed;
    public string TaskName { get; private set; }
    public delegate void OnTaskDelete(string taskName);
    public static event OnTaskDelete TaskDeleteEvent; // Event for deleting task

    private TMP_Text remainingTimeText; // Reference to TMP text for remaining time
    private static TaskPrefab lastClickedTaskPrefab; // Reference to the last clicked TaskPrefab

    private void Start()
    {
        targetRectTransform.DOScale(TargetScale, AnimSpeed);
        remainingTimeText = GameObject.FindWithTag("RemainingTimeText").GetComponent<TMP_Text>();

        // Subscribe to the slider's OnValueChanged event to update the text in real-time
        timerSlider.onValueChanged.AddListener(UpdateStoredTimerText);
    }

    public void Initialize(Task task, TimeManager manager)
    {
        TaskName = task.name;
        taskNameText.text = task.name;
        tempSliderValue = timerSlider.value; // Initialize tempSliderValue with current slider value
    }

    public void SetTaskInfo(string taskName)
    {
        TaskName = taskName;
        taskNameText.text = taskName;
        deleteButton.onClick.AddListener(OnDeleteButtonClick); // Subscribe to the button click event
        deleteButton.onClick.AddListener(OnUpdateTimerButtonClick); // Subscribe to the button click event to update timer value
    }

    public void OnDeleteButtonClick()
    {
        StartCoroutine(WaitForAnimation());
        targetRectTransform.DOAnchorPos(OffsetRight, AnimSpeed);
    }

    private IEnumerator WaitForAnimation()
    {
        yield return new WaitForSeconds(AnimSpeed);
        TaskDeleteEvent?.Invoke(TaskName); // Trigger the event passing the task name 
    }

    // Method to update the stored timer text
    private void UpdateStoredTimerText(float value)
    {
        if (remainingTimeText != null && this == lastClickedTaskPrefab)
        {
            float seconds = value * 3600f; // Convert slider value to seconds
            remainingTimeText.text = "Stored Timer Value: " + FormatTime(seconds);
        }
        else
        {
            Debug.LogError("RemainingTimeText not found or TaskPrefab not last clicked!");
        }
    }

    // New method to handle the click event for updating the stored timer text
    public void OnUpdateTimerButtonClick()
    {
        tempSliderValue = timerSlider.value;
        lastClickedTaskPrefab = this;
        UpdateStoredTimerText(tempSliderValue);
    }

    // Helper method to format time as HH:MM:SS
    private string FormatTime(float seconds)
    {
        int hours = Mathf.FloorToInt(seconds / 3600);
        int minutes = Mathf.FloorToInt((seconds % 3600) / 60);
        int secs = Mathf.FloorToInt(seconds % 60);
        return string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, secs);
    }
}
