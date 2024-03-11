using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClockScript : MonoBehaviour
{
    public TMP_Text timeText;
    public TMP_Text dateText;
    public Slider weekSlider;

    private DateTime currentTime;
    private DateTime startOfWeek;
    private TimeSpan dayDuration;
    private TimeSpan elapsedWeekTime;
    private bool isRunning = false;
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
        StartTimer();
    }
    private void Update()
    {
        if (isRunning)
        {
            UpdateTimer();
        }
    }
    public void StartTimer()
    {
        isRunning = true;
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public void ResetTimer()
    {
        Initialize();
        UpdateDateTime();
    }
    private void UpdateTimer()
    {
        UpdateDateTime();
        UpdateDisplay();
    }

    private void Initialize()
    {
        startOfWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
        dayDuration = TimeSpan.FromHours(24);
        elapsedWeekTime = TimeSpan.Zero;
    }

    private void UpdateDateTime()
    {
        currentTime = DateTime.Now;
        elapsedWeekTime = currentTime - startOfWeek;
    }

    private void UpdateDisplay()
    {
        if (timeText != null)
        {
            timeText.text = currentTime.ToString("HH:mm:ss");
        }

        if (dateText != null)
        {
            dateText.text = currentTime.ToString("dddd, MMMM dd, yyyy");
        }

        if (weekSlider != null)
        {
            float fillAmount = (float)(elapsedWeekTime.TotalSeconds / dayDuration.TotalSeconds);
            weekSlider.value = fillAmount;
        }
    }
  
}
