using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class TaskPrefab : MonoBehaviour
{
    public TMP_Text taskNameText;
    private TMP_Text remainingTimeText;
    public Button deleteButton;
    public Slider timerSlider;
    private TimeManager timeManager;

    private float remainingTimeSeconds;
    private string currentTaskName; // Stores the name of the last clicked task prefab

    private float lastClickTime; // Stores the time of the last click
    private float elapsedTimeSinceClick; // Stores the elapsed time since the last click

    public RectTransform targetRectTransform;
    public Vector3 TargetScale;
    public Vector2 OffsetRight;
    public float AnimSpeed;
    public string TaskName { get; private set; }
    public delegate void OnTaskDelete(string taskName);
    public static event OnTaskDelete TaskDeleteEvent;

    private void Awake()
    {
        remainingTimeText = GameObject.FindGameObjectWithTag("RemainingTimeText").GetComponent<TMP_Text>();
    }

    public void Initialize(Task task, TimeManager manager)
    {
        TaskName = task.name;
        taskNameText.text = task.name;
        timeManager = manager;

        SetRemainingTime(task.remainingTimeSeconds); // Set the remaining time

        UpdateRemainingTimeText();
        StartCoroutine(UpdateRemainingTime());
    }

    private void Start()
    {
        targetRectTransform.DOScale(TargetScale, AnimSpeed);
        timerSlider.onValueChanged.AddListener(OnSliderValueChanged);
        deleteButton.onClick.AddListener(OnDeleteButtonClick);
    }

    public void UpdateRemainingTimeText()
    {
        if (remainingTimeText != null)
        {
            // Only update the remaining time text if the current task name matches the task name of this task prefab
            if (TaskName == currentTaskName)
            {
                remainingTimeText.text = FormatTime(remainingTimeSeconds);
            }
        }
    }

    private string FormatTime(float seconds)
    {
        int hours = Mathf.FloorToInt(seconds / 3600);
        int minutes = Mathf.FloorToInt((seconds % 3600) / 60);
        int secondsLeft = Mathf.FloorToInt(seconds % 60);
        return string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, secondsLeft);
    }

    private IEnumerator UpdateRemainingTime()
    {
        while (true)
        {
            // Update the remaining time continuously by subtracting the elapsed time since the last click
            if (currentTaskName != null)
            {
                elapsedTimeSinceClick = Time.time - lastClickTime;
                remainingTimeSeconds -= elapsedTimeSinceClick;
                lastClickTime = Time.time;

                // Ensure remaining time does not go below zero
                remainingTimeSeconds = Mathf.Max(0, remainingTimeSeconds);

                UpdateRemainingTimeText();
            }

            yield return null;
        }
    }

    private void OnSliderValueChanged(float value)
    {
        SetRemainingTime(value * 3600); // Convert slider value to seconds
    }

    private void SetRemainingTime(float seconds)
    {
        remainingTimeSeconds = seconds;
    }

    public void SetTaskInfo(string taskName)
    {
        TaskName = taskName;
        taskNameText.text = taskName;
        currentTaskName = taskName; // Update the current task name when setting task info
        lastClickTime = Time.time; // Update the last click time when setting task info
    }

    public void OnDeleteButtonClick()
    {
        StartCoroutine(WaitForAnimation());
        targetRectTransform.DOAnchorPos(OffsetRight, AnimSpeed);
    }

    private IEnumerator WaitForAnimation()
    {
        yield return new WaitForSeconds(AnimSpeed);
        TaskDeleteEvent?.Invoke(TaskName);
    }
}
