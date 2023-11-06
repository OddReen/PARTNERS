using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class BetterDoor : NetworkBehaviour
{
    [SerializeField] bool isOpened = false;

    [SerializeField] GameObject door;

    [ServerRpc(RequireOwnership = false)]
    public void InteractDoorServerRpc()
    {
        InteractDoorClientRpc();
    }
    [ClientRpc]
    private void InteractDoorClientRpc()
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
}
