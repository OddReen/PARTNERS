using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class MelodyMonster : NetworkBehaviour
{
    public EventReference ClearSongEvent;
    EventInstance clearSong;
    [SerializeField] Image countdownImage;

    [SerializeField] Light statelight;
    [SerializeField] Gradient gradient;

    [SerializeField] State state;

    NetworkVariable<float> energyCharge =  new NetworkVariable<float>();

    [SerializeField] float energyGain;

    float EnergyCharge
    {
        get { return energyCharge.Value; }
        set { energyCharge.Value = Mathf.Clamp(value, 0, maxEnergyCharge); }
    }

    [SerializeField] float maxEnergyCharge = 30;
    //[SerializeField] float maxEnergyYellow = 20;
    //[SerializeField] float maxEnergyRed = 10;

    enum State
    {
        Green,
        Yellow,
        Red
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            energyCharge.Value = maxEnergyCharge;
            StartCoroutine(EnergyLoss());
        }
    }
    void Start()
    {
        //How to do music in multiplayer?
        //clearSong = RuntimeManager.CreateInstance(ClearSongEvent);
        //clearSong.start();
    }
    void Update()
    {
        //clearSong.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject));
        ColorUpdateClientRpc();
        //StateUpdate();

    }

    //Atualiza a cor da melody em todos os clients
    [ClientRpc]
    void ColorUpdateClientRpc()
    {
        statelight.color = (gradient.Evaluate(energyCharge.Value / maxEnergyCharge));
    }

    //void StateUpdate()
    //{
    //    if (energyCharge < maxEnergyRed)
    //    {
    //        //Red
    //        StopAllCoroutines();
    //        state = State.Red;
    //    }
    //    else if (energyCharge < maxEnergyYellow)
    //    {
    //        //Yellow
    //        state = State.Yellow;
    //    }
    //    else
    //    {
    //        //Green
    //        state = State.Green;
    //    }
    //}

    IEnumerator EnergyLoss()
    {
        //Ask leo if i can reduce the number of times this is executed? InvokeRepeating probably better or even put it in update?? Maybe
        while (true)
        {
            //countdownImage.fillAmount = EnergyCharge / maxEnergyCharge;
            EnergyCharge -= Time.deltaTime;
            yield return null;
        }
    }

    //Find better name
    [ServerRpc(RequireOwnership = false)]
    public void GainEnergy_ServerRpc()
    {
        EnergyManager_Multiplayer.Instance.ChangeEnergy_ServerRpc(energyGain);

        energyCharge.Value = maxEnergyCharge;

        //How to fmod multiplayer?
        //clearSong.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        //clearSong.start();
    }
}
