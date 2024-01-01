using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using FMODUnity;

[CreateAssetMenu(fileName = "TutorialVoiceLines", menuName = "Scriptable Objects/SFX/TutorialVoiceLines")]
public class TutorialVoiceLines : ScriptableObject
{
    public EventReference SmithIntroduction_Tutorial;
    public EventReference TaskPanel_Tutorial;
    public EventReference EnergyPanel_Tutorial;
    public EventReference MelodyIntroduction_Tutorial;
    public EventReference Melody_Tutorial;
    public EventReference BackToCameras_Tutorial;
    public EventReference BackLater_Tutorial;
    public EventReference BlackOut_Tutorial;
    public EventReference DayCompleted1_Tutorial;
    public EventReference DayCompleted2_Tutorial;
}
