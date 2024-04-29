using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml.Serialization;
using System.Linq;

public class SaveData : MonoBehaviour
{
    /*
    public void SaveTasks(TaskInfo task)
    {
        //var xmlSerializer = new XmlSerializer(typeof(TaskInfo));
        var xmlRoot = new XmlRootAttribute { ElementName = "TaskInfo", Namespace = "" };
        var xmlSerializer = new XmlSerializer(typeof(TaskInfo), xmlRoot);   

        task.filePath = Application.dataPath + "/TaskInfo/" + task.taskName + ".xml";

        using (FileStream stream = File.Create(Application.dataPath + "/TaskInfo/" + task.taskName + ".xml"))
        {
            xmlSerializer.Serialize(stream, task);
        }
    }
    */
    /*
    public List<TaskInfo> LoadTasks()
    {
        //var xmlSerializer = new XmlSerializer(typeof(TaskInfo));
        var xmlRoot = new XmlRootAttribute { ElementName = "TaskInfo", Namespace = "" };
        var xmlSerializer = new XmlSerializer(typeof(TaskInfo), xmlRoot);

        List<TaskInfo> tasks = new List<TaskInfo>();

        string directoryPath = Application.dataPath + "/TaskInfo/";

        if (Directory.Exists(directoryPath) && Directory.EnumerateFileSystemEntries(directoryPath).Any())
        {
            Debug.Log(directoryPath);
            foreach (var file in Directory.EnumerateFiles(directoryPath, "*.xml"))
            {
                using (FileStream stream = File.OpenRead(file))
                {
                    var task = xmlSerializer.Deserialize(stream) as TaskInfo;
                    tasks.Add(task);
                }
            }
        }

        return tasks;
    }
    */

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
            Debug.Log(directoryPath);
            foreach (var file in Directory.EnumerateFiles(directoryPath, "*.json"))
            {
                string json = File.ReadAllText(file);

                TaskInfo task = JsonUtility.FromJson<TaskInfo>(json);

                tasks.Add(task);
            }
        }

        return tasks;
    }


}
