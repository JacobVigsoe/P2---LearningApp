using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnclickButton : MonoBehaviour
{
    private Animator containerAnim;
    public Animator boxAnim;

    public Button boxButton;
    public GameObject closeBoxButton;

    private void Start()
    {
        containerAnim = GetComponent<Animator>();
        closeBoxButton.SetActive(false);
    }

    public void StartClick()
    {
        Debug.Log("click");

        containerAnim.SetTrigger("NextMenu");
    }

    public void ExpandBox()
    {
        boxAnim.SetTrigger("ExpandBox");
        closeBoxButton.SetActive(true);
        boxButton.enabled = false;
    }

    public void CloseBox()
    {
        boxButton.enabled = true;
        closeBoxButton.SetActive(false);
        boxAnim.SetTrigger("CloseBox");
    }
}
