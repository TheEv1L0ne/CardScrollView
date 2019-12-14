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

    float sizeX;
    float timeFromPosition;

    private void InitCardValues()
    {

        sizeX = components.Countainer.sizeDelta.x;


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

            //timeFromPosition = (pos.x + (sizeX / 2)) / sizeX;
            timeFromPosition = (pos.x / sizeX) +  0.5f;

            Image image = components.Parts[partsIndex].GetComponent<Image>();

            image.color = image.AlphaFromCurve(components.CardAlphaCurve, timeFromPosition);

            float scalePart = components.CardScaleCurve.Evaluate(timeFromPosition);
            components.Parts[partsIndex].localScale = new Vector3(scalePart, scalePart, scalePart);
        }
    }

    private Color ImageAlpha(Color imageColor, AnimationCurve curve, float time)
    {
        Color color = imageColor;
        color.a = curve.Evaluate(time);
        return color;
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
