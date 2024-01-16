using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MultiplayerDebug : MonoBehaviour
{
    [SerializeField] GameObject networkManagerPrefab;
    [SerializeField] MultiplayerManager multiplayerManagerPrefab;
    private void Awake()
    {
        if (MultiplayerManager.Instance == null)
        {
            Instantiate(networkManagerPrefab);
            MultiplayerManager multiplayerManager = Instantiate(multiplayerManagerPrefab);
            //Necessario colocar um delay para o server não estar a iniciar no mesmo frame que o player
            StartCoroutine(StartGame(multiplayerManager));
        }
    }
    IEnumerator StartGame(MultiplayerManager multiplayerManager)
    {
        yield return new WaitForEndOfFrame();
        multiplayerManager.StartHost();
        yield return new WaitForSeconds(0f);
        InGameManager.Instance.SpawnPlayers();
    }
    private void SpawnPlayer()
    {
        InGameManager.Instance.SpawnPlayers();
    }
}
