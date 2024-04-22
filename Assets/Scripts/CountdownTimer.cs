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
    private WindowGraph windowGraph;

    void Start()
    {
        accuracyManager = GameObject.FindObjectOfType<AccuracyManager>();
        windowGraph = GameObject.FindObjectOfType<WindowGraph>();
        ResetTimer();
    }

    void Update()
    {
        if (isCountingDown)
        {
            currentTime -= Time.deltaTime;
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
        currentTime = duration;
        isCountingDown = true;
    }

    public void StopTimer()
    {
        isCountingDown = false;

        // Calculate accuracy percentage based on the inverse ratio of remaining time to original time
        float accuracyPercentage = 100f * (1f - (currentTime / originalTime));
        Debug.Log("Accuracy Percentage: " + accuracyPercentage.ToString("0.00") + "%");

        accuracyManager.SaveAccuracyToCSV(accuracyPercentage);
        windowGraph.UpdateGraph();
    }

   

    public void ResetTimer()
    {
        currentTime = countdownDuration;;
    }

    public void AddTime(float timeToAdd)
    {
        currentTime += timeToAdd;

        // Start counting down if not already
        StartTimer(currentTime);
    }
}
