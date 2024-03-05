using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonAnim : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool isPressed = false;
    private Vector3 originalScale;
    private Vector3 pressedScale;

    private void Start()
    {
        originalScale = transform.localScale;
        pressedScale = originalScale * 0.8f;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
        transform.DOScale(pressedScale, 0.1f);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (isPressed)
        {
            transform.DOScale(originalScale, 0.1f);
            isPressed = false;
        }
    }
}
