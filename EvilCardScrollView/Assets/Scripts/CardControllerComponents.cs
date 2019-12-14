using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    private AnimationCurve cardAplphaCurve;
    public AnimationCurve CardAlphaCurve => cardAplphaCurve;

    [SerializeField]
    private AnimationCurve cardScaleCurve;
    public AnimationCurve CardScaleCurve => cardScaleCurve;

    [SerializeField]
    private RectTransform container =  null;
    public RectTransform Countainer => container;

    public Canvas[] CardCanvases => cardCanvases;
    //[SerializeField]
    //private Canvas[] cardsCanvases = null;
    //public Canvas[] CardsCanvases => cardsCanvases;

    [SerializeField]
    private Canvas[] cardCanvases;

    // Start is called before the first frame update
    void Start()
    {

    }
}
