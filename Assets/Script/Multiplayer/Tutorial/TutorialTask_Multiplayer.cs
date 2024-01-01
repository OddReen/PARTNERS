using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTask_Multiplayer : MonoBehaviour
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
    public virtual void ActivateTask(TutorialTaskStatus_Multiplayer taskInfo, Tutorial_Multiplayer tutorial)
    {
        this.tutorial = tutorial;
        isTaskActive = true;
        activeTasksList.Add(taskInfo);
        //Coloca codigo que determina que a atividade é possivel aqui
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
            isTaskActive = false;
        }
    }
}
