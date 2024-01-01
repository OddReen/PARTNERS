using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class TaskTutorial : Tutorial_Multiplayer
{
    [SerializeField] OutlineController_Multiplayer cameraConsoleOutline;
    [SerializeField] float outlineTime;

    [SerializeField] TutorialVoiceLines sfx_List;

    public override void ActivateTutorial()
    {
        Invoke(nameof(LockAllDoors), 0.2f);
        Invoke(nameof(PlayStartSound), 5f);
    }
    //The server isnt fully created when this is executed so im delaying it by a couple of milliseconds
    public void LockAllDoors()
    {
        DoorManager_Multiplayer.Instance.ChangeAllDoorLocks_ServerRpc(true);
    }
    private void PlayStartSound()
    {
        SFX_Manager_Multiplayer.Instance.PlaySound_ServerRpc(sfx_List.SmithIntroduction_Tutorial.Path);
        //Yes its hardcoded but no time lol
        Invoke(nameof(StartTasks), 5f);
    }
    private void StartTasks()
    {
        SFX_Manager_Multiplayer.Instance.PlaySound_ServerRpc(sfx_List.TaskPanel_Tutorial.Path);
        cameraConsoleOutline.ActivateOutline_ServerRpc(true);
        foreach (TutorialTask_Multiplayer task in tutorialTaskTutorial)
        {
            TutorialTaskStatus_Multiplayer taskStatus = Instantiate(activeTask_Prefab, activeTask_Container);
            taskStatus.AssignTask(task, TaskCount);
            activeTasksStatusList.Add(taskStatus);
            task.ActivateTask(taskStatus, this);
        }
    }
    protected override void CompleteTutorial()
    {
        cameraConsoleOutline.ActivateOutline_ServerRpc(false);
        base.CompleteTutorial();
    }
}
