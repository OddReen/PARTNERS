using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction_Door : Interactable
{
    public override void Interact()
    {
        CPSewer.Instance.FillMinigame(gameObject.name);
    }
}
