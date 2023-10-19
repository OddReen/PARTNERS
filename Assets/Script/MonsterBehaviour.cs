using FMOD.Studio;
using FMODUnity;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MonsterBehaviour : MonoBehaviour
{
    public EventReference ClearSongEvent;
    EventInstance clearSong;
    [SerializeField] Image countdownImage;

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

    public void MusicBoxRestart()
    {
        clearSong.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        clearSong.start();
    }
    IEnumerator EnergyLoss()
    {
        while (energyCharge <= 1f)
        {
            countdownImage.fillAmount = energyCharge;
            energyCharge += Time.deltaTime / energyLossTimer;
            yield return null;
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