using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraConsoleButtonNextSingleplayer : Interactable
{
    public override void Interact(PlayerController playerController)
    {
        CameraConsole.Instance.NextCam();
    }
}
