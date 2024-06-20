using UnityEngine;
using TMPro;
using System.IO;
using System;

public class CountdownTimer : MonoBehaviour
{   
    // Internal variables
    private float duration; // Original time picked when starting the timer
    private DateTime startTime;

    // Script references
    private TaskManager taskManager;
    private WindowGraph windowGraph;
    private CoinsManager coinsManager;

    // Displaying info about time when finished
    public TMP_Text estimatedTimeText;
    public TMP_Text spentTimeText;
    public TMP_Text minutesOffText;

    void Start()
    {
        coinsManager = GameObject.FindObjectOfType<CoinsManager>();
        taskManager = GameObject.FindObjectOfType<TaskManager>();
        windowGraph = GameObject.FindObjectOfType<WindowGraph>();
    }
    public void AddTime(float timeToAdd)
    {
        // Getting how long the user aims for
        duration = timeToAdd;

        // Starting the time
        StartTimer();
    }
    public void StartTimer()
    {
        // Getting the time when the user starts the timer
        startTime = DateTime.Now;

        Debug.Log(startTime);
    }
    public void StopTimer()
    {
        DateTime endTime = DateTime.Now;
        TimeSpan timespan = CalculateTimeSpent(endTime);
        float timeSpent = (float)timespan.TotalSeconds;

        float accuracyPercentage = CalculateAccuracyPercentage(timeSpent);
        float timeOff = CalculateTimeOff(timeSpent);

        UpdateUI(timespan, timeOff);

        Debug.Log("Time off: " + timeOff);
        Debug.Log("Accuracy: " + accuracyPercentage);

        UpdateOtherClasses(accuracyPercentage, timeOff, timeSpent);

        taskManager.ReCreateTasks();
    }

    private TimeSpan CalculateTimeSpent(DateTime endTime)
    {
        return endTime - startTime;
    }

    private float CalculateAccuracyPercentage(float timeSpent)
    {
        float accuracyPercentage = 100f * (timeSpent / duration);

        if (timeSpent > duration)
        {
            accuracyPercentage = 200f - accuracyPercentage;
        }

        return Math.Max(accuracyPercentage, 0);
    }

    private float CalculateTimeOff(float timeSpent)
    {
        return Math.Abs(duration - timeSpent);
    }

    private void UpdateUI(TimeSpan timespan, float timeOff)
    {
        estimatedTimeText.text = ConvertSecondsToHoursMinutes(duration);
        spentTimeText.text = timespan.ToString(@"hh\:mm");
        minutesOffText.text = ConvertSecondsToHoursMinutes(Math.Abs(timeOff)) + " off!";
    }

    private void UpdateOtherClasses(float accuracyPercentage, float timeOff, float timeSpent)
    {
        coinsManager.UpdateExperienceAmount(accuracyPercentage);
        taskManager.WriteToTask(timeOff, accuracyPercentage, timeSpent, duration);
        windowGraph.UpdateGraph();
    }


    /// <summary>
    /// Converts a float into a string in the format hours:minutes
    /// </summary>
    /// <param name="seconds"></param>
    /// <returns></returns>
    public string ConvertSecondsToHoursMinutes(float seconds)
    {
        TimeSpan time = TimeSpan.FromSeconds(seconds);
        return string.Format("{0:D2}h:{1:D2}m", time.Hours, time.Minutes);
    }

}