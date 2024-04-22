using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class XPStats : MonoBehaviour
{
    [SerializeField] public int CurrentXP, MaxXP, CurrentLevel;

    public SpawnOnObject spawnOnObjectScript; // Reference to the SpawnOnObject script

    private XPManager xpManager;
    private string prefix = "XPStats_"; // Prefix for PlayerPrefs keys
    void Start()
    {
        CurrentLevel = PlayerPrefs.GetInt(prefix + "CurrentLevel", 1); // Default to level 1 if not found
        MaxXP = PlayerPrefs.GetInt(prefix + "MaxXP", 300); // Default to 100 if not found
        CurrentXP = PlayerPrefs.GetInt(prefix + "CurrentXP", 0); // Default to 0 if not found

        // Get a reference to the XPManager instance in Start
        xpManager = GetComponent<XPManager>();
        if (xpManager != null)
        {
            xpManager.OnExperienceChange += HandleExperienceChange;
        }
        else
        {
            Debug.LogError("Could not find XPManager component.");
        }
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt(prefix + "CurrentLevel", CurrentLevel);
        PlayerPrefs.SetInt(prefix + "MaxXP", MaxXP);
        PlayerPrefs.SetInt(prefix + "CurrentXP", CurrentXP);

        // Save PlayerPrefs
        PlayerPrefs.Save();
    }

    private void HandleExperienceChange(int newExperience)
    {
        CurrentXP += newExperience;
        if (CurrentXP >= MaxXP)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        // Increment level and reset XP
        CurrentLevel++;
        CurrentXP = 0;
        MaxXP += 100;
    }
}
