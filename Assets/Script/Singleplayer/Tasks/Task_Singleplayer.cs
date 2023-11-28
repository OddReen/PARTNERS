using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Task_Singleplayer : MonoBehaviour
{
    public bool IsRepeatable;
    public string TaskDescription { get { return taskDesctiption; } private set { taskDesctiption = value; } }
    [SerializeField] string taskDesctiption = "Kill God";
    public float TaskTime { get { return taskTime; } private set { taskTime = value; } }
    [SerializeField] float taskTime = 20f;

    protected List<TaskStatus_Singleplayer> activeTasksList = new List<TaskStatus_Singleplayer>();

    public int ActiveTaskCount { get { return activeTasksList.Count; } private set { } }

    protected bool isTaskActive = false;
    public virtual void ActivateTask(TaskStatus_Singleplayer taskInfo)
    {
        isTaskActive = true;
        activeTasksList.Add(taskInfo);
        //Coloca codigo que determina que a atividade é possivel aqui
    }

    protected virtual void CompleteTask()
    {
        TaskManager_Singleplayer.Instance.TaskCompleted(activeTasksList[0].TaskIndex);
        ClearTaskFromList();
        Debug.Log("Task Completed");
    }
    public virtual void FailTask()
    {
        TaskManager_Singleplayer.Instance.TaskFailed(activeTasksList[0].TaskIndex);
        ClearTaskFromList();
        Debug.Log("Task Failed");
    }
    protected virtual void ClearTaskFromList()
    {
        activeTasksList.RemoveAt(0);
        if (ActiveTaskCount == 0)
        {
            isTaskActive = false;
        }
    }
}
