using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager_Singleplayer : MonoBehaviour
{
    //DOOR Script ainda não é singleplayer do later

    public static DoorManager_Singleplayer Instance;

    [SerializeField] List<Door_Multiplayer> doorList;

    [SerializeField] Door_Multiplayer MelodyDoor;

    private void Awake()
    {
        Instance = this;
    }
    public void ControlDoor(int index)
    {
        doorList[index].InteractDoor_ServerRpc();
    }

    public void ChangeAllDoorLocks(bool locked)
    {
        foreach (Door_Multiplayer doors in doorList)
        {
            doors.ChangeLockStatus_ServerRpc(locked);
        }
    }

    public void ChangeDoorLock(int doorIndex, bool locked)
    {
        doorList[doorIndex].ChangeLockStatus_ServerRpc(locked);
    }
}
