using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class BlackoutManager_Multiplayer : NetworkBehaviour
{
    public static BlackoutManager_Multiplayer Instance;

    [SerializeField] int cpToFix;
    int cpFixedCount;
    private void Start()
    {
        Instance = this;
    }

    [ServerRpc(RequireOwnership =false)]
    public void StartBlackout_ServerRpc()
    {
        EnergyManager_Multiplayer.Instance.StartBlackout_ServerRpc();
        CPGas_Multiplayer.Instance.Break();
        //Start Halucinations
        //Start Monsters Escaping
    }

    [ServerRpc(RequireOwnership =false)]
    public void EndBlackOut_ServerRpc()
    {
        cpFixedCount++;
        if (cpFixedCount>=cpToFix)
        {
            EnergyManager_Multiplayer.Instance.EndBlackOut_ServerRpc();
            //Stop Halucinations
            //Stop Monsters
        }
    }
}
