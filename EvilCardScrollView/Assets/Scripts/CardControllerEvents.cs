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

        InitCardValues();
    }

    private void InitCardValues()
    {
        rightBound = components.Countainer.sizeDelta.x / 2f;
        lefBound = -rightBound;

        float initX = -600f;
        int partIndex = 0;
        float distanceBetweenParts = 300f;

        foreach (var item in components.Parts)
        {
            float x = initX + partIndex * distanceBetweenParts;
            item.anchoredPosition = new Vector2(x, 0f);
            partIndex++;


        }
    }

    Vector2 touchStartPos;
    Vector2 touchCurrentPos;
    Vector2 touchEndPos;

    Vector2[] offset;

    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);

        RectTransform rect = components.RectTransform;
        Vector2 screenPoint = Input.mousePosition;
        Camera cam = CameraManager.Instance.MainCamera;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, screenPoint, cam, out Vector2 convertedPoint);

        touchStartPos = convertedPoint;
        Debug.Log($"touchStartPos {touchStartPos}");

        offset = new Vector2[components.Parts.Length];

        for (int partsIndex = 0; partsIndex < components.Parts.Length; partsIndex++)
        {
            offset[partsIndex] = touchStartPos - components.Parts[partsIndex].anchoredPosition;
        }

        mousePrev = touchStartPos;

    }

    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);

        RectTransform rect = components.RectTransform;
        Vector2 screenPoint = Input.mousePosition;
        Camera cam = CameraManager.Instance.MainCamera;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, screenPoint, cam, out Vector2 convertedPoint);

        MouseDragDirection(touchCurrentPos);

        touchCurrentPos = convertedPoint;

        for (int partsIndex = 0; partsIndex < components.Parts.Length; partsIndex++)
        {
            Vector2 pos = new Vector2(touchCurrentPos.x - offset[partsIndex].x, components.Parts[partsIndex].anchoredPosition.y);
            components.Parts[partsIndex].anchoredPosition = pos;

            if (direction == MouseDirection.LEFT && components.Parts[partsIndex].anchoredPosition.x < -900f)
            {
                float posOffset = 900f - Mathf.Abs(components.Parts[partsIndex].anchoredPosition.x);
                components.Parts[partsIndex].anchoredPosition = new Vector2(900f + posOffset, components.Parts[partsIndex].anchoredPosition.y);
                offset[partsIndex] = touchCurrentPos - components.Parts[partsIndex].anchoredPosition;
            }
            else if (direction == MouseDirection.RIGHT && components.Parts[partsIndex].anchoredPosition.x > 900f)
            {
                float posOffset = 900f - Mathf.Abs(components.Parts[partsIndex].anchoredPosition.x);

                components.Parts[partsIndex].anchoredPosition = new Vector2(-900f - posOffset, components.Parts[partsIndex].anchoredPosition.y);
                offset[partsIndex] = touchCurrentPos - components.Parts[partsIndex].anchoredPosition;
            }

            float sizeX = components.Countainer.sizeDelta.x;
            float cur_t = (pos.x + (sizeX / 2)) / sizeX;

            Image image = components.Parts[partsIndex].GetComponent<Image>();

            Color color = image.color;
            color.a = components.CardAlphaCurve.Evaluate(cur_t);
            image.color = color;

            float scalePart = components.CardScaleCurve.Evaluate(cur_t);
            components.Parts[partsIndex].localScale = new Vector3(scalePart, scalePart, scalePart);
        }
    }



    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        Debug.Log($"OnEndDrag");

        mousePrev = -Vector2.one;
    }

    Vector2 mousePrev = -Vector2.one;
    MouseDirection direction;
    private void MouseDragDirection(Vector2 mousePos)
    {
        if (mousePrev == -Vector2.one)
            mousePrev = mousePos;

        if(mousePrev.x < mousePos.x)
        {
            direction = MouseDirection.RIGHT;
            mousePrev = mousePos;
            Debug.Log("Moving RIGHT");
        }
        else
            if (mousePrev.x > mousePos.x)
        {
            direction = MouseDirection.LEFT;
            mousePrev = mousePos;

            Debug.Log("Moving LEFT");
        }
    }

    enum MouseDirection
    {
        LEFT,
        RIGHT
    }

    float lefBound;
    float rightBound;
}
