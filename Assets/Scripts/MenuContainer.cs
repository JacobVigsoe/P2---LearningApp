using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Menucontainer : MonoBehaviour
{
    private Animator containerAnim;
    public Animator boxAnim;

    public Button boxButton;
    public GameObject closeBoxButton;
    public GameObject inputField;
    private void Start()
    {
        containerAnim = GetComponent<Animator>();
        closeBoxButton.SetActive(false);
        inputField.SetActive(false);
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
        inputField.SetActive(true);
        inputField.transform.DOScale(1, 2).SetEase(Ease.OutSine);
    }

    public void CloseBox()
    {
        boxButton.enabled = true;
        closeBoxButton.SetActive(false);
        boxAnim.SetTrigger("CloseBox");
        inputField.transform.DOScale(0, 2);

        StartCoroutine(Wait());
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(2);

        yield break;
    }

}
