using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using TMPro;
using System.Linq;
using System;

public class TaskInfo 
{
    public string filePath;
    public string taskName;
    public float avgTimeDeviation;
    public float avgPercentage;
    public List<float> timeDeviations = new List<float>();
    public List<float> percentages = new List<float>();

    public void CalculateAverage()
    {
        avgTimeDeviation = 0;
        avgPercentage = 0;

        foreach (float timeDeviation in timeDeviations)
        {
            avgTimeDeviation += timeDeviation;
        }
        avgTimeDeviation /= timeDeviations.Count;

        foreach (float percentage in percentages)
        {
            avgPercentage += percentage;
        }
        avgPercentage /= percentages.Count;
    }

    public List<int> ReturnAsInt()
    {
        List<int> intList = new List<int>();

        foreach (float value in percentages)
        {
            intList.Add((int)value);
        }

        return intList;
    }

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
    public TMP_Text title;
    public TMP_Text avgTimeDeviation;
    public TMP_Text avgPercentage;

    public string lastClickedTask;

    void Awake()
    {
        filePath = Application.dataPath + "/TaskInfo";
    }
    void Start()
    {
        tasks = saveData.LoadTasks();
        Debug.Log(tasks.Count); 
        ReCreateTasks();
    }
    public void AddTask()
    {
        tasks.Add(new TaskInfo
        {
            taskName = taskNameInput.text,
            avgTimeDeviation = 0,
            avgPercentage = 0,
            filePath = filePath + "/" + taskNameInput.text + ".json"
        });
        saveData.SaveTasks(tasks[tasks.Count - 1]);

        ReCreateTasks();
    }
    public void ReCreateTasks()
    {
        foreach (Transform child in taskParent)
        {
            Destroy(child.gameObject);
        }

        foreach (TaskInfo task in tasks)
        {
            int rowIndex = tasks.IndexOf(task);
            int columnIndex = tasks.IndexOf(task);
            Vector3 taskPosition = taskParent.position + spawnOffset + new Vector3(columnIndex * (gridCellSize.x + gridSpacing.x), -rowIndex * (gridCellSize.y + gridSpacing.y), 0);
            GameObject newTaskObject = Instantiate(taskPrefab, taskPosition, Quaternion.identity, taskParent);
            TaskPrefab newTaskPrefab = newTaskObject.GetComponent<TaskPrefab>();
            newTaskPrefab.SetTaskInfo(task.taskName, task.avgTimeDeviation); // Pass info to TaskPrefab
        }
    }
    public void OpenTask(string taskName)
    {
        if(taskName == tasks.Find(x => x.taskName == taskName).taskName)
        {
            title.text = taskName;
            
            TaskInfo task = tasks.Find(x => x.taskName == taskName);
            
            if(task.avgTimeDeviation < 60)
                avgTimeDeviation.text = task.avgTimeDeviation.ToString("F2") + " sec";
            else
                avgTimeDeviation.text = (task.avgTimeDeviation / 60).ToString("F2") + " min";

            avgPercentage.text = task.avgPercentage.ToString("F2") + " %";
        }
    }
    public void EditButton()
    {
        foreach (TaskPrefab child in taskParent.GetComponentsInChildren<TaskPrefab>())
        {
            child.EditTask();
        }
    }
    public void DeleteTask(string name)
    {
        tasks.Remove(tasks.Find(x => x.taskName == name));
        ReCreateTasks();
    }
    public void WriteToTask(float difference, float percentage)
    {
        name = lastClickedTask;

        TaskInfo task = tasks.Find(x => x.taskName == name);

        task.timeDeviations.Add(difference);
        task.percentages.Add(percentage);

        task.CalculateAverage();

        Debug.Log(task.avgPercentage);
        
        saveData.SaveTasks(task);
    }

    public List<int> GetPercentage()
    {
        TaskInfo task = tasks.Find(x => x.taskName == lastClickedTask);

        return task.ReturnAsInt();
    }

}
