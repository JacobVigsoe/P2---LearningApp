using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class XPManager : MonoBehaviour
{

    public static XPManager instance;

    public delegate void ExperienceChangeHandler(int amount);
    public event ExperienceChangeHandler OnExperienceChange;
    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    // public TMP_Text CurrentLevel;
    public XPStats xpStats;
    public TMP_Text XPtext;
    int XPAmount = 100;

    private void FixedUpdate()
    {
        // Get the current XP value from xpStats and convert it to a string
        string xpValueText = xpStats.CurrentXP.ToString();

        // Concatenate "XP" with the XP value string
        string formattedText = "XP " + xpValueText;

        // Set the formatted text to the Text component
        XPtext.text = formattedText;

    }

    public void AddExperience(int amount)
    {
        OnExperienceChange?.Invoke(amount);
    }

    
}
