using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraNextTaskButton : Interactable
{
    [SerializeField] CameraConsoleTask consoleTask;

    public override void Interact(PlayerController playerController)
    {
        CameraConsoleHandler.Instance.NextCam();
        consoleTask.CameraButtonPressed();
    }
}
