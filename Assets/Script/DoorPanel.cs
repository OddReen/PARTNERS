using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorPanel : Interactable
{
    [SerializeField] BetterDoor door;
    public override void Interact()
    {
        door.InteractDoorServerRpc();
    }
}
