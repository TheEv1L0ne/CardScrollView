using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class ExtentionMethods
{
    public static Vector3 ScreenToWorld(this Vector3 vector3)
    {
        Vector3 tmpPos;
        tmpPos = vector3;
        tmpPos.z = 10.0f;
        return CameraManager.Instance.MainCamera.ScreenToWorldPoint(tmpPos);
    }

    public static Color AlphaFromCurve(this Image image, AnimationCurve curve, float time)
    {
        Color color = image.color;
        color.a = curve.Evaluate(time);
        return color;
    }
}
