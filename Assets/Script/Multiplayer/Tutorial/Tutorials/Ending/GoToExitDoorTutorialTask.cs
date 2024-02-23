using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToExitDoorTutorialTask : TutorialTask_Multiplayer
{
    [SerializeField] Ending ending;
    private void OnTriggerEnter(Collider other)
    {
        if (IsServer && other.CompareTag("Player") && isTaskActive)
        {
            ending.PlayerInPosition();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (IsServer && other.CompareTag("Player") && isTaskActive)
        {
            ending.PlayerExitedPosition();
        }
    }
}
