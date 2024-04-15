using UnityEngine;
using System;

public class CountdownManager : MonoBehaviour
{
    public ScrollSnap hoursScrollSnap; // Reference to the ScrollSnap script for hours
    public ScrollSnap minutesScrollSnap; // Reference to the ScrollSnap script for minutes
    public ScrollSnap secondsScrollSnap; // Reference to the ScrollSnap script for minutes

    public CountdownTimer countdownTimer; // Reference to the CountdownTimer script

    // Method to handle the button press
    public void AddTimeFromScrollSnaps()
    {
        int hours = hoursScrollSnap.GetCenteredValue();
        int minutes = minutesScrollSnap.GetCenteredValue();
        int seconds = secondsScrollSnap.GetCenteredValue();

        // Convert hours and minutes to seconds and add to the countdown timer
        float totalTimeInSeconds = hours * 3600 + minutes * 60 + seconds;
        countdownTimer.AddTime(totalTimeInSeconds);
    }
}
