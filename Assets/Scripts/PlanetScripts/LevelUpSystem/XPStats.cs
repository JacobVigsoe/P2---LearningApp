using UnityEngine;

public class XPStats : MonoBehaviour
{
    [SerializeField] public int CurrentXP, MaxXP, CurrentLevel;

    public SpawnOnObject spawnOnObjectScript; // Reference to the SpawnOnObject script

    private void OnEnable()
    {
        // Subscribe Event
        XPManager.instance.OnExperienceChange += HandleExperienceChange;
    }

    private void OnDisable()
    {
        // Unsubscribing from event
        XPManager.instance.OnExperienceChange -= HandleExperienceChange;
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

        // Spawn a tree dynamically
        SpawnTreeDynamically();
    }

    private void SpawnTreeDynamically()
    {
        spawnOnObjectScript.numberOfTreesToSpawn = 5;
        if (spawnOnObjectScript != null)
        {
            // Call the SpawnTrees method of the SpawnOnObject script
            spawnOnObjectScript.SpawnTrees();
        }
        else
        {
            Debug.LogError("SpawnOnObject script reference is not assigned.");
        }
    }
}
