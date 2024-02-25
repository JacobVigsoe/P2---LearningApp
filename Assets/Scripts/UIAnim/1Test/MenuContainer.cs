using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Menucontainer : MonoBehaviour
{
    private Animator containerAnim;

    private void Start()
    {
        containerAnim = GetComponent<Animator>();
    }

    public void StartClick()
    {
        containerAnim.SetTrigger("NextMenu");
    }

}
