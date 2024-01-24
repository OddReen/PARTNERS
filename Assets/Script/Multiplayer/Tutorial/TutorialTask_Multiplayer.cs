using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TutorialTask_Multiplayer : NetworkBehaviour
{
    protected Tutorial_Multiplayer tutorial;

    public string TaskDescription { get { return taskDesctiption; } private set { taskDesctiption = value; } }
    [SerializeField] string taskDesctiption = "Kill God";
    //public float TaskTime { get { return taskTime; } private set { taskTime = value; } }
    //[SerializeField] float taskTime = 20f;

    //THIS MAY NOT BE NEEDED IN TUTORIAL
    protected List<TutorialTaskStatus_Multiplayer> activeTasksList = new List<TutorialTaskStatus_Multiplayer>();

    public int ActiveTaskCount { get { return activeTasksList.Count; } private set { } }

    [HideInInspector] public bool isTaskActive = false;

    //Server
    public void ActivateTask(TutorialTaskStatus_Multiplayer taskInfo, Tutorial_Multiplayer tutorial)
    {
        this.tutorial = tutorial;
        IsTaskActive_ClientRpc(true);
        activeTasksList.Add(taskInfo);
        //Coloca codigo que determina que a atividade é possivel aqui
    }
    [ServerRpc(RequireOwnership = false)]
    protected void CompleteTask_ServerRpc()
    {
        CompleteTask();
    }
    protected virtual void CompleteTask()
    {
        tutorial.TaskCompletedServerRpc(activeTasksList[0].TaskIndex);
    }
    public virtual void FailTask()
    {
        tutorial.TaskFailed_ServerRpc(activeTasksList[0].TaskIndex);
    }
    public void ClearTaskFromList()
    {
        activeTasksList.RemoveAt(0);
        if (ActiveTaskCount == 0)
        {
            IsTaskActive_ClientRpc(false);
        }
    }
    [ClientRpc]
    private void IsTaskActive_ClientRpc(bool isTaskActive)
    {
        this.isTaskActive = isTaskActive;
    }
}
