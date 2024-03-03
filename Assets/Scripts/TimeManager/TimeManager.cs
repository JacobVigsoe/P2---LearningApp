using UnityEngine;
using TMPro;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;

[System.Serializable]
public class Task
{
    public string name;
    public int hoursToComplete;
    public DateTime dueDate; // Add a field for the due date

    public Task(string _name, int _hoursToComplete, DateTime _dueDate)
    {
        name = _name;
        hoursToComplete = _hoursToComplete;
        dueDate = _dueDate;
    }
}

public class TimeManager : MonoBehaviour
{
    private DateTime currentTime;
    private DateTime startOfWeek;
    private TimeSpan weekDuration;
    private TimeSpan elapsedWeekTime;
    private bool isRunning = false;

    public TMP_Text timeText;
    public TMP_Text dateText;
    public Slider weekSlider;

    public List<Task> tasks = new List<Task>();

    public TMP_InputField taskNameInput;
    public TMP_InputField hoursToCompleteInput;

    public GameObject taskPrefab; // Reference to the task prefab
    public Transform tasksParent; // Parent transform to instantiate tasks under
    public Vector2 gridCellSize = new Vector2(200, 100); // Size of each grid cell
    public Vector2 gridSpacing = new Vector2(20, 20); // Spacing between grid cells
    public Vector3 spawnOffset = new Vector3(0, 0, 0); // Offset for spawning position relative to tasksParent

    public Color gizmoColor = Color.blue; // Color of the grid cell gizmos

    public int maxColumns = 3; // Maximum number of columns in the grid

    public Scrollbar hoursToCompleteScrollbar; // Reference to the scrollbar for hours to complete

    void Start()
    {
        Initialize();
        UpdateDateTime();
        StartTimer();
    }

    void Update()
    {
        if (isRunning)
        {
            UpdateTimer();
        }
    }

    public void StartTimer()
    {
        isRunning = true;
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public void ResetTimer()
    {
        Initialize();
        UpdateDateTime();
    }

    public void AddTask()
    {
        string taskName = taskNameInput.text;
        int hoursToComplete = Mathf.RoundToInt(hoursToCompleteScrollbar.value * 24); // Convert scroll value to hours
        // Calculate due date based on current time and hours to complete
        DateTime dueDate = DateTime.Now.AddHours(hoursToComplete);

        tasks.Add(new Task(taskName, hoursToComplete, dueDate));

        // Calculate grid position for the new task
        int rowIndex = (tasks.Count - 1) / maxColumns;
        int columnIndex = (tasks.Count - 1) % maxColumns;

        // Calculate the position for the new task
        Vector3 taskPosition = tasksParent.position + spawnOffset + new Vector3(columnIndex * (gridCellSize.x + gridSpacing.x), -rowIndex * (gridCellSize.y + gridSpacing.y), 0);

        // Create and instantiate a new task prefab
        GameObject newTaskObject = Instantiate(taskPrefab, taskPosition, Quaternion.identity, tasksParent);
        TaskPrefab newTaskPrefab = newTaskObject.GetComponent<TaskPrefab>();
        newTaskPrefab.SetTaskInfo(taskName);

        // Get the slider component from the task prefab

        Slider timerSlider = newTaskObject.GetComponentInChildren<Slider>();
        timerSlider.maxValue = hoursToComplete;

        // Start countdown coroutine for the newly added task
        StartCoroutine(CountdownTask(timerSlider, dueDate));

        // Optionally, you can save the tasks to PlayerPrefs, a file, or a database here
        taskNameInput.text = ""; // Clear input fields after adding task
        hoursToCompleteInput.text = "";
    }

    private void UpdateTimer()
    {
        UpdateDateTime();
        UpdateDisplay();
    }

    private void Initialize()
    {
        startOfWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
        weekDuration = TimeSpan.FromDays(7);
        elapsedWeekTime = TimeSpan.Zero;
    }

    private void UpdateDateTime()
    {
        currentTime = DateTime.Now;
        elapsedWeekTime = currentTime - startOfWeek;
    }

    private void UpdateDisplay()
    {
        if (timeText != null)
        {
            timeText.text = currentTime.ToString("HH:mm:ss");
        }

        if (dateText != null)
        {
            dateText.text = currentTime.ToString("dddd, MMMM dd, yyyy");
        }

        if (weekSlider != null)
        {
            float fillAmount = (float)(elapsedWeekTime.TotalSeconds / weekDuration.TotalSeconds);
            weekSlider.value = fillAmount;
        }
    }
    IEnumerator CountdownTask(Slider timerSlider, DateTime dueDate)
    {
        // Calculate the total time allowed for the task in seconds
        float totalTimeSeconds = (float)(dueDate - DateTime.Now).TotalHours * 3600f;

        while (DateTime.Now < dueDate)
        {
            // Calculate remaining time until due date
            TimeSpan remainingTime = dueDate - DateTime.Now;

            // Calculate the elapsed time since the task's due date in seconds
            float elapsedTimeSeconds = (float)(totalTimeSeconds - remainingTime.TotalHours * 3600f);

            // Calculate the slider value based on the elapsed time
            float sliderValue = Mathf.Lerp(timerSlider.maxValue, 0f, elapsedTimeSeconds / totalTimeSeconds);
            timerSlider.value = sliderValue;

            yield return null; // Wait for the next frame
        }

        // Optionally, you can handle task completion here
    }







    private void OnDrawGizmosSelected()
    {
        Gizmos.color = gizmoColor;
        Vector3 parentPosition = tasksParent.position + spawnOffset;
        for (int row = 0; row < tasks.Count / maxColumns + 1; row++)
        {
            for (int col = 0; col < maxColumns; col++)
            {
                Vector3 cellCenter = parentPosition + new Vector3(col * (gridCellSize.x + gridSpacing.x) + gridCellSize.x / 2, -row * (gridCellSize.y + gridSpacing.y) - gridCellSize.y / 2, 0);
                Gizmos.DrawWireCube(cellCenter, new Vector3(gridCellSize.x, gridCellSize.y, 0));
            }
        }
    }
}
