using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    private Animator anim;
    //private GameObject newNoteButton;
    private UIManager UIManager;

    void Start()
    {
        UIManager = GameObject.FindObjectOfType<UIManager>();
        anim = GetComponent<Animator>();
        //newNoteButton = GameObject.Find("NewNoteButton");
    }

    public void OpenNote()
    {
        UIManager.noteIsOpen = true;
        //newNoteButton.SetActive(false);
        anim.SetTrigger("OpenNote");
    }
}
