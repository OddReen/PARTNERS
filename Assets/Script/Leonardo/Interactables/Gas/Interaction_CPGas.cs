using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction_CPGas : Interactable
{
    public override void Interact(PlayerController playerController)
    {
        CPGas.Instance.ColorMiniGame(gameObject.name);
    }
}
