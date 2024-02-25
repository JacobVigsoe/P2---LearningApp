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
    [SerializeField] private Vector2 notePosition;
    [SerializeField] private GameObject closeButton;

    public List<GameObject> notesList = new List<GameObject>();

    public int currentNoteIndex = 0;

    private Vector2 origin;

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
        closeButton.SetActive(false);
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
        newNote.transform.parent = noteMenu.transform;

        RectTransform newNoteRectTransform = newNote.GetComponent<RectTransform>();

        newNoteRectTransform.DOAnchorPos(origin, animationSpeed);
    }

    public void CloseNoteButton()
    {
        notesList[currentNoteIndex].GetComponent<Animator>().SetTrigger("CloseNote");
        Debug.Log("Close Note");
    }

    /// Note index virker ikke ift at lukke noter

}
