using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using FMODUnity;

public class EnergyManager_Multiplayer : NetworkBehaviour
{
    public static EnergyManager_Multiplayer Instance;

    [SerializeField] Image energyBar;

    NetworkVariable<float> energyNetwork = new NetworkVariable<float>();

    [SerializeField] Image energyLevelWarning;
    [SerializeField] Sprite energyLevelLow;
    [SerializeField] Sprite energyLevelCritical;
    [SerializeField] GameObject energyDown;

    [SerializeField] float MaxEnergy = 100f;

    [SerializeField] float overtimeDecay;

    [SerializeField] bool noEnergyLoss;

    [SerializeField] EventReference powerDown;
    [SerializeField] EventReference powerRestored;
    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            energyNetwork.Value = MaxEnergy;
            if (!noEnergyLoss)
            {
                StartCoroutine(DecreaseEnergy());
            }
            BlackoutManager_Multiplayer.Instance.StartBlackout += BlackoutManager_StartBlackout;
            BlackoutManager_Multiplayer.Instance.EndBlackout += BlackoutManager_EndBlackout;
        }
    }

    private void BlackoutManager_EndBlackout(object sender, EventArgs e)
    {
        EndBlackOut_ServerRpc();
    }

    private void BlackoutManager_StartBlackout(object sender, EventArgs e)
    {
        StartBlackout_ServerRpc();
    }


    private void Awake()
    {
        energyLevelWarning.gameObject.SetActive(false);
        energyDown.SetActive(false);
        Instance = this;
    }
    [ServerRpc(RequireOwnership =false)]
    public void StartEnergyDeacrease_ServerRpc()
    {
        StartCoroutine(DecreaseEnergy());
    }
    IEnumerator DecreaseEnergy()
    {
        while (energyNetwork.Value > 0)
        {
            yield return new WaitForEndOfFrame();
            energyNetwork.Value -= overtimeDecay * Time.deltaTime;
            UpdateEnergyBar_ClientRpc();
        }
    }
    [ClientRpc]
    private void UpdateEnergyBar_ClientRpc()
    {
        energyBar.fillAmount = energyNetwork.Value / MaxEnergy;
        //This probably works better com um animator but too lazy lol
        if (energyNetwork.Value <= 0)
        {
            energyDown.SetActive(true);
            return;
        }
        if (energyNetwork.Value > 50)
        {
            energyLevelWarning.gameObject.SetActive(false);
            energyDown.SetActive(false);
            return;
        }
        if (energyNetwork.Value < 30)
        {
            energyLevelWarning.gameObject.SetActive(true);
            energyLevelWarning.sprite = energyLevelCritical;
            energyDown.SetActive(false);
            return;
        }
        //Energy at 50
        energyLevelWarning.gameObject.SetActive(true);
        energyLevelWarning.sprite = energyLevelLow;
        energyDown.SetActive(false);
        Debug.Log(energyDown.activeInHierarchy);
    }
    [ServerRpc(RequireOwnership = false)]
    public void ChangeEnergy_ServerRpc(float amount)
    {
        energyNetwork.Value = Mathf.Clamp(energyNetwork.Value + amount, 0, MaxEnergy);
        UpdateEnergyBar_ClientRpc();
        if (energyNetwork.Value == 0 && !BlackoutManager_Multiplayer.Instance.inBlackout)
        {
          BlackoutManager_Multiplayer.Instance.StartBlackout_ServerRpc();
        }
    }
    [ServerRpc(RequireOwnership =false)]
    public void StartBlackout_ServerRpc()
    {
        ChangeEnergy_ServerRpc(-1000);
        SFX_Manager_Multiplayer.Instance.PlaySound_ServerRpc(powerDown.Path);
    }
    [ServerRpc(RequireOwnership =false)]
    public void EndBlackOut_ServerRpc()
    {
        ChangeEnergy_ServerRpc(+1000);
        SFX_Manager_Multiplayer.Instance.PlaySound_ServerRpc(powerRestored.Path);
    }
    public override void OnNetworkDespawn()
    {
        BlackoutManager_Multiplayer.Instance.StartBlackout -= BlackoutManager_StartBlackout;
        BlackoutManager_Multiplayer.Instance.EndBlackout -= BlackoutManager_EndBlackout;
        StopAllCoroutines();
    }
}
