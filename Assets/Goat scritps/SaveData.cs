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
    private string taskDataPath;

    // User stuff
    public int money;
    public bool[] charactersUnlocked = new bool[6] {true, false, false, false, false, false};
    public int currentCharacter = 1;
    public static SaveData instance;

    // Shop interface
    public TMPro.TextMeshProUGUI moneyText;

    void Awake()
    {
        directoryPath = Application.dataPath + "/SaveData/";
        taskDataPath = directoryPath + "/TaskData/";

        userDataPath = directoryPath;
        userData = directoryPath + "UserData.json";

        if (instance != null && instance != this)
            Destroy(gameObject);
        else
            instance = this;

        LoadUserData();
        AdjustMoney(0);
    }
    public void SaveTasks(TaskInfo task)
    {
        task.filePath = taskDataPath + task.taskName + ".json";

        string json = JsonUtility.ToJson(task);

        File.WriteAllText(task.filePath, json);
    }
    public List<TaskInfo> LoadTasks()
    {
        List<TaskInfo> tasks = new List<TaskInfo>();

        if (Directory.Exists(taskDataPath) && Directory.EnumerateFileSystemEntries(taskDataPath).Any())
        {
            foreach (var file in Directory.EnumerateFiles(taskDataPath, "*.json"))
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
        string filePath = taskDataPath + name + ".json";

        if (Directory.Exists(taskDataPath) && Directory.EnumerateFileSystemEntries(taskDataPath).Any())
        {
            taskManager.DeleteTask(name);
            File.Delete(filePath);
            Debug.Log("Task deleted");
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
        data.charactersUnlocked = charactersUnlocked;

        // Save the UserData class to a json file
        string json = JsonUtility.ToJson(data);

        File.WriteAllText(userData, json);
    }
    public void LoadUserData()
    {
        if(! Directory.Exists(userDataPath) || ! File.Exists(userData))
            return;
        
        string json = File.ReadAllText(userData);

        UserData data =  JsonUtility.FromJson<UserData>(json);

        money = data.money;
        currentCharacter = data.currentCharacter;
        charactersUnlocked = data.charactersUnlocked;
    }
    public void AdjustMoney(int amount)
    {
        money += amount;
        moneyText.text = money.ToString();
        SaveUserData();
    }
}
public class UserData
{
    public int money;
    public int currentCharacter;
    public bool[] charactersUnlocked;
}