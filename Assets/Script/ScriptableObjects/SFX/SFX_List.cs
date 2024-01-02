using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

[CreateAssetMenu(fileName = "SfX_List", menuName = "Scriptable Objects/SFX/Sfx_List")]
public class SFX_List : ScriptableObject
{
    public EventReference CameraButtonPress;
    public EventReference CameraChange;
    public EventReference CPGasCorrect;
    public EventReference CPGasWrong;
    public EventReference PowerLoss;
    public EventReference PickupPaper;
}
