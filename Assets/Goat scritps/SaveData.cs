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
    
    // Directory and file paths
    private string directoryPath;
    private string userData;
    private string userDataPath;

    // User stuff
    public int money;
    public int currentCharacter;
    public static SaveData instance;

    void Awake()
    {
        directoryPath = Application.dataPath + "/TaskInfo/";
        userDataPath = Application.dataPath + "/UserData/";
        userData = directoryPath + "UserData.json";

        if (instance != null && instance != this)
            Destroy(gameObject);
        else
            instance = this;
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
    public void SaveUserData()
    {
        // Create a new directory for the save data
        if (! Directory.Exists(userDataPath))
            Directory.CreateDirectory(userDataPath);
        
        UserData data = new();

        data.money = money;
        data.currentCharacter = currentCharacter;

        // Save the UserData class to a json file
        string json = JsonUtility.ToJson(data);

        File.WriteAllText(userData, json);
    }
    public void LoadUserData()
    {
        if(! Directory.Exists(userDataPath))
            return;
        
        string json = File.ReadAllText(userData);

        UserData data =  JsonUtility.FromJson<UserData>(json);

        money = data.money;
        currentCharacter = data.currentCharacter;
    }   
    public void AdjustMoney(int amount)
    {
        money += amount;
        SaveUserData();
    }
}
public class UserData
{
    public int money;
    public int currentCharacter;
}