using UnityEngine;
using TMPro;
using System;

public class TaskPrefab : MonoBehaviour
{
    public TMP_Text taskNameText;

    public void SetTaskInfo(string taskName)
    {
        taskNameText.text = taskName;
    }
}
