using System;
using TMPro;
using UnityEngine;

public class CurrentTaskInformation : MonoBehaviour
{
    public Task task;
    public TimeManager timeManager;
    public TMP_Text taskTimerText;

    public void SetTaskInformation(string information)
    {
        taskTimerText.text = information;
    }
}
