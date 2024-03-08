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
    private GameObject noteTitle;
    private TMP_InputField noteTitleInput;
    private Button button;
    private GameObject inputField;
    void Start()
    {
        button = GetComponent<Button>();
        noteTitle = transform.GetChild(0).gameObject;
        UIManager = FindObjectOfType<UIManager>();
        anim = GetComponent<Animator>();
        inputField = transform.GetChild(1).gameObject;
        noteTitleInput = noteTitle.GetComponent<TMP_InputField>();
        
        noteTitleInput.text = "Note " + UIManager.notesList.Count;
        inputField.SetActive(false);
    }

    public void OpenNote()
    {
        UIManager.noteIsOpen = true;
        anim.SetTrigger("OpenNote");
    }

    void Update()
    {
        if (UIManager.noteIsOpen)
        {
            noteTitleInput.interactable = true;
            button.enabled = false;
            inputField.SetActive(true);
        }
        else
        {
            noteTitleInput.interactable = false;
            button.enabled = true;
            inputField.SetActive(false);
        } 
    }



}
