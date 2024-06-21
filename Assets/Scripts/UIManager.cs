using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    [SerializeField] private float animationSpeed;
    public bool noteIsOpen = false;
    private float resolutionWidth;

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
    [SerializeField] private GameObject confirmButton;
    [SerializeField] private GameObject endTaskButton;
    [SerializeField] private GameObject confirmText;
    [SerializeField] private Button backGroundButton;

    // Offsets and positions
    private Vector2 origin = new Vector2(0, 0);
    private Vector2 offsetleft = new Vector2(-1500, 0);
    private Vector2 offsetUp = new Vector2(0, 3040);
    private Vector2 offsetRight = new Vector2(1500, 0);
    private Vector2 offsetDown = new Vector2(0, -3040);

    void Start()
    {
        backGroundButton.interactable = false;

        mainMenu.DOAnchorPos(origin, animationSpeed);
        Back();
    }

    public void setTimeMenu ()
    {
        mainMenu.DOAnchorPos(origin, animationSpeed);
        noteMenu.DOAnchorPos(origin, animationSpeed);
        //closeButton.DOAnchorPos(offsetDown, animationSpeed);
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
        //closeButton.DOAnchorPos(offsetDown, animationSpeed);
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
        //closeButton.DOAnchorPos(offsetDown, animationSpeed);
        analyticsMenu.DOAnchorPos(offsetDown, animationSpeed);
        TaskStatsMenu.DOAnchorPos(origin, animationSpeed);
        IdleMenu.DOAnchorPos(offsetRight, animationSpeed);
    }

    public void idleMenu()
    {
        ShopMenu.DOAnchorPos(offsetleft, animationSpeed);
        mainMenu.DOAnchorPos(origin, animationSpeed);
        noteMenu.DOAnchorPos(offsetRight, animationSpeed);
        //closeButton.DOAnchorPos(offsetDown, animationSpeed);
        analyticsMenu.DOAnchorPos(offsetDown, animationSpeed);
        TaskStatsMenu.DOAnchorPos(origin, animationSpeed);
        IdleMenu.DOAnchorPos(origin, animationSpeed);
    }

    public void taskCompletedMenu()
    {
        ShopMenu.DOAnchorPos(offsetleft, animationSpeed);
        mainMenu.DOAnchorPos(origin, animationSpeed);
        noteMenu.DOAnchorPos(offsetRight, animationSpeed);
        //closeButton.DOAnchorPos(offsetDown, animationSpeed);
        analyticsMenu.DOAnchorPos(offsetDown, animationSpeed);
        TaskStatsMenu.DOAnchorPos(origin, animationSpeed);
        IdleMenu.DOAnchorPos(origin, animationSpeed);
        TaskCompletedMenu.DOAnchorPos(origin, animationSpeed);

        confirmText.SetActive(false);
        confirmButton.SetActive(false);
        endTaskButton.SetActive(true);
        backGroundButton.interactable = false;
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

    public void ConfirmButton()
    {
        backGroundButton.interactable = true;
        confirmText.SetActive(true);
        confirmButton.SetActive(true);
        endTaskButton.SetActive(false);
    }
    public void UnConfirmButton()
    {
        confirmText.SetActive(false);
        confirmButton.SetActive(false);
        endTaskButton.SetActive(true);
        backGroundButton.interactable = false;
    }   
}



