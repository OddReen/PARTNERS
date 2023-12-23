using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction_CameraConsole : Interactable
{
    public override void Interact(PlayerController playerController)
    {
        CameraConsole.Instance.ButtonPressed(gameObject.name);
    }
}
