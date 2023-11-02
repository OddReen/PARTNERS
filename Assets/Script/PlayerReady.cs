using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;

public class PlayerReady : NetworkBehaviour
{
    public static PlayerReady Instance { get; private set; }

    private Dictionary<ulong, bool> playerReadyDictionary;

    public event EventHandler OnReadyChange;

    bool isReady = false;

    [Header("Debug")]
    [SerializeField] bool debugAllowHostStart;

    public override void OnNetworkSpawn()
    {
        Instance = this;
        playerReadyDictionary = new Dictionary<ulong, bool>();
    }

    public override void OnNetworkDespawn()
    {
        SetPlayerUnReadyServerRpc();
    }

    public void SetPlayerReadyStatus()
    {
        isReady = !isReady;
        if (isReady)
        {
            SetPlayerReadyServerRpc();
        }
        else
        {
            SetPlayerUnReadyServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerUnReadyServerRpc(ServerRpcParams serverRpcParams = default)
    {
        SetPlayerUnReadyClientRpc(serverRpcParams.Receive.SenderClientId);
        playerReadyDictionary[serverRpcParams.Receive.SenderClientId] = false;
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerReadyServerRpc(ServerRpcParams serverRpcParams = default)
    {
        SetPlayerReadyClientRpc(serverRpcParams.Receive.SenderClientId);
        playerReadyDictionary[serverRpcParams.Receive.SenderClientId] = true;

        bool allClientsReady = true;
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            if (!playerReadyDictionary.ContainsKey(clientId) || !playerReadyDictionary[clientId])
            {
                allClientsReady = false;
                break;
            }
        }
        if (NetworkManager.Singleton.ConnectedClientsList.Count < MultiplayerManager.Instance.MaxPlayers)
        {
            allClientsReady = false;
        }

        #region Debug
        //Se o debug estiver ativo e o client que mandou a informação é o host allow start
        if (debugAllowHostStart && serverRpcParams.Receive.SenderClientId == NetworkManager.ServerClientId)
        {
            allClientsReady = true;
        }
        #endregion

        if (allClientsReady)
        {
            Loader.LoadNetwork(Loader.Scene.MultiplayerTest);
        }
    } 

    [ClientRpc]
    private void SetPlayerReadyClientRpc(ulong clientId)
    {
        playerReadyDictionary[clientId] = true;
        OnReadyChange?.Invoke(this, EventArgs.Empty);
    }

    [ClientRpc]
    private void SetPlayerUnReadyClientRpc(ulong clientId)
    {
        playerReadyDictionary[clientId] = false;
        OnReadyChange?.Invoke(this, EventArgs.Empty);
    }

    public bool IsPlayerReady(ulong clientId)
    {
        return playerReadyDictionary.ContainsKey(clientId) && playerReadyDictionary[clientId];
    }
}
