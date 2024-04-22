using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    [SerializeField] private float animationSpeed;
    public bool noteIsOpen = false;

    [Header("Menus")]
    [SerializeField] private RectTransform mainMenu;
    [SerializeField] private RectTransform noteMenu;
    [SerializeField] private RectTransform analyticsMenu;
    [SerializeField] private RectTransform ShopMenu;
    [SerializeField] private RectTransform TaskStatsMenu;
    [SerializeField] private RectTransform IdleMenu;
    [SerializeField] private RectTransform TaskCompletedMenu;

    [Header("Notes")]
    [SerializeField] private GameObject notePrefab;
    public List<GameObject> notesList = new List<GameObject>();
    public int currentNoteIndex = 0;

    [Header("Buttons")]
    [SerializeField] private RectTransform closeButton;
    [SerializeField] private GameObject nextNoteButton;

    // Offsets and positions
    private Vector2 origin = new Vector2(0, 0);
    private Vector2 offsetleft = new Vector2(-1440, 0);
    private Vector2 offsetUp = new Vector2(0, 3040);
    private Vector2 offsetRight = new Vector2(1440, 0);
    private Vector2 offsetDown = new Vector2(0, -3040);

    void Start()
    {
        mainMenu.DOAnchorPos(origin, animationSpeed);
    }

    public void setTimeMenu ()
    {
        mainMenu.DOAnchorPos(origin, animationSpeed);
        noteMenu.DOAnchorPos(origin, animationSpeed);
        closeButton.DOAnchorPos(offsetDown, animationSpeed);
        analyticsMenu.DOAnchorPos(offsetDown, animationSpeed);
        ShopMenu.DOAnchorPos(offsetleft, animationSpeed);
        TaskStatsMenu.DOAnchorPos(origin, animationSpeed);
    }
    public void AnalyticsMenu()
    {
        analyticsMenu.DOAnchorPos(origin, animationSpeed);
        mainMenu.DOAnchorPos(offsetUp, animationSpeed);
        noteMenu.DOAnchorPos(offsetRight, animationSpeed);
        ShopMenu.DOAnchorPos(offsetleft, animationSpeed);
        TaskStatsMenu.DOAnchorPos(offsetRight, animationSpeed);
    }

    public void shopMenu()
    {
        ShopMenu.DOAnchorPos(origin, animationSpeed);
        mainMenu.DOAnchorPos(offsetRight, animationSpeed);
        noteMenu.DOAnchorPos(offsetRight, animationSpeed);
        closeButton.DOAnchorPos(offsetDown, animationSpeed);
        analyticsMenu.DOAnchorPos(offsetDown, animationSpeed);
        TaskStatsMenu.DOAnchorPos(offsetRight, animationSpeed);
        IdleMenu.DOAnchorPos(offsetRight, animationSpeed);
        TaskCompletedMenu.DOAnchorPos(offsetRight, animationSpeed);
    }

    public void taskStatsMenu()
    {
        ShopMenu.DOAnchorPos(offsetleft, animationSpeed);
        mainMenu.DOAnchorPos(origin, animationSpeed);
        noteMenu.DOAnchorPos(offsetRight, animationSpeed);
        closeButton.DOAnchorPos(offsetDown, animationSpeed);
        analyticsMenu.DOAnchorPos(offsetDown, animationSpeed);
        TaskStatsMenu.DOAnchorPos(origin, animationSpeed);
        IdleMenu.DOAnchorPos(offsetRight, animationSpeed);
    }

    public void idleMenu()
    {
        ShopMenu.DOAnchorPos(offsetleft, animationSpeed);
        mainMenu.DOAnchorPos(origin, animationSpeed);
        noteMenu.DOAnchorPos(offsetRight, animationSpeed);
        closeButton.DOAnchorPos(offsetDown, animationSpeed);
        analyticsMenu.DOAnchorPos(offsetDown, animationSpeed);
        TaskStatsMenu.DOAnchorPos(origin, animationSpeed);
        IdleMenu.DOAnchorPos(origin, animationSpeed);
    }

    public void taskCompletedMenu()
    {
        ShopMenu.DOAnchorPos(offsetleft, animationSpeed);
        mainMenu.DOAnchorPos(origin, animationSpeed);
        noteMenu.DOAnchorPos(offsetRight, animationSpeed);
        closeButton.DOAnchorPos(offsetDown, animationSpeed);
        analyticsMenu.DOAnchorPos(offsetDown, animationSpeed);
        TaskStatsMenu.DOAnchorPos(origin, animationSpeed);
        IdleMenu.DOAnchorPos(origin, animationSpeed);
        TaskCompletedMenu.DOAnchorPos(origin, animationSpeed);
    }


    void Update()
    {
        if (noteIsOpen)
            closeButton.DOAnchorPos(origin + new Vector2(-350, 750), animationSpeed); // Sets the close buttons position to its anchor position + an offset
        else
            closeButton.DOAnchorPos(offsetDown, animationSpeed);

        if (notesList.Count > 1)
            nextNoteButton.SetActive(true);
        
        else
            nextNoteButton.SetActive(false);
    }

    public void Back()
    {
        noteMenu.DOAnchorPos(offsetRight, animationSpeed);
        mainMenu.DOAnchorPos(origin, animationSpeed);
        analyticsMenu.DOAnchorPos(offsetDown, animationSpeed);
        ShopMenu.DOAnchorPos(offsetleft, animationSpeed);
        TaskStatsMenu.DOAnchorPos(offsetRight, animationSpeed);
        IdleMenu.DOAnchorPos(offsetRight, animationSpeed);
        TaskCompletedMenu.DOAnchorPos(offsetRight, animationSpeed);
    }

    public void NewNote()
    {
        // Turn off the current note
        if (notesList.Count > 0)
        {
            notesList[currentNoteIndex].GetComponent<RectTransform>().DOAnchorPos(offsetleft, animationSpeed);
            StartCoroutine(CloseNoteCoroutine(currentNoteIndex));
        }

        // Jump to the end of the list
        currentNoteIndex = notesList.Count;
        
        // Create a new note
        GameObject newNote = Instantiate(notePrefab) as GameObject;
        notesList.Add(newNote);
        newNote.name = "New Note " + notesList.Count;
        newNote.transform.SetParent(noteMenu.transform, false);
        RectTransform newNoteRectTransform = newNote.GetComponent<RectTransform>();
        newNoteRectTransform.localPosition = offsetRight;
        newNoteRectTransform.DOAnchorPos(origin, animationSpeed);
        noteIsOpen = false;
    }

    public void CloseNote()
    {
        notesList[currentNoteIndex].GetComponent<Animator>().SetTrigger("CloseNote");
        noteIsOpen = false;
        closeButton.DOAnchorPos(offsetDown, animationSpeed);
    }

    public void NextNote()
    {
        notesList[currentNoteIndex].GetComponent<RectTransform>().DOAnchorPos(offsetleft, animationSpeed).SetEase(Ease.InOutSine);
        StartCoroutine(CloseNoteCoroutine(currentNoteIndex));
        noteIsOpen = false;

        currentNoteIndex++;

        if(currentNoteIndex >= notesList.Count)
            currentNoteIndex = 0;
        
        notesList[currentNoteIndex].SetActive(true);
        notesList[currentNoteIndex].GetComponent<RectTransform>().DOAnchorPos(origin, animationSpeed).SetEase(Ease.InOutSine);
    }

    private IEnumerator CloseNoteCoroutine(int index)
    {
        yield return new WaitForSeconds(animationSpeed);
        notesList[index].SetActive(false);
        notesList[index].GetComponent<RectTransform>().localPosition = offsetRight;

    }
}



