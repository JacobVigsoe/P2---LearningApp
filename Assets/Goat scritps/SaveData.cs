using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml.Serialization;

public class SaveData : MonoBehaviour
{


    public void SaveTasks(List<TaskInfo> tasks)
    {
        var xmlSerializer = new XmlSerializer(typeof(List<TaskInfo>));

        foreach(TaskInfo task in tasks)
        {
            task.filePath = Application.dataPath + "/TaskInfo/" + task.taskName + ".xml";

            using (FileStream stream = File.Create(Application.dataPath + "/TaskInfo/" + task.taskName + ".xml"))
            {
                xmlSerializer.Serialize(stream, tasks);
            }
        }
        
    }


    
}
