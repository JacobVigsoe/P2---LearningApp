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
        // Getting the time the user presses to end the task
        DateTime endTime = DateTime.Now;

        // Calculate the absolute difference between original time and current time
        TimeSpan timespan = (startTime - endTime).Duration();
        float timeSpent = (float)timespan.TotalSeconds;

        //Debug.Log("Time spent:" + timeSpent);

        // Calculate accuracy percentage based on the ratio of time difference to original time
        float accuracyPercentage = 100f * (timeSpent / duration);

        if (accuracyPercentage <= 0)
        {
            accuracyPercentage = 0;
        }

        // Calculating what the user missed by
        float timeOff = duration - timeSpent;

        // Setting the texts to display the values
        estimatedTimeText.text = ConvertSecondsToHoursMinutes(duration);
        spentTimeText.text = timespan.ToString(@"hh\:mm");
        minutesOffText.text = ConvertSecondsToHoursMinutes(timeOff) + " off!";

        // Updating other classes
        coinsManager.UpdateExperienceAmount(accuracyPercentage);
        taskManager.WriteToTask(timeOff, accuracyPercentage, timeSpent, duration);
        windowGraph.UpdateGraph();

        // Redrawing the classes on main menu so that the values are updated
        taskManager.ReCreateTasks();
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