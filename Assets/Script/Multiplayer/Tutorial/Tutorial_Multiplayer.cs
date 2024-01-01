using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Tutorial_Multiplayer : NetworkBehaviour
{
    [SerializeField] protected List<TutorialTask_Multiplayer> tutorialTaskTutorial = new();

   protected List<TutorialTaskStatus_Multiplayer> activeTasksStatusList = new();

    [SerializeField] protected TutorialTaskStatus_Multiplayer activeTask_Prefab;
    [SerializeField] protected Transform activeTask_Container;

    protected int TaskCount { get { return activeTasksStatusList.Count; } }

    //Rpc não gostam de inheritance so functions que não são herdadas é que podem possuir server Rpcs

    public virtual void ActivateTutorial()
    {
        foreach (TutorialTask_Multiplayer task in tutorialTaskTutorial)
        {
            TutorialTaskStatus_Multiplayer taskStatus = Instantiate(activeTask_Prefab, activeTask_Container);
            taskStatus.AssignTask(task, TaskCount);
            activeTasksStatusList.Add(taskStatus);
            task.ActivateTask(taskStatus, this);
        }
    }
   
    [ServerRpc(RequireOwnership = false)]
    public void TaskCompletedServerRpc(int taskIndex)
    {
        DeleteTask_ClientRpc(taskIndex,true);
    }

    [ServerRpc(RequireOwnership = false)]
    public void TaskFailed_ServerRpc(int taskIndex)
    {
        DeleteTask_ClientRpc(taskIndex,false);
    }

    [ClientRpc]
    private void DeleteTask_ClientRpc(int taskIndex,bool wasTaskCompleted)
    {
        if (taskIndex + 1 < TaskCount)
        {
            for (int i = taskIndex + 1; i < TaskCount; i++)
            {
                activeTasksStatusList[i].ReduceIndex();
            }
        }
        TutorialTaskStatus_Multiplayer taskStatus = activeTasksStatusList[taskIndex];
        activeTasksStatusList.Remove(taskStatus);
        taskStatus.DeleteTaskStatus(wasTaskCompleted);

        taskStatus.TaskReference.ClearTaskFromList();

        if (TaskCount == 0)
        {
            CompleteTutorial();
        }
    } 

    protected virtual void CompleteTutorial()
    {
        TutorialManager_Multiplayer.Instance.NextTutorial_ServerRpc();
    }
}
