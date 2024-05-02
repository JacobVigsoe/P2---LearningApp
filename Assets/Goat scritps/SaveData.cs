using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml.Serialization;
using System.Linq;
using System;

public class SaveData : MonoBehaviour
{
    public TaskManager taskManager;
    private string directoryPath;
    private string userData;
    private string userDataPath;

    void Awake()
    {
        directoryPath = Application.dataPath + "/TaskInfo/";
        userDataPath = Application.dataPath + "/UserData/";
        userData = directoryPath + "UserData.json";
    }

    public void SaveTasks(TaskInfo task)
    {
        task.filePath = directoryPath + task.taskName + ".json";

        string json = JsonUtility.ToJson(task);

        File.WriteAllText(task.filePath, json);
    }
    public List<TaskInfo> LoadTasks()
    {
        List<TaskInfo> tasks = new List<TaskInfo>();

        if (Directory.Exists(directoryPath) && Directory.EnumerateFileSystemEntries(directoryPath).Any())
        {
            foreach (var file in Directory.EnumerateFiles(directoryPath, "*.json"))
            {
                string json = File.ReadAllText(file);

                TaskInfo task = JsonUtility.FromJson<TaskInfo>(json);

                tasks.Add(task);
            }
        }
        return tasks;
    }
    public void DeleteTask(string name)
    {
        string filePath = directoryPath + name + ".json";

        if (Directory.Exists(directoryPath) && Directory.EnumerateFileSystemEntries(directoryPath).Any())
        {
            taskManager.DeleteTask(name);
            File.Delete(filePath);
        }
    }

    public void Save()
    {
        // Create a new directory for the save data
        if (! Directory.Exists(userDataPath))
            Directory.CreateDirectory(userDataPath);
        
        UserData data = new();

        // Save the UserData class to a json file
        string json = JsonUtility.ToJson(data);

        File.WriteAllText(userData, json);
    }
}

[Serializable]
public class UserData
{
    public int money;
    public int currentCar;
    public bool[] charactersUnlocked = new bool[6];
}