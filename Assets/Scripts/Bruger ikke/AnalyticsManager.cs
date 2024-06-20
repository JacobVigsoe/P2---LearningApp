using UnityEngine;
using TMPro;

public class AnalyticsManager : MonoBehaviour
{
    public TMP_Text completedTasksText; // Reference to the text UI element to display completed tasks count

    void Start()
    {
        // Load completed tasks count from PlayerPrefs and update the UI
        UpdateCompletedTasksCount();
    }
    private void FixedUpdate()
    {
        UpdateCompletedTasksCount();
    }

    public void UpdateCompletedTasksCount()
    {
        // Retrieve the stored removed tasks string from PlayerPrefs
        string removedTasksString = PlayerPrefs.GetString("RemovedTasks", "");

        // Check if the string is empty
        if (string.IsNullOrEmpty(removedTasksString))
        {
            // If the string is empty, there are no completed tasks
            completedTasksText.text = "Completed Tasks: 0";
            return;
        }

        // Split the string into an array of task information
        string[] removedTaskInfos = removedTasksString.Split(',');

        // Update the completed tasks count based on the number of task infos
        int completedTasksCount = removedTaskInfos.Length;

        // Update the UI text to display the completed tasks count
        completedTasksText.text = "Completed Tasks: " + completedTasksCount;
    }


    public void DeleteStoredTasks()
    {
        // Clear the stored removed tasks from PlayerPrefs
        PlayerPrefs.DeleteKey("RemovedTasks");

        UpdateCompletedTasksCount();
    }
}
