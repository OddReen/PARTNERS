using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraConsoleButtonPreviousSinglePlayer : Interactable
{
    public override void Interact(PlayerController playerController)
    {
        CameraConsole.Instance.PreviousCam();
    }
}
