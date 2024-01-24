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

    float energy;
    [SerializeField] Image energyLevelWarning;
    [SerializeField] Sprite energyLevelLow;
    [SerializeField] Sprite energyLevelCritical;
    [SerializeField] GameObject energyDown;

    [SerializeField] float MaxEnergy = 100f;

    [SerializeField] float overtimeDecay;

    [SerializeField] bool noEnergyLoss;

    [SerializeField] SFX_List sFX_List;
    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            energy = MaxEnergy;
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
    [ServerRpc(RequireOwnership = false)]
    public void StartEnergyDeacrease_ServerRpc()
    {
        StartCoroutine(DecreaseEnergy());
    }
    IEnumerator DecreaseEnergy()
    {
        while (energy > 0)
        {
            yield return new WaitForEndOfFrame();
            ChangeEnergy_ServerRpc(-overtimeDecay * Time.deltaTime);
        }
    }
    [ClientRpc]
    private void UpdateEnergyBar_ClientRpc(float energy)
    {
        energyBar.fillAmount = energy / MaxEnergy;
        //This probably works better com um animator but too lazy lol
        if (energy <= 0)
        {
            energyDown.SetActive(true);
            return;
        }
        if (energy > 50)
        {
            energyLevelWarning.gameObject.SetActive(false);
            energyDown.SetActive(false);
            return;
        }
        if (energy < 30)
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

    }
    /// <summary>
    /// Muda o valor da energia pela quantidade dada
    /// </summary>
    /// <param name="amount"></param>
    [ServerRpc(RequireOwnership = false)]
    public void ChangeEnergy_ServerRpc(float amount)
    {
        energy = Mathf.Clamp(energy + amount, 0, MaxEnergy);
        UpdateEnergyBar_ClientRpc(energy);
        if (energy <= 0 && !BlackoutManager_Multiplayer.Instance.inBlackout)
        {
            BlackoutManager_Multiplayer.Instance.StartBlackout_ServerRpc();
        }
    }
    [ServerRpc(RequireOwnership = false)]
    public void StartBlackout_ServerRpc()
    {
        ChangeEnergy_ServerRpc(-1000);
        SFX_Manager_Multiplayer.Instance.PlaySound_ServerRpc(sFX_List.PowerLossPath);
    }
    [ServerRpc(RequireOwnership = false)]
    public void EndBlackOut_ServerRpc()
    {
        ChangeEnergy_ServerRpc(+1000);
        SFX_Manager_Multiplayer.Instance.PlaySound_ServerRpc(sFX_List.PowerRegainedPath);
    }
    public override void OnNetworkDespawn()
    {
        BlackoutManager_Multiplayer.Instance.StartBlackout -= BlackoutManager_StartBlackout;
        BlackoutManager_Multiplayer.Instance.EndBlackout -= BlackoutManager_EndBlackout;
        StopAllCoroutines();
    }
}
