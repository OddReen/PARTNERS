using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction_CPEnergy : Interactable
{
    public override void Interact()
    {
        CPEnergy.Instance.PatternMiniGame(gameObject.name);
    }
}