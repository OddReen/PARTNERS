using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD;
using FMOD.Studio;
using FMODUnity;

public class MonsterBehaviour : MonoBehaviour
{
    public EventReference ClearSongEvent;
    EventInstance clearSong;

    [SerializeField] State state;

    [SerializeField] float energyCharge = 100;
    [SerializeField] float maxEnergyCharge = 100;

    [SerializeField] float maxEnergyYellow = 65;
    [SerializeField] float maxEnergyRed = 35;

    [SerializeField] float energyLoss = 10;
    [SerializeField] float energyLossTimer = 30;

    enum State
    {
        Green,
        Yellow,
        Red
    }
    void Start()
    {
        clearSong = RuntimeManager.CreateInstance(ClearSongEvent);
        clearSong.start();
        StartCoroutine(EnergyLoss());
    }
    void Update()
    {
        clearSong.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject));
        StateUpdate();
    }
    void StateUpdate()
    {
        if (energyCharge < maxEnergyRed)
        {
            //Red
            state = State.Red;
            //Play "Clear Song"
        }
        else if (energyCharge < maxEnergyYellow)
        {
            //Yellow
            state = State.Yellow;
            //Play "Song Distortions"
        }
        else
        {
            //Green
            state = State.Green;
            //Play "Faulty Song"
        }
    }

    void MusicBoxStart()
    {
        
    }
    IEnumerator EnergyLoss()
    {
        while (true)
        {
            yield return new WaitForSeconds(energyLossTimer);
            energyCharge -= energyLoss;
        }
    }
    //Press and hold Interaction On The Music Box
    void MusicBoxInteraction()
    {

    }
    void MusicBoxCancel()
    {

    }
}