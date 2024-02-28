using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.XR;
using UnityEngine;
using UnityEngine.UI;

public class Note : MonoBehaviour
{
    private Animator anim;
    private UIManager UIManager;
    private TextMeshProUGUI noteTitle;
    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
        noteTitle = GetComponentInChildren<TextMeshProUGUI>();
        UIManager = GameObject.FindObjectOfType<UIManager>();
        anim = GetComponent<Animator>();
        
        noteTitle.text = "Note " + UIManager.notesList.Count;
    }

    public void OpenNote()
    {
        UIManager.noteIsOpen = true;
        anim.SetTrigger("OpenNote");
    }

    void Update()
    {
        if (UIManager.noteIsOpen)
            button.interactable = false;
        
        else
            button.interactable = true;
        
    }
    
}
