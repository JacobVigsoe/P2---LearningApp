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
    private SoundManager soundManager;
    [SerializeField] private float animationTime = .1f;

    void Start()
    {
        soundManager = FindObjectOfType<SoundManager>();
        originalScale = transform.localScale;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        soundManager.PressSound();
        isPressed = true;
        transform.DOScale(originalScale * pressedScaleMultiplier, animationTime);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(isPressed)
        {
            soundManager.ReleaseSound();
            isPressed = false;
            transform.DOScale(originalScale, animationTime);
        }
    }

}
