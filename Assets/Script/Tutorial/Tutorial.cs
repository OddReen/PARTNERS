using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Tutorial : NetworkBehaviour
{
    public static Tutorial Instance;
    private void Awake()
    {
        Instance = this;
    }
    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            //Put network start here if needed
        }
    }
    public void NextTutorial()
    {

    }
    //Need to be in start because of execution order
    private void Start()
    {
        DoorManager_Multiplayer.Instance.ChangeAllDoorLocks_ServerRpc(true);
    }
}
