using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class TutorialManager_Multiplayer : NetworkBehaviour
{
    public static TutorialManager_Multiplayer Instance;

    [SerializeField] List<Tutorial_Multiplayer> tutorialList = new();
    int currentTutorial = -1;

    [Header("Debug")]
    [SerializeField] int startInTutorial = 0;
    private void Awake()
    {
        Instance = this;
    }
    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            currentTutorial = startInTutorial;
            Invoke(nameof(Delay),0.5f);
        }
    }
    private void Delay()
    {
        Debug.Log("StartTutorial");
        tutorialList[currentTutorial].ActivateTutorial_ServerRpc();
    }
    [ServerRpc(RequireOwnership = false)]
    public void NextTutorial_ServerRpc()
    {
        currentTutorial++;
        tutorialList[currentTutorial].ActivateTutorial_ServerRpc();
    }
}
