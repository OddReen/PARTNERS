using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class DoorManager_Multiplayer : NetworkBehaviour
{
    public static DoorManager_Multiplayer Instance;

    [SerializeField] List<Door_Multiplayer> doorList;

    [SerializeField] Door_Multiplayer MelodyDoor;
   
    private void Awake()
    {
        Instance = this;
    } 
    [ServerRpc(RequireOwnership = false)]
    public void ControlDoor_ServerRpc(int index)
    {
        doorList[index].InteractDoor_ServerRpc();
    }
    [ServerRpc(RequireOwnership = false)]
    public void ChangeAllDoorLocks_ServerRpc(bool locked)
    {
        foreach (Door_Multiplayer doors in doorList)
        {
            doors.ChangeLockStatus_ServerRpc(locked);
        }
    }
    [ServerRpc(RequireOwnership = false)]
    public void ChangeDoorLock_ServerRpc(int doorIndex, bool locked)
    {
        doorList[doorIndex].ChangeLockStatus_ServerRpc(locked);
    }
}
