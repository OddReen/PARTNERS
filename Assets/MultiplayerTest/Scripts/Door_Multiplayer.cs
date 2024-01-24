using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Door_Multiplayer : NetworkBehaviour
{
    [SerializeField] bool isOpened = false;

    bool isLocked = false;

    [SerializeField] GameObject door;

    [ServerRpc(RequireOwnership = false)]
    public void InteractDoor_ServerRpc()
    {
        if (isLocked)
        {
            return;
        }
        InteractDoor_ClientRpc();
    }
    [ServerRpc(RequireOwnership =false)]
    public void CloseDoor_ServerRpc()
    {
        CloseDoor_ClientRpc();
    }
    [ClientRpc]
    private void CloseDoor_ClientRpc()
    {
        isOpened = false;
        door.SetActive(true);
    }
    [ClientRpc]
    private void InteractDoor_ClientRpc()
    {
        isOpened = !isOpened;
        if (isOpened)
        {
            door.SetActive(false);
        }
        else
        {
            door.SetActive(true);
        }

    }
    [ServerRpc]
    public void ChangeLockStatus_ServerRpc(bool isLocked)
    {
        ChangeLockStatus_ClientRpc(isLocked);
    }
    [ClientRpc]
    private void ChangeLockStatus_ClientRpc(bool isLocked)
    {
        this.isLocked = isLocked;
    }
}
