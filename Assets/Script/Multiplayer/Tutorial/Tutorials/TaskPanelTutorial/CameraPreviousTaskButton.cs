using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPreviousTaskButton : Interactable
{

    [SerializeField] CameraConsoleTask consoleTask;

    public override void Interact(PlayerController playerController)
    {
        CameraConsoleHandler.Instance.PreviousCam();
        consoleTask.CameraButtonPressed();
    }
}
