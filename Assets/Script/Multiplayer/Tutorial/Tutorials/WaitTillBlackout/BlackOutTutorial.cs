using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackOutTutorial : Tutorial_Multiplayer
{
    [SerializeField] float timeTillBlackOut;

    [SerializeField] SFX_List voiceLines;

    [SerializeField] OutlineController_Multiplayer cpGasOutline;
    [SerializeField] OutlineController_Multiplayer cpEnergyOutline;
    protected override void ActivateTutorialServerSide()
    {
        BlackoutManager_Multiplayer.Instance.StartBlackout += StartBlackoutTutorial; 
        BlackoutManager_Multiplayer.Instance.EndBlackout += EndTutorial;
    }

    private void StartBlackoutTutorial(object sender, System.EventArgs e)
    {
        BlackoutManager_Multiplayer.Instance.StartBlackout -= StartBlackoutTutorial; 
        SFX_Manager_Multiplayer.Instance.PlaySound_ServerRpc(voiceLines.BlackOut_TutorialPath);
        cpGasOutline.StartOutlineTimer_ServerRpc(10f);
        cpEnergyOutline.StartOutlineTimer_ServerRpc(10f);
    }

    private void EndTutorial(object sender, System.EventArgs e)
    {
        BlackoutManager_Multiplayer.Instance.EndBlackout -= EndTutorial;
        SFX_Manager_Multiplayer.Instance.PlaySound_ServerRpc(voiceLines.DayCompleted1_TutorialPath);
        Invoke(nameof(EndVoice2), 5.2f);
    }
    private void EndVoice2()
    {
        SFX_Manager_Multiplayer.Instance.PlaySound_ServerRpc(voiceLines.DayCompleted2_TutorialPath);
    }
}
