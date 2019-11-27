using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CameraExtentions
{
    public static Vector3 ScreenToWorld(this Vector3 vector3)
    {
        Vector3 tmpPos;
        tmpPos = vector3;
        tmpPos.z = 10.0f;
        return  CameraManager.Instance.MainCamera.ScreenToWorldPoint(tmpPos);
    }
}
