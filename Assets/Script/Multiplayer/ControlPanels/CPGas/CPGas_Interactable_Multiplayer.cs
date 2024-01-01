using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPGas_Interactable_Multiplayer : Interactable
{
    public override void Interact(PlayerController playerController)
    {
        CPGas_Multiplayer.Instance.ColorMiniGame_ServerRpc(name);
    }
}
