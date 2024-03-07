using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Note : MonoBehaviour
{
    private Animator anim;
    private UIManager UIManager;
    private TextMeshProUGUI noteTitle;
    private Button button;
    private SoundManager soundManager;

    
    void Start()
    {
        soundManager = GameObject.FindObjectOfType<SoundManager>();
        button = GetComponent<Button>();
        noteTitle = GetComponentInChildren<TextMeshProUGUI>();
        UIManager = GameObject.FindObjectOfType<UIManager>();
        anim = GetComponent<Animator>();
        
        noteTitle.text = "Note " + UIManager.notesList.Count;
    }

    public void OpenNote()
    {
        //soundManager.PressSound();
        UIManager.noteIsOpen = true;
        anim.SetTrigger("OpenNote");
    }

    void Update()
    {
        if (UIManager.noteIsOpen)
            button.enabled = false;
        else
            button.enabled = true;
        
    }
}
