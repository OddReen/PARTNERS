using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;



[CreateAssetMenu(fileName = "SfX_List", menuName = "Scriptable Objects/SFX/Sfx_List")]
public class SFX_List : ScriptableObject
{
    //Cant use this eventReference.Path so funciona no editor por alguma razão de deus
    //se eu encontrar quem escreveu os docs do Fmod someones gonna die
    [Header("Sound Effects")]
    [SerializeField] EventReference CameraButtonPress;
    [SerializeField] EventReference CameraChange;
    [SerializeField] EventReference ControlPanelClick;
    [SerializeField] EventReference ControlPanelCorrect;
    [SerializeField] EventReference ControlPanelWrong;
    [SerializeField] EventReference PowerLoss;
    [SerializeField] EventReference PowerRegained;
    [SerializeField] EventReference PickupPaper;
    [Header("Voice Lines")]
    [SerializeField] EventReference SmithIntroduction_Tutorial;
    [SerializeField] EventReference TaskPanel_Tutorial;
    [SerializeField] EventReference EnergyPanel_Tutorial;
    [SerializeField] EventReference MelodyIntroduction_Tutorial;
    [SerializeField] EventReference Melody_Tutorial;
    [SerializeField] EventReference BackToCameras_Tutorial;
    [SerializeField] EventReference BackLater_Tutorial;
    [SerializeField] EventReference BlackOut_Tutorial;
    [SerializeField] EventReference DayCompleted1_Tutorial;
    [SerializeField] EventReference DayCompleted2_Tutorial;

    [Header("Strings")]
    public string CameraButtonPressPath;
    public string CameraChangePath;
    public string ControlPanelClickPath;
    public string ControlPanelCorrectPath;
    public string ControlPanelWrongPath;
    public string PowerLossPath;
    public string PowerRegainedPath;
    public string PickupPaperPath;
    public string SmithIntroduction_TutorialPath;
    public string TaskPanel_TutorialPath;
    public string EnergyPanel_TutorialPath;
    public string MelodyIntroduction_TutorialPath;
    public string Melody_TutorialPath;
    public string BackToCameras_TutorialPath;
    public string BackLater_TutorialPath;
    public string BlackOut_TutorialPath;
    public string DayCompleted1_TutorialPath;
    public string DayCompleted2_TutorialPath;
#if UNITY_EDITOR
    public void ConvertToString()
    {
        EditorUtility.SetDirty(this);

        CameraButtonPressPath = CameraButtonPress.Path;
        CameraChangePath = CameraChange.Path;
        ControlPanelClickPath = ControlPanelClick.Path;
        ControlPanelCorrectPath = ControlPanelCorrect.Path;
        ControlPanelWrongPath = ControlPanelWrong.Path;
        PowerLossPath = PowerLoss.Path;
        PowerRegainedPath = PowerRegained.Path;
        PickupPaperPath = PickupPaper.Path;
        SmithIntroduction_TutorialPath = SmithIntroduction_Tutorial.Path;
        TaskPanel_TutorialPath = TaskPanel_Tutorial.Path;
        EnergyPanel_TutorialPath = EnergyPanel_Tutorial.Path;
        MelodyIntroduction_TutorialPath = MelodyIntroduction_Tutorial.Path;
        Melody_TutorialPath = Melody_Tutorial.Path;
        BackToCameras_TutorialPath = BackToCameras_Tutorial.Path;
        BackLater_TutorialPath = BackLater_Tutorial.Path;
        BlackOut_TutorialPath = BlackOut_Tutorial.Path;
        DayCompleted1_TutorialPath = DayCompleted1_Tutorial.Path;
        DayCompleted2_TutorialPath = DayCompleted2_Tutorial.Path;
    }
#endif

}
