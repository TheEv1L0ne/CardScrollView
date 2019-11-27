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
            Camera[] camera = new Camera[1];
            //Main camera is not referenced is scene
            if (mainCamera == null)
            {
                //Finds main camera if it is not referenced in scene
                //Camera.main under the hood uses GameObject.Find which is to slow for big projects
                camera[0] = Camera.main;
                if (camera[0] == null)
                {
                    Debug.LogWarning($"There is no camera taged with Main camera in scene...");
                    Debug.Log($"Trying to find other camera...");
                    camera = GameObject.FindObjectsOfType<Camera>();
                    if (camera.Length == 0)
                    {
                        Debug.LogError($"There are no cameras in scene! Please add main camera!");
                    }
                    else
                    {
                        if(camera.Length > 1)
                            Debug.Log($"Multiple cameras found.");
                        Debug.Log($"Asigning {camera[0].name} to main camera");
                        mainCamera = camera[0];
                    }
                }
                else
                    mainCamera = Camera.main;
            }

            return mainCamera;
        }
    }
}
