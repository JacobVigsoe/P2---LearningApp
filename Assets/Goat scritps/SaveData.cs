using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml.Serialization;
using System.Linq;

public class SaveData : MonoBehaviour
{
    public TaskManager taskManager;

    public void SaveTasks(TaskInfo task)
    {
        task.filePath = Application.dataPath + "/TaskInfo/" + task.taskName + ".json";

        string json = JsonUtility.ToJson(task);

        File.WriteAllText(task.filePath, json);
    }

    public List<TaskInfo> LoadTasks()
    {
        List<TaskInfo> tasks = new List<TaskInfo>();

        string directoryPath = Application.dataPath + "/TaskInfo/";

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
        string directoryPath = Application.dataPath + "/TaskInfo/";
        string filePath = directoryPath + name + ".json";

        if (Directory.Exists(directoryPath) && Directory.EnumerateFileSystemEntries(directoryPath).Any())
        {
            taskManager.DeleteTask(name);
            File.Delete(filePath);
        }
    }
}
