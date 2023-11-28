using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraView : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera firstPersonCamera, thirdPersonCamera;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            firstPersonCamera.gameObject.SetActive(!firstPersonCamera.gameObject.activeSelf);
            thirdPersonCamera.gameObject.SetActive(!firstPersonCamera.gameObject.activeSelf);
        }
    }
}
