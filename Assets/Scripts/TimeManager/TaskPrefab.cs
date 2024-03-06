using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class TaskPrefab : MonoBehaviour
{
    public TMP_Text taskNameText;
    public Button deleteButton; // Reference to the delete button

    //Animations
    public RectTransform targetRectTransform;
    public Vector3 TargetScale;
    public Vector2 OffsetRight;
    public float AnimSpeed;
    public string TaskName { get; private set; }
    public delegate void OnTaskDelete(string taskName);
    public static event OnTaskDelete TaskDeleteEvent; // Event for deleting task

    private void Start()
    {

        targetRectTransform.DOScale(TargetScale, AnimSpeed);
    }

    public void SetTaskInfo(string taskName)
    {
        TaskName = taskName;
        taskNameText.text = taskName;
        deleteButton.onClick.AddListener(OnDeleteButtonClick); // Subscribe to the button click event
    }

    private void OnDeleteButtonClick()
    {
        StartCoroutine(WaitForAnimation());
        targetRectTransform.DOAnchorPos(OffsetRight, AnimSpeed);
      
    }
    private IEnumerator WaitForAnimation()
    {
        yield return new WaitForSeconds(AnimSpeed);
        TaskDeleteEvent(TaskName); // Trigger the event passing the task name 
       
    }
}
