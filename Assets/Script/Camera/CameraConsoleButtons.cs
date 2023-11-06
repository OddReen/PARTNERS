using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraConsoleButtons : Interactable
{
    [Range(0, 5)]
    [SerializeField] int cameraIndex;

    public override void Interact()
    {
        CameraConsoleHandler.Instance.ChangeCameraServerRpc(cameraIndex);
    }
}
