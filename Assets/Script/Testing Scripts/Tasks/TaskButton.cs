using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskButton : Interactable
{
    [SerializeField] TaskTest taskTest;

    public override void Interact(PlayerController playerController)
    {
        taskTest.ButtonPressed();
    }
}
