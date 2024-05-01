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
    public TMP_Text LVLText;
    public TMP_Text XPToClaimText;
    int XPAmount = 100;


    private void FixedUpdate()
    {
        // Get the current XP value from xpStats and convert it to a string
        string xpValueText = xpStats.CurrentXP.ToString();
        string lvlValueText = xpStats.CurrentLevel.ToString();

        // Concatenate "XP" with the XP value string
        string formattedCurrentXPText = "XP " + xpValueText;
        string formattedLevelText = lvlValueText;

        // Set the formatted text to the Text component
        XPtext.text = formattedCurrentXPText;
        LVLText.text = formattedLevelText;

    }

    public void UpdateExperienceAmount(float XP)
    {
        // Check if valueToCheck is between minValue and maxValue
        if (XP > 0 && XP < 20)
        {
            XPToClaimText.text = 60.ToString() + " XP";
            AddExperience(60);
            Debug.Log("Gave 60 XP");
        }
        else if (XP > 20 && XP < 40)
        {
            XPToClaimText.text = 100.ToString() + " XP";
            AddExperience(100);
            Debug.Log("Gave 100 XP");
        }
        else if (XP > 40 && XP < 60)
        {
            XPToClaimText.text = 160.ToString() + " XP";
            AddExperience(160);
            Debug.Log("Gave 160 XP");
        }
        else if (XP > 60 && XP < 80)
        {
            XPToClaimText.text = 280.ToString() + " XP";
            AddExperience(280);
            Debug.Log("Gave 280 XP");
        }
        else if (XP > 80 && XP < 100)
        {
            XPToClaimText.text = 400.ToString() + " XP";
            AddExperience(400);
            Debug.Log("Gave 400 XP");
        }
    }

    public void AddExperience(int amount)
    {
        OnExperienceChange?.Invoke(amount);
    }


    
}
