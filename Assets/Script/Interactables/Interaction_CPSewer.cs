using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction_CPSewer : Interactable
{
    public override void Interact()
    {
        CPSewer.Instance.FillMinigame(gameObject.name);
    }
}
