using FMOD.Studio;
using FMODUnity;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MonsterBehaviour : MonoBehaviour
{
    LineRenderer lineRenderer;
    public EventReference ClearSongEvent;
    EventInstance clearSong;
    [SerializeField] Image countdownImage;

    Vector3 defaultPosition;
    Quaternion defaultRotation;
    [SerializeField] Transform grabPos;
    [SerializeField] Light light;
    [SerializeField] Gradient gradient;

    [SerializeField] State state;

    [SerializeField] float pullDistance = 3;
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
        defaultPosition = transform.position;
        defaultRotation = transform.rotation;
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
    private void LateUpdate()
    {
        DrawLine();
    }
    void ColorUpdate()
    {
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
    void DrawLine()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, defaultPosition);
        lineRenderer.SetPosition(1, transform.position);
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
    public void ExecuteAction(PlayerInput playerInput)
    {
        StartCoroutine(CordPull(playerInput));
    }
    IEnumerator CordPull(PlayerInput playerInput)
    {
        while (Vector3.Distance(defaultPosition, transform.position) < pullDistance && playerInput.isInteracting)
        {
            transform.position = grabPos.position;
            transform.LookAt(defaultPosition);
            yield return null;
        }
        if (!playerInput.isInteracting)
        {
            transform.position = defaultPosition;
            transform.rotation = defaultRotation;
        }
        else
        {
            transform.position = defaultPosition;
            transform.rotation = defaultRotation;
            energyCharge = maxEnergyCharge;
            clearSong.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            clearSong.start();
        }
    }
}