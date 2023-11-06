using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraConsoleButtonPrevious : Interactable
{
    public override void Interact()
    {
        CameraConsoleHandler.Instance.PreviousCam();
    }
}
