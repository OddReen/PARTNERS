using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayVoiceLineTest_Interactable : Interactable
{
    [SerializeField] EventReference voiceLine;

    public override void Interact(PlayerController playerController)
    {
        RuntimeManager.PlayOneShotAttached(voiceLine,MultiplayerPlayerController.DebugInstance.gameObject);
    }
}
