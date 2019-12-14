using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class UIImageExtentions 
{
    public static Color AlphaFromCurve(this Image image, AnimationCurve curve, float time)
    {
        Color color = image.color;
        color.a = curve.Evaluate(time);
        return color;
    }
}
