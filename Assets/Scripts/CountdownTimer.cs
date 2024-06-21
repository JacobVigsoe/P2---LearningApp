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
        // Get the current time
        DateTime endTime = DateTime.Now;

        // Calculate the time span between the start time and end time
        TimeSpan timespan = endTime - startTime;

        // Convert the time span to total seconds
        float timeSpent = (float)timespan.TotalSeconds;

        // Calculate the accuracy percentage based on the time spent
        float accuracyPercentage = CalculateAccuracyPercentage(timeSpent);

        // Calculate the time off based on the time spent
        float timeOff = CalculateTimeOff(timeSpent);

        // Update the user interface with the time span and time off
        UpdateUI(timespan, timeOff);

        // Log the time off for debugging purposes
        Debug.Log("Time off: " + timeOff);
        // Log the accuracy percentage for debugging purposes
        Debug.Log("Accuracy: " + accuracyPercentage);

        // Update other classes with the accuracy percentage, time off, and time spent
        UpdateOtherClasses(accuracyPercentage, timeOff, timeSpent);

        // Recreate tasks in the task manager
        taskManager.ReCreateTasks();
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