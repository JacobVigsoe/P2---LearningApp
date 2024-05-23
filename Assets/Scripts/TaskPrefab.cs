using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using UnityEngine.Events;

public class TaskPrefab : MonoBehaviour
{
    private SaveData saveData;

    [Header("References")]
    public TMP_Text taskNameText;
    public Button nextButton; // Reference to the delete button
    private TaskManager taskManager;
    public GameObject arrowButton;
    public GameObject DeleteButton;
    public RectTransform targetRectTransform;
    public TMP_Text avgTime;
    public WindowGraph windowGraph;

    [Header("settings")]
    public Vector3 TargetScale;
    public Vector2 OffsetRight;
    public float AnimSpeed;
    public string TaskName { get; private set; }
    public delegate void OnTaskDelete(string taskName);
    public static event OnTaskDelete TaskDeleteEvent; // Event for deleting task
    private UIManager uimanager;

    private void Start()
    {
        windowGraph = GameObject.FindObjectOfType<WindowGraph>();
        saveData = SaveData.instance;
        taskManager = GameObject.FindObjectOfType<TaskManager>();
        uimanager = GameObject.FindObjectOfType<UIManager>();
        targetRectTransform.DOScale(TargetScale, AnimSpeed);
    }

    public void SetTaskInfo(string taskName, float avgDeviation)
    {
        TaskName = taskName;
        taskNameText.text = taskName;
        avgTime.text += avgDeviation.ToString("F2") + "%";
 
        nextButton.onClick.AddListener(OnButtonClick); // Subscribe to the button click event
    }

    public void OnButtonClick()
    {
        uimanager.taskStatsMenu();

        taskManager.lastClickedTask = TaskName;
        taskManager.OpenTask(TaskName);
        
        windowGraph.UpdateGraph();
    }

    public void DeleteTask()
    {
        saveData.DeleteTask(TaskName);
    }

    private IEnumerator WaitForAnimation()
    {
        yield return new WaitForSeconds(AnimSpeed);
        TaskDeleteEvent?.Invoke(TaskName); // Trigger the event passing the task name 
    }

    public void EditTask()
    {
        if(arrowButton.activeSelf == true)
        {
            nextButton.interactable = false;
            arrowButton.SetActive(false);
            DeleteButton.SetActive(true);
        }
        else
        {
            nextButton.interactable = true;
            arrowButton.SetActive(true);
            DeleteButton.SetActive(false);
        }
    }



}
