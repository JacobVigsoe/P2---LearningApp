using UnityEngine;
using TMPro;
using System.IO;
using System;

public class CountdownTimer : MonoBehaviour
{
    public float countdownDuration = 60f; // Initial countdown duration in seconds

    private float originalTime; // Original time picked when starting the timer
    private float currentTime; // Current time left in the countdown
    private bool isCountingDown = false; // Flag to track if the countdown is active
    private AccuracyManager accuracyManager;

    private TaskManager taskManager;
    private WindowGraph windowGraph;
    private CoinsManager coinsManager;

    private DateTime pauseTime;
    private bool wasCountingDown;

    void Start()
    {
        coinsManager = GameObject.FindObjectOfType<CoinsManager>();
        taskManager = GameObject.FindObjectOfType<TaskManager>();
        accuracyManager = GameObject.FindObjectOfType<AccuracyManager>();
        windowGraph = GameObject.FindObjectOfType<WindowGraph>();
        ResetTimer();
    }

    void Update()
    {
        if (isCountingDown)
        {
            currentTime += Time.deltaTime;
            if (currentTime <= 0)
            {
                currentTime = 0;
                isCountingDown = false;
                
            }
        }
    }

    public void StartTimer(float duration)
    {
        originalTime = duration;
        isCountingDown = true;
    }

    void OnApplicationPause(bool pauseStatus)
    {
        HandleApplicationStateChange(pauseStatus);
    }

    void OnApplicationFocus(bool hasFocus)
    {
        HandleApplicationStateChange(!hasFocus);
    }

    private void HandleApplicationStateChange(bool isPaused)
    {
        Debug.Log(isPaused);
        if (isPaused)
        {
            // The application is paused (locked, in background, or lost focus)
            // Save the current time and whether the timer was counting down
            pauseTime = DateTime.Now;
            wasCountingDown = isCountingDown;
            isCountingDown = false;
        }
        else
        {
            // The application is resumed or regained focus
            // If the timer was counting down when the app was paused, resume the timer
            if (wasCountingDown)
            {
                // Calculate the duration the application was paused
                TimeSpan pausedDuration = DateTime.Now - pauseTime;

                // Subtract the paused duration from the current time
                currentTime -= (float)pausedDuration.TotalSeconds;

                // Ensure currentTime doesn't go below zero
                if (currentTime < 0)
                {
                    currentTime = 0;
                }

                isCountingDown = true;
            }
        }
    }

    public void StopTimer()
    {
        isCountingDown = false;

        // Calculate the absolute difference between original time and current time
        float timeDifference = Mathf.Abs(originalTime - currentTime);

        // Calculate accuracy percentage based on the ratio of time difference to original time
        float accuracyPercentage = 100f * (1f - (timeDifference / originalTime));

        if (accuracyPercentage <= 0)
        {
            accuracyPercentage = 0;
        }
        Debug.Log("Accuracy Percentage: " + accuracyPercentage.ToString("0.00") + "%");

        coinsManager.UpdateExperienceAmount(accuracyPercentage);
        taskManager.WriteToTask(timeDifference, accuracyPercentage, currentTime, originalTime);
        windowGraph.UpdateGraph();
    }

   
    public void ResetTimer()
    {
        currentTime = countdownDuration;;
    }

    public void AddTime(float timeToAdd)
    {
        currentTime = 0;
        originalTime = timeToAdd;

        StartTimer(originalTime);
    }
}