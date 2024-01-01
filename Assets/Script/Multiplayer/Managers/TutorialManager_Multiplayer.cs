using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class TutorialManager_Multiplayer : NetworkBehaviour
{
    public static TutorialManager_Multiplayer Instance;

    [SerializeField] List<Tutorial_Multiplayer> tutorialList = new();
    int currentTutorial = -1;

    private void Awake()
    {
        Instance = this;
    }
    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            NextTutorial_ServerRpc();
            //Put network start here if needed
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void NextTutorial_ServerRpc()
    {
        NextTutorial_ClientRpc();
    }
    [ClientRpc]
    private void NextTutorial_ClientRpc()
    {
        currentTutorial++;
        tutorialList[currentTutorial].ActivateTutorial();
    }
}
