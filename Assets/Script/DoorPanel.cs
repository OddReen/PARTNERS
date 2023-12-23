using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorPanel : Interactable
{
    [SerializeField] Door_Multiplayer door;
    public override void Interact(PlayerController playerController)
    {
        door.InteractDoor_ServerRpc();
    }
}
