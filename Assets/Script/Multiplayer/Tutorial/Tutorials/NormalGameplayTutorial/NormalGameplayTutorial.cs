using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalGameplayTutorial : Tutorial_Multiplayer
{
    [SerializeField] SFX_List voiceLines;

    protected override void ActivateTutorialServerSide()
    {
        SFX_Manager_Multiplayer.Instance.PlaySound_ServerRpc(voiceLines.BackToCameras_TutorialPath);
        base.ActivateTutorialServerSide();
    }
    protected override void CompleteTutorial()
    {
        TaskManager_Multiplayer.Instance.StartCreatingTasks_ServerRpc();
        EnergyManager_Multiplayer.Instance.StartEnergyDeacrease_ServerRpc();
        DoorManager_Multiplayer.Instance.LockMelodyDoor_ServerRpc(true);
        base.CompleteTutorial();
    }
}
