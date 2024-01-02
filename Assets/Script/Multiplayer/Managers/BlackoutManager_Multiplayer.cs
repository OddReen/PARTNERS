using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;

public class BlackoutManager_Multiplayer : NetworkBehaviour
{
    public static BlackoutManager_Multiplayer Instance;

    [SerializeField] int cpToFix;
    int cpFixedCount;

    [SerializeField] bool onlyHalucinations;

    [SerializeField] SFX_List sFX;

   [HideInInspector]public bool inBlackout;

    public event EventHandler StartBlackout;
    public event EventHandler EndBlackout;
    private void Awake()
    {
        Instance = this;
    }

    [ServerRpc(RequireOwnership =false)]
    public void StartBlackout_ServerRpc()
    {
        inBlackout = true;
        SFX_Manager_Multiplayer.Instance.PlaySound_ServerRpc(sFX.PowerLoss.Path);
        StartBlackout(this,EventArgs.Empty);
        CPGas_Multiplayer.Instance.Break();
        if (onlyHalucinations)
        {
            StartHalucinations_ClientRpc();
            return;
        }
        StartHalucinations_ClientRpc();
        //Start Monsters Escaping
    }
    [ClientRpc]
    private void StartHalucinations_ClientRpc()
    {
        HalucinationManager_Temp.Instance.ActivateHalucination();
    }
    [ClientRpc]
    private void StopHalucinations_ClientRpc()
    {
        HalucinationManager_Temp.Instance.StopHalucination();
    }
    [ServerRpc(RequireOwnership =false)]
    public void EndBlackOut_ServerRpc()
    {
        cpFixedCount++;
        if (cpFixedCount>=cpToFix)
        {
            inBlackout = false;
            EndBlackout(this,EventArgs.Empty);
            if (onlyHalucinations)
            {
                StopHalucinations_ClientRpc();
                return;
            }
            StopHalucinations_ClientRpc();
            //Stop Monsters
        }
    }
}
