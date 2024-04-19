using UnityEngine;
using TMPro;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

[System.Serializable]
public class Task
{
    public string name;

    public Task(string _name)
    {
        name = _name;

    }


}

public class TimeManager : MonoBehaviour
{
    public List<Task> tasks = new List<Task>();
    private Dictionary<string, bool> suggestedTasks = new Dictionary<string, bool>();
    private Dictionary<string, Transform> taskGroupParents = new Dictionary<string, Transform>();

    public TMP_InputField taskNameInput;

    public TMP_Dropdown taskSuggestionsDropdown; // Reference to the dropdown UI element

    public GameObject taskPrefab;
    public Transform tasksParent;
    public Vector2 gridCellSize = new Vector2(200, 100);
    public Vector2 gridSpacing = new Vector2(20, 20);
    public Vector3 spawnOffset = new Vector3(0, 0, 0);
    public Color gizmoColor = Color.blue;
    public int maxColumns = 3;
    public Scrollbar hoursToCompleteScrollbar;

    public VerticalLayoutGroup verticalLayoutGroup;

    private const string TaskInfosKey = "TaskInfos";
    private void Start()
    {
        TaskPrefab.TaskDeleteEvent += OnTaskRemove; // Subscribe to the task delete event
        LoadTasks();
    }

    private void OnTaskRemove(string taskName)
    {
        // Find the task with the given name
        Task taskToRemove = tasks.Find(task => task.name == taskName);
        if (taskToRemove != null)
        {
            // Remove the task from the list
            tasks.Remove(taskToRemove);

            // Save the removed task data to another local storage
            SaveRemovedTask(taskToRemove);

            // Save the updated tasks list
            SaveTasks();
        }

        // Destroy the corresponding TaskPrefab GameObject
        foreach (Transform child in tasksParent)
        {
            TaskPrefab taskPrefab = child.GetComponent<TaskPrefab>();
            if (taskPrefab != null && taskPrefab.TaskName == taskName)
            {
                Destroy(child.gameObject);
                break; // Break out of the loop once the TaskPrefab is found and destroyed
            }
        }
    }

    private void SaveRemovedTask(Task removedTask)
    {
        // Retrieve the existing removed tasks string from PlayerPrefs
        string removedTasksString = PlayerPrefs.GetString("RemovedTasks", "");

        // Check if the existing string is empty
        if (!string.IsNullOrEmpty(removedTasksString))
        {
            // Append a comma if the string is not empty
            removedTasksString += ",";
        }

        // Append the information of the removed task to the existing string
        string taskInfo = $"{removedTask.name}";
        removedTasksString += taskInfo;

        // Save the updated removed tasks string back to PlayerPrefs
        PlayerPrefs.SetString("RemovedTasks", removedTasksString);


        PlayerPrefs.Save();
    }


    public void AddTask()
    {
        string taskName = taskNameInput.text;

        tasks.Add(new Task(taskName)); // Pass the group name to the Task constructor

        int rowIndex = (tasks.Count - 1) / maxColumns;
        int columnIndex = (tasks.Count - 1) % maxColumns;

        Vector3 taskPosition = tasksParent.position + spawnOffset + new Vector3(columnIndex * (gridCellSize.x + gridSpacing.x), -rowIndex * (gridCellSize.y + gridSpacing.y), 0);

        GameObject newTaskObject = Instantiate(taskPrefab, tasksParent.GetChild(1)); // Assuming the task parent group's transform is the second child
        TaskPrefab newTaskPrefab = newTaskObject.GetComponent<TaskPrefab>();
        newTaskPrefab.SetTaskInfo(taskName);


        taskNameInput.text = "";

        SaveTasks();

        StartCoroutine(UpdateScrollAreaGroups());
    }


    private IEnumerator UpdateScrollAreaGroups()
    {
        verticalLayoutGroup.spacing = 2.01f;

        // Wait for a specific amount of time
        yield return new WaitForSeconds(0.01f);

        verticalLayoutGroup.spacing = 2f;

    }


    public void RemoveTask(int index)
    {
        tasks.RemoveAt(index);
        SaveTasks();
    }

    public void SaveTasks()
    {
        List<string> taskInfos = new List<string>();

        foreach (Task task in tasks)
        {

            // Concatenate the task information with remaining time, slider value, and color using '|' as delimiter
            string taskInfo = $"{task.name}";

            // Add the task information to the list
            taskInfos.Add(taskInfo);
        }

        string taskInfosString = string.Join(",", taskInfos);
        PlayerPrefs.SetString("TaskInfos", taskInfosString);
        PlayerPrefs.Save();

        Debug.Log("Task information saved: " + taskInfosString);

        LoadTasks();
    }



    private void OnApplicationQuit()
    {
        SaveTasks(); // Save remaining time for tasks
    }


    public void LoadTasks()
    {
        if (PlayerPrefs.HasKey("TaskInfos"))
        {
            string taskInfosString = PlayerPrefs.GetString("TaskInfos");
            string[] taskInfos = taskInfosString.Split(',');

            tasks.Clear();

            Debug.Log("Loading tasks...");

            foreach (string taskInfo in taskInfos)
            {
                Debug.Log("Task info: " + taskInfo);

                string[] info = taskInfo.Split('|');
                string taskName = info[0];


                Debug.Log("Task name: " + taskName);

                Task task = new Task(taskName);

                tasks.Add(task);


            }

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
            int rowIndex = (tasks.IndexOf(task)) / maxColumns;
            int columnIndex = (tasks.IndexOf(task)) % maxColumns;
            Vector3 taskPosition = tasksParent.position + spawnOffset + new Vector3(columnIndex * (gridCellSize.x + gridSpacing.x), -rowIndex * (gridCellSize.y + gridSpacing.y), 0);
            GameObject newTaskObject = Instantiate(taskPrefab, taskPosition, Quaternion.identity, tasksParent);
            TaskPrefab newTaskPrefab = newTaskObject.GetComponent<TaskPrefab>();
            newTaskPrefab.SetTaskInfo(task.name); // Pass color information to TaskPrefab


        }
    }



}
