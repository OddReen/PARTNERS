using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToMelodyTutorialTask : TutorialTask_Multiplayer
{
    [SerializeField] SFX_List voiceLines;

    [SerializeField] OutlineController_Multiplayer melodyOutline;
    private void OnTriggerEnter(Collider other)
    {
        if (IsServer)
        {
            if (other.CompareTag("Player") && isTaskActive)
            {
                CompleteTask();
                SFX_Manager_Multiplayer.Instance.PlaySound_ServerRpc(voiceLines.Melody_TutorialPath);
                melodyOutline.ActivateOutline_ServerRpc(false);
            }
        }

    }
}
