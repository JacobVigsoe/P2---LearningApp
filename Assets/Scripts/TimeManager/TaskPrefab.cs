using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using UnityEngine.Events;

public class TaskPrefab : MonoBehaviour
{
    public TMP_Text taskNameText;
    public Button nextButton; // Reference to the delete button
    private TaskManager taskManager;


    //Animations
    public RectTransform targetRectTransform;
    public Vector3 TargetScale;
    public Vector2 OffsetRight;
    public float AnimSpeed;
    public string TaskName { get; private set; }
    public delegate void OnTaskDelete(string taskName);
    public static event OnTaskDelete TaskDeleteEvent; // Event for deleting task

    private UIManager uimanager;
    private GetXPTest getXPTest;
    private TMP_Text remainingTimeText; // Reference to TMP text for remaining time
    private static TaskPrefab lastClickedTaskPrefab; // Reference to the last clicked TaskPrefab

    private void Start()
    {
        taskManager = GameObject.FindObjectOfType<TaskManager>();
        getXPTest = GameObject.FindObjectOfType<GetXPTest>();
        uimanager = GameObject.FindObjectOfType<UIManager>();
        targetRectTransform.DOScale(TargetScale, AnimSpeed);
        //remainingTimeText = GameObject.FindWithTag("RemainingTimeText").GetComponent<TMP_Text>();

    }

    public void Initialize(Task task, TimeManager manager)
    {
        TaskName = task.name;
        taskNameText.text = task.name;
    }

    public void SetTaskInfo(string taskName)
    {
        TaskName = taskName;
        taskNameText.text = taskName;
 
        nextButton.onClick.AddListener(OnButtonClick); // Subscribe to the button click event
    }


    public void OnButtonClick()
    {
        uimanager.taskStatsMenu();
        taskManager.OpenTask(TaskName);
    }

    private IEnumerator WaitForAnimation()
    {
        yield return new WaitForSeconds(AnimSpeed);
        TaskDeleteEvent?.Invoke(TaskName); // Trigger the event passing the task name 
    }
}
