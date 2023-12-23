using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskSewer_Interactable : Interactable
{
    public override void Interact(PlayerController playerController)
    {
        TaskSewer_Multiplayer.Instance.FillMinigame(gameObject.name);
    }
}
