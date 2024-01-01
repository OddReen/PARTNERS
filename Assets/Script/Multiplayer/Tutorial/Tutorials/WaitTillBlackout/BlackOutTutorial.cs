using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackOutTutorial : Tutorial_Multiplayer
{
    [SerializeField] float timeTillBlackOut;

    [SerializeField] TutorialVoiceLines voiceLines;

    [SerializeField] OutlineController_Multiplayer cpGasOutline;
    public override void ActivateTutorial()
    {
        Invoke(nameof(BlackOutVoice), timeTillBlackOut);
    }
    private void BlackOutVoice()
    {
        SFX_Manager_Multiplayer.Instance.PlaySound_ServerRpc(voiceLines.BlackOut_Tutorial.Path);
        BlackoutManager_Multiplayer.Instance.StartBlackout_ServerRpc();
        cpGasOutline.ActivateOutline_ServerRpc(true);
    }
}
