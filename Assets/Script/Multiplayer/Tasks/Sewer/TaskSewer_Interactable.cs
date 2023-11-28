using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskSewer_Interactable : Interactable
{
    public override void Interact()
    {
        TaskSewer_Multiplayer.Instance.FillMinigame(gameObject.name);
    }
}
