using UnityEngine;
using TMPro;
using System.IO;

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