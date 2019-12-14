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

    /// <summary>
    /// Evaluate image alpha from curve in given time.
    /// </summary>
    /// <param name="image"></param>
    /// <param name="curve"></param>
    /// <param name="time"></param>
    /// <returns></returns>
    public static Color AlphaFromCurve(this Image image, AnimationCurve curve, float time)
    {
        Color color = image.color;
        color.a = curve.Evaluate(time);
        return color;
    }

    /// <summary>
    /// Returns postiion in Vector2 coordinates of mouse position in given rect transform.
    /// </summary>
    /// <param name="vector3"></param>
    /// <param name="rectTransform"></param>
    /// <returns></returns>
    public static Vector2 PositionInCanvasCoordinates(this Vector3 vector3, RectTransform rectTransform)
    {
        RectTransform rect = rectTransform;
        Vector2 screenPoint = vector3;
        Camera cam = CameraManager.Instance.MainCamera;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, screenPoint, cam, out Vector2 convertedPoint);

        return convertedPoint;
    }
}
