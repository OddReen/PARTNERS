using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class TaskTutorial : Tutorial_Multiplayer
{
    [SerializeField] OutlineController_Multiplayer cameraConsoleOutline;
    [SerializeField] float outlineTime;

    [SerializeField] SFX_List sfx_List;

    protected override void ActivateTutorialServerSide()
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
        SFX_Manager_Multiplayer.Instance.PlaySound_ServerRpc(sfx_List.SmithIntroduction_TutorialPath);
        //Yes its hardcoded but no time lol
        Invoke(nameof(StartTasks), 5f);
    }
    private void StartTasks()
    {
        SFX_Manager_Multiplayer.Instance.PlaySound_ServerRpc(sfx_List.TaskPanel_TutorialPath);
        cameraConsoleOutline.ActivateOutline_ServerRpc(true);
        ActivateTasks();
    }
    protected override void CompleteTutorial()
    {
        cameraConsoleOutline.ActivateOutline_ServerRpc(false);
        base.CompleteTutorial();
    }
}
