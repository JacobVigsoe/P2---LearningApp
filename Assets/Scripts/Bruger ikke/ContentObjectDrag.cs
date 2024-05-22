using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.UI;

public class ContentObjectDrag : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IPointerExitHandler
{
    private bool isDragging = false;
    private bool isLongPressed = false;
    private Vector2 pointerOffset;
    private float longPressDuration = 0.5f; // Adjust as needed

    private Coroutine longPressCoroutine;
    private ScrollRect scrollRect;

    private void Start()
    {
        scrollRect = GetComponentInParent<ScrollRect>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        longPressCoroutine = StartCoroutine(LongPressDetection());
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging) return;

        Vector2 localPointerPosition;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                transform.parent as RectTransform,
                eventData.position,
                eventData.pressEventCamera,
                out localPointerPosition
            ))
        {
            transform.localPosition = localPointerPosition - pointerOffset;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (longPressCoroutine != null)
        {
            StopCoroutine(longPressCoroutine);
        }

        if (isDragging)
        {
            isDragging = false;
        }
        else
        {
            // Handle short press (click)
            Debug.Log("Short press (click) detected!");
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (longPressCoroutine != null)
        {
            StopCoroutine(longPressCoroutine);
        }
    }

    private IEnumerator LongPressDetection()
    {
        yield return new WaitForSeconds(longPressDuration);

        // If the coroutine completes, it means it's a long press
        if (!isDragging)
        {
            isLongPressed = true;

            // Calculate pointer offset
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                transform.parent as RectTransform,
                Input.mousePosition,
                Camera.main,
                out pointerOffset
            );

            // Adjust the pointerOffset to be relative to the current position of the object
            pointerOffset -= (Vector2)transform.localPosition;

            isDragging = true;
        }
    }

    public void PassEventToScrollRect(PointerEventData eventData)
    {
        if (scrollRect != null)
        {
            scrollRect.OnScroll(eventData);
        }
    }
}
