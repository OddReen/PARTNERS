using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskTest : Task_Multiplayer
{
    public override void ActivateTask(TaskStatus_Multiplayer taskInfo)
    {
        base.ActivateTask(taskInfo);
        isTaskActive = true;
    }
    public void ButtonPressed()
    {
        if (isTaskActive)
        {
            CompleteTask();
        }
    }
}
