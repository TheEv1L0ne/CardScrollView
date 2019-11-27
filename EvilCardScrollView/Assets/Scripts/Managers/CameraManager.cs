using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{

    [SerializeField]
    private Camera mainCamera = null;

    public Camera MainCamera {
        get
        {
            if (mainCamera == null)
            {
                //Finds main camera if it is not referenced in scene
                //Camera.main under the hood uses GameObject.Find which is to slow for big projects
                mainCamera = Camera.main;
            }

            return mainCamera;
        }
    }
}
