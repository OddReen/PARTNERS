using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoBackToCameraRoomTask : TutorialTask_Multiplayer
{
    [SerializeField] TutorialVoiceLines voiceLines;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && isTaskActive)
        {  
            SFX_Manager_Multiplayer.Instance.PlaySound_ServerRpc(voiceLines.BackLater_Tutorial.Path);
            CompleteTask();       
        }
    }
}
