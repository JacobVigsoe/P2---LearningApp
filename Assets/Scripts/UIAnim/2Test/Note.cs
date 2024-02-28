using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Note : MonoBehaviour
{
    private Animator anim;
    private UIManager UIManager;
    private bool isOpen;
    private Button button;


    void Start()
    {
        button = GetComponent<Button>();
        UIManager = GameObject.FindObjectOfType<UIManager>();
        anim = GetComponent<Animator>();
    }

    public void OpenNote()
    {
        UIManager.noteIsOpen = true;
        anim.SetTrigger("OpenNote");
    }

    void Update()
    {
        
    }


    
}
