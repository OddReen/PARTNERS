using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class TutorialManager_Multiplayer : NetworkBehaviour
{
    //public static TutorialManager_Multiplayer Instance;

    //[SerializeField] List<TutorialTask_Multiplayer> tutorialList = new();

    //List<TutorialTaskStatus_Multiplayer> activeTaskStatus = new();
    //int currentTutorial;

    //private void Awake()
    //{
    //    Instance = this;
    //}
    //public override void OnNetworkSpawn()
    //{
    //    if (IsServer)
    //    {
    //        tutorialList[currentTutorial].ActivateTask();
    //        //Put network start here if needed
    //    }
    //}
    //private void Start()
    //{
    //    //Need to be in start because of execution order
    //    DoorManager_Multiplayer.Instance.ChangeAllDoorLocks_ServerRpc(true);
    //}

    //[ServerRpc(RequireOwnership = false)]
    //public void NextTutorial_ServerRpc()
    //{
    //    currentTutorial++;
    //    tutorialList[currentTutorial].ActivateTutorial();
    //}
}
