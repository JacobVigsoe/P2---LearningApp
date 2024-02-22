using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TextAnim : MonoBehaviour
{
    private Button boxButton;
    private Animator boxAnim;

    [Header("Inside inputbox")]
    public GameObject closeBoxButton;
    public GameObject inputField;
    //private Text text;

    //private Color color;

    // Start is called before the first frame update
    void Start()
    {
        boxAnim = GetComponent<Animator>();
        boxButton = GetComponent<Button>();

        inputField.SetActive(false);
        closeBoxButton.SetActive(false);

        //text.text = inputField.GetComponent<InputField>().text;

        //color = Color.clear;
    }

    public void ExpandBox()
    {
        boxAnim.SetTrigger("ExpandBox");
        closeBoxButton.SetActive(true);
        boxButton.enabled = false;
        inputField.SetActive(true);







        //text.DOColor(Color.black, 2);

        inputField.transform.DOScale(1, 2).SetEase(Ease.OutSine);
    }

    public void CloseBox()
    {
        boxButton.enabled = true;
        boxAnim.SetTrigger("CloseBox");
        inputField.transform.DOScale(0, 2);

        //text.DOColor(color, 2);

        StartCoroutine(Wait());
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(2);
        closeBoxButton.SetActive(false);
        inputField.SetActive(false);


        yield break;
    }

}
