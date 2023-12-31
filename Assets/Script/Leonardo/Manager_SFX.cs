using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;

public class Manager_SFX : MonoBehaviour
{
    public static Manager_SFX Instance;

    public EventReference Win;
    public EventInstance win;

    public EventReference Lose;
    public EventInstance lose;

    private void Start()
    {
        //Instance = this;
        //lose = RuntimeManager.CreateInstance(Lose);
        //win = RuntimeManager.CreateInstance(Win);
        //lose.set3DAttributes(RuntimeUtils.To3DAttributes(Camera.main.transform));
        //win.set3DAttributes(RuntimeUtils.To3DAttributes(Camera.main.transform));
    }
    void PlaySound(string soundPath)
    {
        RuntimeManager.PlayOneShot(soundPath);
    }
}
