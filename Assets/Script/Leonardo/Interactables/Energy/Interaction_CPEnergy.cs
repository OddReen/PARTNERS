using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction_CPEnergy : Interactable
{
    public override void Interact(PlayerController playerController)
    {
        CPEnergy.Instance.PatternMiniGame(gameObject.name);
    }
}