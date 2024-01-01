using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractWithMelodyTask : TutorialTask_Multiplayer
{
    protected override void CompleteTask()
    {
        EnergyManager_Multiplayer.Instance.ChangeEnergy_ServerRpc(10000);
        base.CompleteTask();
    }
    public void TaskCompleted()
    {
        CompleteTask();
    }
}
