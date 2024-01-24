using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTaskButton : Interactable
{
    [SerializeField] CameraConsoleTask consoleTask;

    [Range(0, 5)]
    [SerializeField] int cameraIndex;

    public override void Interact(PlayerController playerController)
    {
        CameraConsoleHandler_Multiplayer.Instance.ChangeCameraServerRpc(cameraIndex);
        consoleTask.CameraButtonPressed_ServerRpc();
    }
}
