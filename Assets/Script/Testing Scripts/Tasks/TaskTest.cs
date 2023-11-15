using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskTest : Task
{
    public override void ActivateTask(TaskStatus taskInfo)
    {
        base.ActivateTask(taskInfo);
    }
    public void ButtonPressed()
    {
        if (activeTasksList.Count>0)
        {
            CompleteTask();
        }
    }
}
