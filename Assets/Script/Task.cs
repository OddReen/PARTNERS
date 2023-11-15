using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Task : MonoBehaviour
{
    public bool IsRepeatable;
    public string TaskDescription { get { return taskDesctiption; } private set { taskDesctiption = value; } }
    [SerializeField] string taskDesctiption = "Kill God";
    public float TaskTime { get { return taskTime; } private set { taskTime = value; } }
    [SerializeField] float taskTime = 20f;

    protected List<TaskStatus> activeTasksList = new List<TaskStatus>();
    public virtual void ActivateTask(TaskStatus taskInfo)
    {
        activeTasksList.Add(taskInfo);
        //Coloca codigo que determina que a atividade é possivel aqui
    }

    protected virtual void CompleteTask()
    {
        TaskManager.Instance.TaskCompletedServerRpc(activeTasksList[0].TaskIndex);
        activeTasksList.RemoveAt(0);
        Debug.Log("Task Completed");
    }
    public virtual void FailTask()
    {
        TaskManager.Instance.TaskFailedServerRpc(activeTasksList[0].TaskIndex);
        activeTasksList.RemoveAt(0);
        Debug.Log("Task Failed");
    }
}
