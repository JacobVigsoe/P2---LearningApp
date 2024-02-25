using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    private Animator anim;
    public GameObject closeButton;
    private GameObject newNoteButton;

    void Start()
    {
        anim = GetComponent<Animator>();
        newNoteButton = GameObject.Find("NewNoteButton");
        closeButton = GameObject.FindWithTag("CloseButton");
    }

    public void OpenNote()
    {
        newNoteButton.SetActive(false);
        anim.SetTrigger("OpenNote");
        closeButton.SetActive(true);
    }


    /// Close button vil finde reference til objektet og tænde for det
}
