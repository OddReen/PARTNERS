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
            multiplayerManager.StartHost();
            //Necessario colocar um delay para o server não estar a iniciar no mesmo frame que o player
            Invoke(nameof(SpawnPlayer), 0.1f);
        }
    }
    private void SpawnPlayer()
    {
        InGameManager.Instance.SpawnPlayers();
    }
}
