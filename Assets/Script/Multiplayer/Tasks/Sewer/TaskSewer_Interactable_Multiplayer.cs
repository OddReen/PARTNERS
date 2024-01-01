using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskSewer_Interactable_Multiplayer : Interactable
{
    public override void Interact(PlayerController playerController)
    {
        TaskSewer_Multiplayer.Instance.FillMinigame(gameObject.name);
    }
}
