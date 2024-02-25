using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TextAnim : MonoBehaviour
{
    private Button boxButton;
    private Animator boxAnim;

    [Header("Box parameters")]
    public GameObject closeBoxButton;
    public GameObject inputField;
    public float animationTime = 1;

    void Start()
    {
        boxAnim = GetComponent<Animator>();
        boxButton = GetComponent<Button>();

        inputField.SetActive(false);
        closeBoxButton.SetActive(false);

    }

    public void ExpandBox()
    {
        boxAnim.SetTrigger("ExpandBox");
        closeBoxButton.SetActive(true);
        boxButton.enabled = false;
        inputField.SetActive(true);

        SpindleHandler.openBox = true;

        inputField.transform.DOScale(1, animationTime).SetEase(Ease.OutSine);
    }

    public void CloseBox()
    {
        boxAnim.SetTrigger("CloseBox");
        inputField.transform.DOScale(0, animationTime);
        StartCoroutine(WaitForAnimation());
    }

    private IEnumerator WaitForAnimation()
    {
        yield return new WaitForSeconds(animationTime);
        boxButton.enabled = true;
        closeBoxButton.SetActive(false);
        inputField.SetActive(false);
        SpindleHandler.openBox = false;


        yield break;
    }

}
