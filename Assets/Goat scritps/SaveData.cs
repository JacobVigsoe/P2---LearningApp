using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml.Serialization;
using System.Linq;
using System;
using System.Threading.Tasks;

public class SaveData : MonoBehaviour
{
    public TaskManager taskManager;
    public static SaveData instance;
    
    // Directory and file paths
    private string directoryPath;
    private string userData;
    private string userDataPath;
    private string taskDataPath;

    // User stuff
    public int money;
    public bool[] charactersUnlocked = new bool[6] {true, false, false, false, false, false};
    public int currentCharacter = 1;

    // Money display references
    public TMPro.TextMeshProUGUI moneyTextShop;
    public TMPro.TextMeshProUGUI moneyTextMainMenu;
    public TMPro.TextMeshProUGUI moneyTextProfile;

    void Awake()
    {
        CreateDirectory(Application.persistentDataPath + "/SaveData/");
        CreateDirectory(Application.persistentDataPath + "/SaveData/TaskData/");
        directoryPath = Application.persistentDataPath + "/SaveData/";
        taskDataPath = directoryPath + "/TaskData/";

        userDataPath = directoryPath;
        userData = directoryPath + "UserData.json";
        Debug.Log(userDataPath);

        if (instance != null && instance != this)
            Destroy(gameObject);
        else
            instance = this;

        LoadUserData();
        AdjustMoney(0);
    }

    private void CreateDirectory(string path)
    {
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
    }

    void Start()
    {
        if (taskManager.tasks.Count == 0)
        {
            // Lav en besked om at man skal trykke plus!
        }
    }

    public void SaveTasks(TaskInfo task)
    {
        task.filePath = taskDataPath + task.taskName + ".json";

        string json = JsonUtility.ToJson(task, true);

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
        moneyTextShop.text = money.ToString();
        moneyTextMainMenu.text = "$ " + money.ToString();
        moneyTextProfile.text = money.ToString();
        SaveUserData();
    }
}
public class UserData
{
    public int money;
    public int currentCharacter;
    public bool[] charactersUnlocked;
}