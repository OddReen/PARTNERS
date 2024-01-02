using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MelodyTutorial : Tutorial_Multiplayer
{
   [SerializeField] TutorialVoiceLines sFX_List;


    [SerializeField] OutlineController_Multiplayer melodyOutline;

    [SerializeField] OutlineController_Multiplayer panelOutline;
    public override void ActivateTutorial()
    {
        Invoke(nameof(StartEnergyPanelTutorial), 5f);
    }
    public void StartEnergyPanelTutorial()
    {
        foreach (TutorialTask_Multiplayer task in tutorialTaskTutorial)
        {
            TutorialTaskStatus_Multiplayer taskStatus = Instantiate(activeTask_Prefab, activeTask_Container);
            taskStatus.AssignTask(task, TaskCount);
            activeTasksStatusList.Add(taskStatus);
            task.ActivateTask(taskStatus, this);
        }
        DoorManager_Multiplayer.Instance.ChangeAllDoorLocks_ServerRpc(false);
        SFX_Manager_Multiplayer.Instance.PlaySound_ServerRpc(sFX_List.EnergyPanel_Tutorial.Path);
        EnergyManager_Multiplayer.Instance.ChangeEnergy_ServerRpc(-80);
        panelOutline.StartOutlineTimer_ServerRpc(6f);
        Invoke(nameof(MelodyTask), 6f);
    }
    private void MelodyTask()
    {
        melodyOutline.ActivateOutline_ServerRpc(true);
        SFX_Manager_Multiplayer.Instance.PlaySound_ServerRpc(sFX_List.MelodyIntroduction_Tutorial.Path);
    }
    

}
