using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Note : MonoBehaviour
{
    private Animator anim;
    private UIManager UIManager;

    private TextMeshProUGUI noteTitle;

    void Start()
    {
        noteTitle = GetComponentInChildren<TextMeshProUGUI>();
        UIManager = GameObject.FindObjectOfType<UIManager>();
        anim = GetComponent<Animator>();
        
        noteTitle.text = "Note " + UIManager.notesList.Count;
    }

    public void OpenNote()
    {
        UIManager.noteIsOpen = true;
        anim.SetTrigger("OpenNote");
        // macel
    }
}
