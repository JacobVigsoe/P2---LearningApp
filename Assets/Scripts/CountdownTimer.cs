using UnityEngine;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    public float countdownDuration = 60f; // Initial countdown duration in seconds
    public TMP_Text countdownText; // Reference to the TextMeshPro text element to display the countdown

    private float originalTime; // Original time picked when starting the timer
    private float currentTime; // Current time left in the countdown
    private bool isCountingDown = false; // Flag to track if the countdown is active

    void Start()
    {
        ResetTimer();
    }

    void Update()
    {
        if (isCountingDown)
        {
            currentTime -= Time.deltaTime;
            UpdateUI();
            if (currentTime <= 0)
            {
                currentTime = 0;
                isCountingDown = false;
            }
        }
    }

    void UpdateUI()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);
        countdownText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void StartTimer(float duration)
    {
        originalTime = duration;
        currentTime = duration;
        UpdateUI();
        isCountingDown = true;
    }

    public void StopTimer()
    {
        isCountingDown = false;

        // Calculate accuracy percentage based on the inverse ratio of remaining time to original time
        float accuracyPercentage = 100f * (1f - (currentTime / originalTime));
        Debug.Log("Accuracy Percentage: " + accuracyPercentage.ToString("0.00") + "%");
    }

    public void ResetTimer()
    {
        currentTime = countdownDuration;
        UpdateUI();
    }

    public void AddTime(float timeToAdd)
    {
        currentTime += timeToAdd;
        UpdateUI();

        // Start counting down if not already
        StartTimer(currentTime);
    }
}
