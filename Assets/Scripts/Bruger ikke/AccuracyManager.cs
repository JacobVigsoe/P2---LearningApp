using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class AccuracyManager : MonoBehaviour
{
    private TaskManager taskManager;
    private static AccuracyManager instance;
    public static AccuracyManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AccuracyManager>();
                if (instance == null)
                {
                    GameObject accuracyManagerObject = new GameObject("AccuracyManager");
                    instance = accuracyManagerObject.AddComponent<AccuracyManager>();
                    DontDestroyOnLoad(accuracyManagerObject);
                }
            }
            return instance;
        }
    }

    public List<float> accuracyValues = new List<float>();

    public float[] AccuracyValues => accuracyValues.ToArray();

    private void Awake()
    {
        LoadAccuracyValues();
    }

    private void Start()
    {
        taskManager = GameObject.FindObjectOfType<TaskManager>();
    }

    private void LoadAccuracyValues()
    {
        string filePath = Application.persistentDataPath + taskManager.lastClickedTask  + ".json";

        if (File.Exists(filePath))
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                // Skip the header line
                reader.ReadLine();

                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    if (!string.IsNullOrEmpty(line))
                    {
                        if (float.TryParse(line, out float accuracy))
                        {
                            accuracyValues.Add(accuracy);
                        }
                        else
                        {
                            Debug.LogWarning("Failed to parse accuracy value: " + line);
                        }
                    }
                }
            }
        }
        else
        {
            Debug.LogWarning("Accuracy file does not exist.");
        }
    }

    public void SaveAccuracyToCSV(float accuracy)
    {
        string filePath = Application.persistentDataPath + "/accuracy.csv";

        // Check if file exists, if not, create it and write headers
        if (!File.Exists(filePath))
        {
            using (StreamWriter writer = File.CreateText(filePath))
            {
                writer.WriteLine("AccuracyPercentage");
            }
        }

        // Append accuracy percentage to CSV file
        using (StreamWriter writer = File.AppendText(filePath))
        {
            writer.WriteLine(accuracy.ToString("00."));
        }

        LoadAccuracyValues();
    }
}
