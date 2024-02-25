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
    [SerializeField] private GameObject note;
    [SerializeField] private Vector2 notePosition;

    private List<GameObject> notesList = new List<GameObject>();

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
    }

    public void BackButton()
    {
        noteMenu.DOAnchorPos(offsetRight, animationSpeed);
        mainMenu.DOAnchorPos(origin, animationSpeed);
    }

    public void NewNoteButton()
    {
        Instantiate(note);
        
        RectTransform newNote = note.GetComponent<RectTransform>();

        newNote.SetParent(noteMenu, false);

        newNote.DOAnchorPos(offsetUp, animationSpeed);
        newNote.DOAnchorPos(notePosition, animationSpeed);

        
        
    }

}
