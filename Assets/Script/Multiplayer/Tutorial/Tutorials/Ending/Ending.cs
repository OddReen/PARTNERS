using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class Ending : Tutorial_Multiplayer
{
    [SerializeField] OutlineController_Multiplayer exitDoor;
    [SerializeField] EndingMelodyAnimation endingAnimation;
    int numberOfPlayers;
    int playerInPosition;
    public override void OnNetworkSpawn()
    {
        numberOfPlayers = NetworkManager.Singleton.ConnectedClientsList.Count;
    }
    protected override void ActivateTutorialServerSide()
    {
        exitDoor.ActivateOutline_ServerRpc(true);
        base.ActivateTutorialServerSide();
    }
    public void PlayerInPosition()
    {
        playerInPosition++;
        if (playerInPosition >= numberOfPlayers)
        {
            EndingAnim_ClientRpc();
        }
    }
    [ClientRpc]
    private void EndingAnim_ClientRpc()
    {
        endingAnimation.EndingAnimation();
    }
    public void PlayerExitedPosition()
    {
        playerInPosition--;

    }
}
