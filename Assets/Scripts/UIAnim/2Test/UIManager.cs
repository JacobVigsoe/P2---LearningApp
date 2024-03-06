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
    }

    public void StartButton()
    {
        mainMenu.DOAnchorPos(offsetleft, animationSpeed);
        noteMenu.DOAnchorPos(origin, animationSpeed);
        closeButton.DOAnchorPos(offsetDown, animationSpeed);
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
    }

    public void NewNote()
    {
        // Turn off the current note
        if (notesList.Count > 0)
        {
            notesList[currentNoteIndex].SetActive(false);
        }

        // Jump to the end of the list
        currentNoteIndex = notesList.Count;



        // Create a new note
        GameObject newNote = Instantiate(notePrefab) as GameObject;
        notesList.Add(newNote);
        newNote.name = "New Note " + notesList.Count;
        newNote.transform.SetParent(noteMenu.transform, false);
        RectTransform newNoteRectTransform = newNote.GetComponent<RectTransform>();
        newNoteRectTransform.DOScale(0.8f, animationSpeed);
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
        notesList[currentNoteIndex].SetActive(false);
        noteIsOpen = false;

        currentNoteIndex++;

        if(currentNoteIndex >= notesList.Count)
            currentNoteIndex = 0;
        
        notesList[currentNoteIndex].SetActive(true);
    }
}



