using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CardControllerEvents))]
public class CardControllerComponents : MonoBehaviour
{
    [SerializeField]
    private RectTransform rectTransform = null;
    public RectTransform RectTransform => rectTransform;

    [SerializeField]
    private RectTransform[] parts = null;
    public RectTransform[] Parts => parts;

    [Header("Curves")]
    [SerializeField]
    private AnimationCurve cardAplphaCurve = null;
    public AnimationCurve CardAlphaCurve => cardAplphaCurve;

    [SerializeField]
    private AnimationCurve cardScaleCurve = null;
    public AnimationCurve CardScaleCurve => cardScaleCurve;

    [SerializeField]
    private RectTransform container =  null;
    public RectTransform Countainer => container;

    public Canvas[] CardCanvases => cardCanvases;
    //[SerializeField]
    //private Canvas[] cardsCanvases = null;
    //public Canvas[] CardsCanvases => cardsCanvases;

    [SerializeField]
    private Canvas[] cardCanvases = null;

}
