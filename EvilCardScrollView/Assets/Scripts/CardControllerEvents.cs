using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CardControllerComponents))]
public class CardControllerEvents : EventTrigger
{
    private CardControllerComponents components;

    private void Awake()
    {
        components = GetComponent<CardControllerComponents>();
    }

    Vector2 touchStartPos;
    Vector2 touchCurrentPos;
    Vector2 touchEndPos;

    Vector2 offset;

    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);

        RectTransform rect = components.RectTransform;
        Vector2 screenPoint = Input.mousePosition;
        Camera cam = CameraManager.Instance.MainCamera;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, screenPoint, cam, out Vector2 convertedPoint);

        touchStartPos = convertedPoint;
        Debug.Log($"touchStartPos {touchStartPos}");

        offset = touchStartPos - components.Parts[0].anchoredPosition;
    }

    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);

        RectTransform rect = components.RectTransform;
        Vector2 screenPoint = Input.mousePosition;
        Camera cam = CameraManager.Instance.MainCamera;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, screenPoint, cam, out Vector2 convertedPoint);

        touchCurrentPos = convertedPoint;


        components.Parts[0].anchoredPosition = new Vector2(touchCurrentPos.x - offset.x, components.Parts[0].anchoredPosition.y);

    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        Debug.Log($"OnEndDrag");
    }
}
