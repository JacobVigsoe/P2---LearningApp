using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    [SerializeField] private float animationSpeed;

    [Header("Menus")]
    [SerializeField] private RectTransform mainMenu;
    [SerializeField] private RectTransform noteMenu;

    [Header("Notes")]
    [SerializeField] private GameObject notePrefab;
    public List<GameObject> notesList = new List<GameObject>();
    public int currentNoteIndex = 0;

    [Header("Buttons")]
    [SerializeField] private RectTransform closeButton;
    public bool noteIsOpen = false;

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

    public void StartButton()
    {
        mainMenu.DOAnchorPos(offsetleft, animationSpeed);
        noteMenu.DOAnchorPos(origin, animationSpeed);
        closeButton.DOAnchorPos(offsetDown, animationSpeed);
    }

    void Update()
    {
        if (noteIsOpen)
            closeButton.DOAnchorPos(origin + new Vector2(-335, 180), animationSpeed); // Sets the close buttons position to its anchor position + an offset
        else
            closeButton.DOAnchorPos(offsetDown, animationSpeed);
    }

    public void BackButton()
    {
        noteMenu.DOAnchorPos(offsetRight, animationSpeed);
        mainMenu.DOAnchorPos(origin, animationSpeed);
    }

    public void NewNoteButton()
    {
        GameObject newNote = Instantiate(notePrefab) as GameObject;
        notesList.Add(newNote);
        newNote.name = "New Note " + notesList.Count;
        newNote.transform.SetParent(noteMenu.transform, false);

        RectTransform newNoteRectTransform = newNote.GetComponent<RectTransform>();

        newNoteRectTransform.DOAnchorPos(origin, animationSpeed);
    }

    public void CloseNoteButton()
    {
        notesList[currentNoteIndex].GetComponent<Animator>().SetTrigger("CloseNote");
        noteIsOpen = false;
        closeButton.DOAnchorPos(offsetDown, animationSpeed);
        Debug.Log("Close Note");
    }
}
