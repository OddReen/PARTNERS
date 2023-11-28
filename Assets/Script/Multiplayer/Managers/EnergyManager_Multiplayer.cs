using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

public class EnergyManager_Multiplayer : NetworkBehaviour
{
    public static EnergyManager_Multiplayer Instance;

    [SerializeField] Image energyBar;

    NetworkVariable<float> energyNetwork = new NetworkVariable<float>();
    public float Energy { get {return energyNetwork.Value; } private set { energyNetwork.Value = value; } }

    [SerializeField] float MaxEnergy = 100f;

    [SerializeField] float overtimeDecay;
    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            Energy = MaxEnergy;
            StartCoroutine(DecreaseEnergy());
        }
    }
    private void Awake()
    {
        Instance = this;
    }

    IEnumerator DecreaseEnergy()
    {
        while (Energy > 0)
        {
            yield return new WaitForEndOfFrame();
            Energy -= overtimeDecay * Time.deltaTime;
            UpdateEnergyBar_ClientRpc();
        }
    }
    [ClientRpc]
    private void UpdateEnergyBar_ClientRpc()
    {
        energyBar.fillAmount = Energy / MaxEnergy;
    }
    [ServerRpc(RequireOwnership = false)]
    public void ChangeEnergy_ServerRpc(float amount)
    {   
        Energy = Mathf.Clamp(Energy + amount, 0, MaxEnergy);
        UpdateEnergyBar_ClientRpc();
    }
    public override void OnNetworkDespawn()
    {
        StopAllCoroutines();
    }
}
