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

    [SerializeField] Light light;
    [SerializeField] Color greenLight;
    [SerializeField] Color yellowLight;
    [SerializeField] Color redLight;

    [SerializeField] State state;

    [SerializeField] float energyCharge = 30;
    float EnergyCharge
    {
        get { return energyCharge; }
        set { energyCharge = Mathf.Clamp(value, 0, maxEnergyCharge); }
    }
    [SerializeField] float maxEnergyCharge = 30;
    [SerializeField] float maxEnergyYellow = 20;
    [SerializeField] float maxEnergyRed = 10;
    
    enum State
    {
        Green,
        Yellow,
        Red
    }
    void Start()
    {
        energyCharge = maxEnergyCharge;
        clearSong = RuntimeManager.CreateInstance(ClearSongEvent);
        clearSong.start();
        StartCoroutine(EnergyLoss());
    }
    void Update()
    {
        clearSong.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject));
        StateUpdate();
        ColorUpdate();
    }
    void ColorUpdate()
    {
        var gradient = new Gradient();

        var colors = new GradientColorKey[3];
        colors[0] = new GradientColorKey(greenLight, 1f);
        colors[1] = new GradientColorKey(yellowLight, .5f);
        colors[2] = new GradientColorKey(redLight, 0f);

        var alphas = new GradientAlphaKey[1];
        alphas[0] = new GradientAlphaKey(1.0f, 0.0f);

        gradient.SetKeys(colors, alphas);

        light.color = (gradient.Evaluate(energyCharge / maxEnergyCharge));
    }
    void StateUpdate()
    {
        if (energyCharge < maxEnergyRed)
        {
            //Red
            state = State.Red;
        }
        else if (energyCharge < maxEnergyYellow)
        {
            //Yellow
            state = State.Yellow;
        }
        else
        {
            //Green
            state = State.Green;
        }
    }
    public void MusicBoxRestart()
    {
        energyCharge = maxEnergyCharge;
        clearSong.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        clearSong.start();
    }
    IEnumerator EnergyLoss()
    {
        while (true)
        {
            countdownImage.fillAmount = EnergyCharge / maxEnergyCharge;
            EnergyCharge -= Time.deltaTime;
            yield return null;
        }
    }
}