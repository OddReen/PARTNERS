using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MelodyTutorial : Tutorial_Multiplayer
{
    [SerializeField] SFX_List sFX_List;


    [SerializeField] OutlineController_Multiplayer melodyOutline;

    [SerializeField] OutlineController_Multiplayer panelOutline;

    const float waitTimeToStart = 5f;
    const float waitTimeToStart2 = 6f;
    protected override void ActivateTutorialServerSide()
    {
        Invoke(nameof(StartEnergyPanelTutorialServer), waitTimeToStart);
    }
    private void StartEnergyPanelTutorialServer()
    {
        DoorManager_Multiplayer.Instance.ChangeAllDoorLocks_ServerRpc(false);
        SFX_Manager_Multiplayer.Instance.PlaySound_ServerRpc(sFX_List.EnergyPanel_TutorialPath);
        EnergyManager_Multiplayer.Instance.ChangeEnergy_ServerRpc(-80);
        panelOutline.StartOutlineTimer_ServerRpc(waitTimeToStart2);
        Invoke(nameof(MelodyTask), waitTimeToStart2);
    }
    private void MelodyTask()
    {
        melodyOutline.ActivateOutline_ServerRpc(true);
        ActivateTasks();
        SFX_Manager_Multiplayer.Instance.PlaySound_ServerRpc(sFX_List.MelodyIntroduction_TutorialPath);
    }


}
