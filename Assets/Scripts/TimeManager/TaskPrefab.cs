using UnityEngine;
using TMPro;

public class TaskPrefab : MonoBehaviour
{
    public TMP_Text taskNameText;

    // Add a property to store the task name
    public string TaskName { get; private set; }

    public void SetTaskInfo(string taskName)
    {
        TaskName = taskName; // Store the task name
        taskNameText.text = taskName;
    } 
}
