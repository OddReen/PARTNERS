using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraConsoleButtonPrevious : Interactable
{
    public override void Interact(PlayerController playerController)
    {
        CameraConsoleHandler.Instance.PreviousCam();
    }
}
