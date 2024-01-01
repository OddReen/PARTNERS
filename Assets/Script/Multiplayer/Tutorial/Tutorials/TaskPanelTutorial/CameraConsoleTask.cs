using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraConsoleTask : TutorialTask_Multiplayer
{
    public void CameraButtonPressed()
    {
        if (isTaskActive)
        {
            CompleteTask();
        }
    }
}
