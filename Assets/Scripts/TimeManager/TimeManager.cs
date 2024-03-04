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
    public float savedSliderValue; // New property to store saved slider value

    public Task(string _name, int _hoursToComplete, DateTime _dueDate)
    {
        name = _name;
        hoursToComplete = _hoursToComplete;
        dueDate = _dueDate;
        remainingTimeSeconds = CalculateRemainingTime(); // Initialize remaining time
        savedSliderValue = CalculateSavedSliderValue();
    }

    // Method to calculate remaining time
    public float CalculateRemainingTime()
    {
        TimeSpan timeRemaining = dueDate - DateTime.Now;
        return Mathf.Max((float)timeRemaining.TotalSeconds, 0f);
    }

    // Method to update remaining time and saved slider value
    public void UpdateRemainingTime()
    {
        remainingTimeSeconds = CalculateRemainingTime();
        savedSliderValue = CalculateSavedSliderValue();
    }


    // Method to calculate saved slider value
    public float CalculateSavedSliderValue()
    {
        // Calculate the slider value based on the remaining time and total hours to complete
        return Mathf.Clamp01(remainingTimeSeconds / (hoursToComplete * 3600)); // Convert hours to seconds
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
        
        Initialize();
        LoadTasks();
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

        // Get the saved slider value for this task
        float savedSliderValue = GetSavedSliderValue(taskName);

        // Calculate remaining time
        float remainingTimeSeconds = (float)(dueDate - DateTime.Now).TotalSeconds;

        // Start the countdown coroutine with the correct remaining time and saved slider value
        StartCoroutine(CountdownTask(timerSlider, dueDate, remainingTimeSeconds, savedSliderValue));

        taskNameInput.text = "";
        hoursToCompleteInput.text = "";

        SaveTasks();
    }

    private float GetSavedSliderValue(string taskName)
    {
        // Retrieve the saved slider value from PlayerPrefs using the task's name
        if (PlayerPrefs.HasKey(taskName + "_SliderValue"))
        {
            return PlayerPrefs.GetFloat(taskName + "_SliderValue");
        }
        return 0f; // Return 0 if the saved slider value doesn't exist
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
            // Update the remaining time for the task
            task.UpdateRemainingTime();

            // Get the slider value from the task's slider component
            Slider timerSlider = FindSliderForTask(task);
            float sliderValue = timerSlider != null ? timerSlider.value : 0f;

            // Concatenate the task information with remaining time and slider value using '|' as delimiter
            string taskInfo = $"{task.name}|{task.hoursToComplete}|{task.remainingTimeSeconds}|{sliderValue}|{task.savedSliderValue}";

            // Add the task information to the list
            taskInfos.Add(taskInfo);

            // Save the slider value for this task separately
            PlayerPrefs.SetFloat(task.name + "_SliderValue", sliderValue);
            PlayerPrefs.SetFloat(task.name + "_SavedSliderValue", task.savedSliderValue);
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
        SaveTasks(); // Save remaining time for tasks
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
                // Split the task information into name, hoursToComplete, remainingTime, sliderValue, and savedSliderValue using '|' as delimiter
                string[] info = taskInfo.Split('|');
                string taskName = info[0];
                int hoursToComplete = int.Parse(info[1]);
                float remainingTimeSeconds = float.Parse(info[2]);
                float sliderValue = float.Parse(info[3]);
                float savedSliderValue = float.Parse(info[4]);

                // Calculate due date based on remaining time
                DateTime dueDate = DateTime.Now.AddSeconds(remainingTimeSeconds);

                // Create a new Task object and add it to the tasks list
                Task task = new Task(taskName, hoursToComplete, dueDate);
                task.remainingTimeSeconds = remainingTimeSeconds; // Update remaining time
                task.savedSliderValue = savedSliderValue; // Assign saved slider value

                // Set the saved slider value back to the task
                task.savedSliderValue = PlayerPrefs.GetFloat(taskName + "_SavedSliderValue", sliderValue);

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

            // Get the slider component inside the task prefab
            Slider timerSlider = newTaskPrefab.GetComponentInChildren<Slider>();
            if (timerSlider != null)
            {
                // Set the max value of the slider
                timerSlider.maxValue = task.hoursToComplete;
                // Set the slider value to the saved value
                timerSlider.value = task.savedSliderValue;
            }

            // Start the countdown coroutine with the correct remaining time and saved slider value
            StartCoroutine(CountdownTask(timerSlider, task.dueDate, task.remainingTimeSeconds, task.savedSliderValue));
        }
    }


    private IEnumerator CountdownTask(Slider timerSlider, DateTime dueDate, float remainingTimeSeconds, float savedSliderValue)
    {
        // Check if the timer has already started
        if (DateTime.Now < dueDate)
        {
            // Calculate the total time remaining
            float totalTimeSeconds = (float)(dueDate - DateTime.Now).TotalSeconds;

            // Set the initial slider value to the saved value
            timerSlider.value = savedSliderValue;

            // Start the countdown from the saved slider value
            while (DateTime.Now < dueDate)
            {
                // Update the remaining time
                remainingTimeSeconds = (float)(dueDate - DateTime.Now).TotalSeconds;

                // Calculate the slider value based on the remaining time and the total time
                float sliderValue = Mathf.Clamp01(remainingTimeSeconds / totalTimeSeconds) * timerSlider.maxValue;

                // Set the slider value
                timerSlider.value = savedSliderValue;

                yield return null;
            }
        }
        else
        {
            // If the timer has already ended, set the slider value to its maximum
            timerSlider.value = timerSlider.maxValue;
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
