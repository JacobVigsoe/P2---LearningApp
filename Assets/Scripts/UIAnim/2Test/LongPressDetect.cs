using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class LongPressDetect : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool isPressed = false;
    private Vector3 originalScale;
    [SerializeField] private float pressedScaleMultiplier = 0.9f;

    [SerializeField] private float animationTime = .1f;

    void Start()
    {
        originalScale = transform.localScale;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
        transform.DOScale(originalScale * pressedScaleMultiplier, animationTime);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(isPressed)
        {
            isPressed = false;
            transform.DOScale(originalScale, animationTime);
        }
    }

}
