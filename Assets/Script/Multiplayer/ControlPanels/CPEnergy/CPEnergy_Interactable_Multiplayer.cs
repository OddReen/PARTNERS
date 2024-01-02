using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPEnergy_Interactable_Multiplayer : Interactable
{
    public override void Interact(PlayerController playerController)
    {
        CPEnergy_Multiplayer.Instance.PatternMiniGame_ServerRpc(gameObject.name);
    }
}
