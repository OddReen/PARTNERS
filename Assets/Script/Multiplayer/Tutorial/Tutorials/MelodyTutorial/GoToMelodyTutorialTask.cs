using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToMelodyTutorialTask : TutorialTask_Multiplayer
{
    [SerializeField] TutorialVoiceLines voiceLines;

    [SerializeField] OutlineController_Multiplayer melodyOutline;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && isTaskActive)
        {
            CompleteTask();
            SFX_Manager_Multiplayer.Instance.PlaySound_ServerRpc(voiceLines.Melody_Tutorial.Path);
            melodyOutline.ActivateOutline_ServerRpc(false);
        }
    }
}
