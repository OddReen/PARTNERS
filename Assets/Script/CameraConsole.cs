using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraConsole : MonoBehaviour
{
    [SerializeField] Camera[] cctv;

    Camera lastCamera;

    private void Start()
    {
        lastCamera = cctv[0];
    }
    public void ButtonPressed(string name)
    {
        switch (name)
        {
            case "Button1":
                Debug.Log("Yeah");
                CameraHandler(0);
                break;
            case "Button2":
                CameraHandler(1);
                break;
            case "Button3":
                CameraHandler(2);
                break;
            case "Button4":
                CameraHandler(3);
                break;
            case "Button5":
                CameraHandler(4);
                break;
            default:
                break;
        }
    }
    void CameraHandler(int cameraIndex)
    {
        lastCamera.gameObject.SetActive(false);
        lastCamera = cctv[cameraIndex];
        cctv[cameraIndex].gameObject.SetActive(true);
    }
}
