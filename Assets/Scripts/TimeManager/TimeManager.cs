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
    public DateTime dueDate;
    public float remainingTimeSeconds; // New property to store remaining time

    public Task(string _name, int _hoursToComplete, DateTime _dueDate)
    {
        name = _name;
        hoursToComplete = _hoursToComplete;
        dueDate = _dueDate;
        remainingTimeSeconds = CalculateRemainingTime(); // Initialize remaining time
    }

    // Method to calculate remaining time
    public float CalculateRemainingTime()
    {
        TimeSpan timeRemaining = dueDate - DateTime.Now;
        return Mathf.Max((float)timeRemaining.TotalSeconds, 0f);
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

    public GameObject taskPrefab;
    public Transform tasksParent;
    public Vector2 gridCellSize = new Vector2(200, 100);
    public Vector2 gridSpacing = new Vector2(20, 20);
    public Vector3 spawnOffset = new Vector3(0, 0, 0);
    public Color gizmoColor = Color.blue;
    public int maxColumns = 3;
    public Scrollbar hoursToCompleteScrollbar;

    private const string TaskInfosKey = "TaskInfos";

    private void Start()
    {
        LoadTasks();
        Initialize();
        
        UpdateDateTime();
        StartTimer();
    }

    private void Update()
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
        int hoursToComplete = Mathf.RoundToInt(hoursToCompleteScrollbar.value * 24);
        DateTime dueDate = DateTime.Now.AddHours(hoursToComplete);

        tasks.Add(new Task(taskName, hoursToComplete, dueDate));

        int rowIndex = (tasks.Count - 1) / maxColumns;
        int columnIndex = (tasks.Count - 1) % maxColumns;

        Vector3 taskPosition = tasksParent.position + spawnOffset + new Vector3(columnIndex * (gridCellSize.x + gridSpacing.x), -rowIndex * (gridCellSize.y + gridSpacing.y), 0);

        GameObject newTaskObject = Instantiate(taskPrefab, taskPosition, Quaternion.identity, tasksParent);
        TaskPrefab newTaskPrefab = newTaskObject.GetComponent<TaskPrefab>();
        newTaskPrefab.SetTaskInfo(taskName);

        Slider timerSlider = newTaskObject.GetComponentInChildren<Slider>();
        timerSlider.maxValue = hoursToComplete;

        float remainingTimeSeconds = (float)(dueDate - DateTime.Now).TotalSeconds;
        StartCoroutine(CountdownTask(timerSlider, dueDate, remainingTimeSeconds));

        taskNameInput.text = "";
        hoursToCompleteInput.text = "";

        SaveTasks();
    }

    public void RemoveTask(int index)
    {
        tasks.RemoveAt(index);
        SaveTasks();
    }

    public void SaveTasks()
    {
        // Create a list to store task information (name, hoursToComplete, remainingTime, sliderValue)
        List<string> taskInfos = new List<string>();

        // Iterate through each task and add its information to the list
        foreach (Task task in tasks)
        {
            // Calculate remaining time until due date
            float remainingTimeSeconds = Mathf.Max((float)(task.dueDate - DateTime.Now).TotalSeconds, 0f);

            // Get the slider value from the task's slider component
            Slider timerSlider = FindSliderForTask(task);
            float sliderValue = timerSlider != null ? timerSlider.value : 0f;

            // Concatenate the task information with remaining time and slider value using '|' as delimiter
            string taskInfo = $"{task.name}|{task.hoursToComplete}|{remainingTimeSeconds}|{sliderValue}";

            // Add the task information to the list
            taskInfos.Add(taskInfo);
        }

        // Convert the list of task information to a single string separated by comma
        string taskInfosString = string.Join(",", taskInfos);

        // Save the task information string to PlayerPrefs
        PlayerPrefs.SetString("TaskInfos", taskInfosString);
        PlayerPrefs.Save();

        Debug.Log("Task information saved: " + taskInfosString);
    }

    private void OnApplicationQuit()
    {
        SaveRemainingTime(); // Save remaining time for tasks
    }

    private void SaveRemainingTime()
    {
        foreach (Task task in tasks)
        {
            // Calculate the remaining time until the due date
            TimeSpan timeRemaining = task.dueDate - DateTime.Now;
            float remainingTimeSeconds = Mathf.Max((float)timeRemaining.TotalSeconds, 0f);

            // Find the slider associated with the task
            Slider timerSlider = FindSliderForTask(task);
            if (timerSlider != null)
            {
                // Save the remaining time in PlayerPrefs
                PlayerPrefs.SetFloat(task.name + "_RemainingTime", task.remainingTimeSeconds);
                Debug.Log($"Remaining time saved for task '{task.name}': {remainingTimeSeconds} seconds");
            }
        }
        PlayerPrefs.Save(); // Save PlayerPrefs
        Debug.Log("All remaining times saved.");
    }



    private Slider FindSliderForTask(Task task)
    {
        foreach (Transform child in tasksParent)
        {
            TaskPrefab taskPrefab = child.GetComponent<TaskPrefab>();
            if (taskPrefab != null && taskPrefab.TaskName == task.name)
            {
                Slider timerSlider = child.GetComponentInChildren<Slider>();
                return timerSlider;
            }
        }
        return null;
    }


    public void LoadTasks()
    {
        // Check if the TaskInfos key exists in PlayerPrefs
        if (PlayerPrefs.HasKey("TaskInfos"))
        {
            // Retrieve the task information string from PlayerPrefs
            string taskInfosString = PlayerPrefs.GetString("TaskInfos");

            // Split the task information string into an array using comma as delimiter
            string[] taskInfos = taskInfosString.Split(',');

            // Clear the existing tasks list
            tasks.Clear();

            // Create new Task objects with the loaded task information
            foreach (string taskInfo in taskInfos)
            {
                // Split the task information into name, hoursToComplete, remainingTime, and sliderValue using '|' as delimiter
                string[] info = taskInfo.Split('|');
                string taskName = info[0];
                int hoursToComplete = int.Parse(info[1]);
                float remainingTimeSeconds = float.Parse(info[2]);
                float sliderValue = float.Parse(info[3]);

                // Calculate due date based on remaining time
                DateTime dueDate = DateTime.Now.AddSeconds(remainingTimeSeconds);

                // Create a new Task object and add it to the tasks list
                Task task = new Task(taskName, hoursToComplete, dueDate);
                task.remainingTimeSeconds = remainingTimeSeconds; // Update remaining time
                tasks.Add(task);
            }

            // After loading tasks, recreate prefabs for each task
            RecreateTaskPrefabs();
        }
    }







    private void RecreateTaskPrefabs()
    {
        foreach (Transform child in tasksParent)
        {
            Destroy(child.gameObject);
        }

        foreach (Task task in tasks)
        {
            int rowIndex = (tasks.Count - 1) / maxColumns;
            int columnIndex = (tasks.Count - 1) % maxColumns;
            Vector3 taskPosition = tasksParent.position + spawnOffset + new Vector3(columnIndex * (gridCellSize.x + gridSpacing.x), -rowIndex * (gridCellSize.y + gridSpacing.y), 0);
            GameObject newTaskObject = Instantiate(taskPrefab, taskPosition, Quaternion.identity, tasksParent);
            TaskPrefab newTaskPrefab = newTaskObject.GetComponent<TaskPrefab>();
            newTaskPrefab.SetTaskInfo(task.name);
            Slider timerSlider = newTaskObject.GetComponentInChildren<Slider>();
            timerSlider.maxValue = task.hoursToComplete;
            float remainingTimeSeconds = (float)(task.dueDate - DateTime.Now).TotalSeconds;
            StartCoroutine(CountdownTask(timerSlider, task.dueDate, remainingTimeSeconds));
        }
    }

    private IEnumerator CountdownTask(Slider timerSlider, DateTime dueDate, float remainingTimeSeconds)
    {
        float totalTimeSeconds = (float)(dueDate - DateTime.Now).TotalSeconds;

        float initialSliderValue = Mathf.Clamp01(remainingTimeSeconds / totalTimeSeconds) * timerSlider.maxValue;
        timerSlider.value = initialSliderValue;

        while (DateTime.Now < dueDate)
        {
            float elapsedTimeSeconds = (float)(dueDate - DateTime.Now).TotalSeconds;
            float sliderValue = Mathf.Clamp01(elapsedTimeSeconds / totalTimeSeconds) * timerSlider.maxValue;
            timerSlider.value = sliderValue;
            yield return null;
        }


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
}
