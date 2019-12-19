using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CardControllerComponents))]
public class CardControllerEvents : EventTrigger
{
    private CardControllerComponents components;

    private Vector2 touchStartPos;
    private Vector2 touchCurrentPos;

    private Vector2[] offset;

    float sizeX;
    float timeFromPosition;

    private Vector2 mousePrev = -Vector2.one;
    private MouseDirection direction;

    private enum MouseDirection
    {
        LEFT,
        RIGHT
    }


    private void Awake()
    {
        components = GetComponent<CardControllerComponents>();

        InitCardValues();
    }

    private void InitCardValues()
    {

        sizeX = components.Countainer.sizeDelta.x;

        float initX = -600f;
        int partIndex = 0;
        float distanceBetweenParts = 300f;

        foreach (var item in components.Parts)
        {
            float x = initX + partIndex * distanceBetweenParts;
            item.anchoredPosition = new Vector2(x, 0f);

            CalculateAlphaAndScale(item, x);
            CalculateOrderInLayer(partIndex);

            partIndex++;

        }
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
        Debug.Log($"OnBeginDrag");

        touchStartPos = ConvertMousePosition();

        offset = new Vector2[components.Parts.Length];

        UpdateOffsets(touchStartPos);

        mousePrev = touchStartPos;

    }

    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);

        touchCurrentPos = ConvertMousePosition();
        MoveCards(touchCurrentPos);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        Debug.Log($"OnEndDrag");

        mousePrev = -Vector2.one;

        SnapCards();
    }

    private void SnapCards()
    {
        Vector2 snapFrom = FindClosestToCenter();
        UpdateOffsets(snapFrom);

        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }

        coroutine = ISnapCards(Vector2.zero, snapFrom);

        StartCoroutine(coroutine);
    }

    private Vector2 FindClosestToCenter()
    {
        float closestX = 1000f;

        foreach (var item in components.Parts)
        {
            if (Mathf.Abs(closestX) > Mathf.Abs(item.anchoredPosition.x))
            {
                closestX = item.anchoredPosition.x;
            }
        }

        return  new Vector2(closestX,0f);
    }

    private void UpdateOffsets(Vector2 fromPoint)
    {
        for (int partsIndex = 0; partsIndex < components.Parts.Length; partsIndex++)
        {
            offset[partsIndex] = fromPoint - components.Parts[partsIndex].anchoredPosition;
        }
    }


    IEnumerator coroutine = null;

    private IEnumerator ISnapCards(Vector2 snapToPOs, Vector2 snapFromPos)
    {

        Debug.Log("ISnapCards");
        Vector2 toPos = snapToPOs;
        Vector2 fromPos = snapFromPos;
        float step = 0;
        float x = -1;


        while (x != toPos.x)
        {
            x = Mathf.Lerp(fromPos.x, toPos.x, step);
            step += Time.deltaTime;
            MoveCards(new Vector2(x, 0f));
            yield return null;
        }

        mousePrev = -Vector2.one;

        coroutine = null;
    }

    private void MoveCards(Vector2 newPosition, bool useOffset = true)
    {
        MouseDragDirection(newPosition);

        for (int partsIndex = 0; partsIndex < components.Parts.Length; partsIndex++)
        {
            Vector2 pos = new Vector2(newPosition.x - offset[partsIndex].x, components.Parts[partsIndex].anchoredPosition.y);

            SetCardPosition(partsIndex, pos, newPosition);
            CalculateOrderInLayer(partsIndex);
            CalculateAlphaAndScale(components.Parts[partsIndex], components.Parts[partsIndex].anchoredPosition.x);
        }
    }

    private void SetCardPosition(int partsIndex, Vector2 pos, Vector2 touchCurrentPos)
    {
        Vector2 finalPos;

        if (direction == MouseDirection.LEFT && pos.x < -900f)
        {
            float posOffset = 900f - Mathf.Abs(pos.x);

            finalPos = new Vector2(900f + posOffset, pos.y);
            offset[partsIndex] = touchCurrentPos - finalPos;
        }
        else if (direction == MouseDirection.RIGHT && pos.x > 900f)
        {
            float posOffset = 900f - Mathf.Abs(pos.x);

            finalPos = new Vector2(-900f - posOffset, pos.y);
            offset[partsIndex] = touchCurrentPos - finalPos;
        }
        else
            finalPos = pos;

        components.Parts[partsIndex].anchoredPosition = finalPos;
    }

    private void CalculateAlphaAndScale(RectTransform rectTransform, float x)
    {
        timeFromPosition = (x / sizeX) + 0.5f;
        CanvasGroup canvasGroup = rectTransform.GetComponent<CanvasGroup>();
        canvasGroup.alpha = components.CardAlphaCurve.Evaluate(timeFromPosition);

        float scalePart = components.CardScaleCurve.Evaluate(timeFromPosition);
        rectTransform.localScale = new Vector3(scalePart, scalePart, scalePart);
    }

    private void CalculateOrderInLayer(int index)
    {
        float x = components.Parts[index].anchoredPosition.x;
        int layer = Mathf.RoundToInt(x / 300f);
        int noOfNeededLayers = components.CardCanvases.Length / 2;
        components.CardCanvases[index].sortingOrder = noOfNeededLayers - Mathf.Abs(layer);
    }

    private Vector2 ConvertMousePosition()
    {
        return Input.mousePosition.PositionInCanvasCoordinates(components.RectTransform);
    }

    private void MouseDragDirection(Vector2 mousePos)
    {
        if (mousePrev == -Vector2.one)
            mousePrev = mousePos;

        if (mousePrev.x < mousePos.x)
        {
            direction = MouseDirection.RIGHT;
            mousePrev = mousePos;
            //Debug.Log("RIGHT");
        }
        else if (mousePrev.x > mousePos.x)
        {
            direction = MouseDirection.LEFT;
            mousePrev = mousePos;
        }
    }


}
