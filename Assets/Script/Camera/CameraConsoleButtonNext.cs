using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraConsoleButtonNext : Interactable
{
    public override void Interact()
    {
        CameraConsoleHandler.Instance.NextCam();
    }
}
