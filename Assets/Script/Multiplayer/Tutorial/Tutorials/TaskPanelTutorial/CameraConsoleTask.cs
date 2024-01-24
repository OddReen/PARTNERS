using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CameraConsoleTask : TutorialTask_Multiplayer
{
    [ServerRpc(RequireOwnership = false)]
    public void CameraButtonPressed_ServerRpc()
    {
        if (isTaskActive)
        {
            CompleteTask_ServerRpc();
        }
    }
    [ServerRpc]
    public void CompleteTask_ServerRpc()
    {
        CompleteTask();
    }
}
