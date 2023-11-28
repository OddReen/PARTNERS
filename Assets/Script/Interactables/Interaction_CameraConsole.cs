using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction_CameraConsole : Interactable
{
    public override void Interact()
    {
        CPSewer.Instance.FillMinigame(gameObject.name);
    }
}
