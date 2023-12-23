using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction_Door : Interactable
{
    public override void Interact(PlayerController playerController)
    {
        CPSewer.Instance.FillMinigame(gameObject.name);
    }
}
