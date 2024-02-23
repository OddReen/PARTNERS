using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Task_Multiplayer : NetworkBehaviour
{
    //Make tasks inherit from NetworkBehaviour its probably for the best but no time gota go fast
    public bool IsRepeatable;
    public string TaskDescription { get { return taskDesctiption; } private set { taskDesctiption = value; } }
    [SerializeField] string taskDesctiption = "Kill God";
    public float TaskTime { get { return taskTime; } private set { taskTime = value; } }
    [SerializeField] float taskTime = 20f;

    protected List<TaskStatus_Multiplayer> activeTasksList = new List<TaskStatus_Multiplayer>();

    public int ActiveTaskCount { get { return activeTasksList.Count; } private set { } }

    protected bool isTaskActive = false;
    public virtual void ActivateTask(TaskStatus_Multiplayer taskInfo)
    {
        IsTaskActive_ClientRpc(true);
        activeTasksList.Add(taskInfo);
        //Coloca codigo que determina que a atividade é possivel aqui
    }
    [ServerRpc(RequireOwnership = false)]
    public void CompleteTask_ServerRpc()
    {
        CompleteTask();
    }
    protected virtual void CompleteTask()
    {
        //Delivery Objects dont despawn
        if (isTaskActive == false)
        {
            return;
        }
        TaskManager_Multiplayer.Instance.TaskCompleted_ServerRpc(activeTasksList[0].TaskIndex);
        Debug.Log("Task Completed");
    }
    public virtual void FailTask()
    {
        TaskManager_Multiplayer.Instance.TaskFailed_ServerRpc(activeTasksList[0].TaskIndex);

        Debug.Log("Task Failed");
    }
    //Called trough ClientRpc so its deleted in both players
    public void ClearTaskFromList()
    {
        activeTasksList.RemoveAt(0);
        if (ActiveTaskCount == 0)
        {
            IsTaskActive_ClientRpc(false);
        }
    }
    [ClientRpc]
    protected void IsTaskActive_ClientRpc(bool isTaskActive)
    {
        this.isTaskActive = isTaskActive;
    }
}
