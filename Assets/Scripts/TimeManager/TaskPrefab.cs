using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using UnityEngine.Events;

public class TaskPrefab : MonoBehaviour
{
    public TMP_Text taskNameText;
    private TMP_Text remainingTimeText; // Reference to the text object to display remaining time
    public Button deleteButton; // Reference to the delete button
    public Slider timerSlider; // Reference to the slider component
    private TimeManager timeManager;

    // Remaining time in seconds
    private float remainingTimeSeconds;
    //Animations
    public RectTransform targetRectTransform;
    public Vector3 TargetScale;
    public Vector2 OffsetRight;
    public float AnimSpeed;
    public string TaskName { get; private set; }
    public delegate void OnTaskDelete(string taskName);
    public static event OnTaskDelete TaskDeleteEvent; // Event for deleting task

    private void Awake()
    {
        // Find the remaining time text object by tag
        remainingTimeText = GameObject.FindGameObjectWithTag("RemainingTimeText").GetComponent <TMP_Text>();
    }
    public void Initialize(Task task, TimeManager manager)
    {
        TaskName = task.name;
        taskNameText.text = task.name;
        timeManager = manager;

        // Set the remaining time
        remainingTimeSeconds = task.remainingTimeSeconds;

        // Update the remaining time text
        UpdateRemainingTimeText();
    }

    private void Start()
    {
        targetRectTransform.DOScale(TargetScale, AnimSpeed);
    }

    public void SetTaskInfo(string taskName)
    {
        taskNameText.text = taskName;
    }

    // Method to update the remaining time text
    private void UpdateRemainingTimeText()
    {
        if (remainingTimeText != null)
        {
            remainingTimeText.text = FormatTime(remainingTimeSeconds);
        }
    }

    // Method to format remaining time as HH:MM:SS
    private string FormatTime(float seconds)
    {
        int hours = Mathf.FloorToInt(seconds / 3600);
        int minutes = Mathf.FloorToInt((seconds % 3600) / 60);
        int secondsLeft = Mathf.FloorToInt(seconds % 60);
        return string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, secondsLeft);
    }

    // Method to handle the button click for updating remaining time
    public void UpdateRemainingTimeOnClick()
    {
        // Update the remaining time from the timer slider
        remainingTimeSeconds = timerSlider.value * 3600; // Convert slider value to seconds

        // Update the remaining time text
        UpdateRemainingTimeText();
    }

    private void OnDeleteButtonClick()
    {
        StartCoroutine(WaitForAnimation());
        targetRectTransform.DOAnchorPos(OffsetRight, AnimSpeed);
      
    }
    private IEnumerator WaitForAnimation()
    {
        yield return new WaitForSeconds(AnimSpeed);
        TaskDeleteEvent(TaskName); // Trigger the event passing the task name 
       
    }
}
