using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CardControllerComponents))]
public class CardControllerEvents : EventTrigger
{
    private CardControllerComponents components;

    Vector2 touchStartPos;
    Vector2 touchCurrentPos;
    Vector2 touchEndPos;

    Vector2[] offset;

    float sizeX;
    float timeFromPosition;

    Vector2 mousePrev = -Vector2.one;
    MouseDirection direction;

    enum MouseDirection
    {
        LEFT,
        RIGHT
    }

    float lefBound;
    float rightBound;

    private void Awake()
    {
        components = GetComponent<CardControllerComponents>();

        InitCardValues();
    }

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


            timeFromPosition = (x / sizeX) + 0.5f;

            Image image = item.GetComponent<Image>();

            image.color = image.AlphaFromCurve(components.CardAlphaCurve, timeFromPosition);

            float scalePart = components.CardScaleCurve.Evaluate(timeFromPosition);
            item.localScale = new Vector3(scalePart, scalePart, scalePart);

            CalculateOrderInLayer(partIndex);

            partIndex++;

        }
    }

    private void CalculateOrderInLayer(int index)
    {
        float x = components.Parts[index].anchoredPosition.x;
        int layer = Mathf.RoundToInt(x / 300f);
        int noOfNeededLayers = components.CardCanvases.Length / 2;
        components.CardCanvases[index].sortingOrder = noOfNeededLayers - Mathf.Abs(layer);
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

        for (int partsIndex = 0; partsIndex < components.Parts.Length; partsIndex++)
        {
            offset[partsIndex] = touchStartPos - components.Parts[partsIndex].anchoredPosition;
        }

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

        touchEndPos = ConvertMousePosition();

        for (int partsIndex = 0; partsIndex < components.Parts.Length; partsIndex++)
        {
            offset[partsIndex] = touchEndPos - components.Parts[partsIndex].anchoredPosition;
        }

        Debug.Log($"touchEndPos {touchEndPos}");

        SnapCards();
    }

    private void SnapCards()
    {
        float touchDistanceFromNearest = touchEndPos.x % 300;

        Vector2 snapToPos;

        Debug.Log($"touchEndPos {touchEndPos}");
        Debug.Log($"offset[0].x {offset[0].x}");
        Debug.Log($"touchDistanceFromNearest {touchDistanceFromNearest}");
        //Debug.Log($"offset[0].x {offset[0].x}");

        float correction = offset[0].x % 300;
        if (direction == MouseDirection.LEFT)
            correction = - correction;

        Debug.Log($"correction {correction}");

        float baseX = 0;

            baseX = touchEndPos.x - touchDistanceFromNearest;
            Debug.Log($"baseX {baseX}");
            baseX += correction;
            Debug.Log($"baseX C {baseX}");


        snapToPos = new Vector2(baseX, 0f);

        Debug.Log($"End location {snapToPos}");


        if(coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }

        coroutine = ISnapCards(snapToPos, touchEndPos);

        StartCoroutine(coroutine);
    }


    IEnumerator coroutine = null;
    //TODO: Check if coroutine is in progress!!!!
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
            MoveCards(new Vector2(x,0f));
            yield return null;
        }

        Debug.Log(toPos.x);
        Debug.Log(x);

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

            timeFromPosition = (pos.x / sizeX) + 0.5f;

            Image image = components.Parts[partsIndex].GetComponent<Image>();

            image.color = image.AlphaFromCurve(components.CardAlphaCurve, timeFromPosition);

            float scalePart = components.CardScaleCurve.Evaluate(timeFromPosition);
            components.Parts[partsIndex].localScale = new Vector3(scalePart, scalePart, scalePart);
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
