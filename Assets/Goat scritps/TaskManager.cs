using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using TMPro;

public class TaskInfo 
{
    public string taskName;
    public float avgTimeDeviation;
    public float avgPercentage;
    public string filePath;
}

public class TaskManager : MonoBehaviour
{
    public SaveData saveData;

    // Task list settings
    public List<TaskInfo> tasks = new List<TaskInfo>();
    public TMP_InputField taskNameInput;
    private string filePath;

    // Task prefab settings
    public Transform taskParent;
    public GameObject taskPrefab;

    // Grid layout settings
    public Vector2 gridCellSize = new Vector2(200, 100);
    public Vector2 gridSpacing = new Vector2(20, 20);
    public Vector3 spawnOffset = new Vector3(0, 0, 0);

    // Open task stuff


    void Awake()
    {
        filePath = Application.dataPath + "/TaskInfo";
    }
    void Start()
    {
        ReCreateTasks();
    }


    public void AddTask()
    {
        tasks.Add(new TaskInfo
        {
            taskName = taskNameInput.text,
            avgTimeDeviation = 0,
            avgPercentage = 0,
            filePath = filePath + "/" + taskNameInput.text + ".csv"
        });
        ReCreateTasks();

        saveData.SaveTasks(tasks);
    }

    public void ReCreateTasks()
    {
        foreach (TaskInfo task in tasks)
        {
            int rowIndex = tasks.IndexOf(task);
            int columnIndex = tasks.IndexOf(task);
            Vector3 taskPosition = taskParent.position + spawnOffset + new Vector3(columnIndex * (gridCellSize.x + gridSpacing.x), -rowIndex * (gridCellSize.y + gridSpacing.y), 0);
            GameObject newTaskObject = Instantiate(taskPrefab, taskPosition, Quaternion.identity, taskParent);
            TaskPrefab newTaskPrefab = newTaskObject.GetComponent<TaskPrefab>();
            newTaskPrefab.SetTaskInfo(task.taskName); // Pass color information to TaskPrefab
        }
    }

    public void OpenTask()
    {
        
    }

}
