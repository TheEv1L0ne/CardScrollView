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
    private RectTransform container;
    public RectTransform Countainer => container;


    // Start is called before the first frame update
    void Start()
    {
        float sizeX = Countainer.sizeDelta.x; //=> translate tis to 1 sec

        // max / cur = 1 / cur_t

        float cur_t = 960f / sizeX;

        Debug.Log(CardAlphaCurve.Evaluate(cur_t));
    }
}
