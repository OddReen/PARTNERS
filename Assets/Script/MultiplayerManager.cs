using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MultiplayerManager : NetworkBehaviour
{
    //Known bugs list 
    //If host leaves and the client exits and joins a new game it will soflock
    //If host starts and presses ready in lobby and then a client joins it will not show the ready above the host. Purely visual bug
    //Defered message warning fuck if i know desativei todos os objetos e ainda fucking aparece


    public static MultiplayerManager Instance;

    public int MaxPlayers { get; private set; } = 2;

    //Events
    public event EventHandler OnJoinGameAttempt;
    public event EventHandler OnJoinGameAttemptFailed;
    public event EventHandler OnPlayerDataNetworkListChanged;

    private NetworkList<PlayerData> playerDataNetworkList;


    #region Initialization
    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        playerDataNetworkList = new NetworkList<PlayerData>();

    }
    public override void OnNetworkSpawn()
    {
        playerDataNetworkList.OnListChanged += PlayerDataNetworkList_OnListChanged;
    }
    #endregion

    //Usa para dar start e shutdown ão server não uses as funçoes do network manager
    #region Start&End Client/Host
    //Começar como host
    public void StartHost()
    {
        NetworkManager.Singleton.ConnectionApprovalCallback += NetworkManager_ConectionApprovalCallback;
        NetworkManager.Singleton.OnClientConnectedCallback += NetworkManager_Server_OnClientConnectedCallback;
        NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_Server_OnClientDisconnectCallback;
        NetworkManager.Singleton.StartHost();
    }
    //Começar como client
    public void StartClient()
    {
        if (OnJoinGameAttempt == null)
        {
            Debug.Log("Bug");
        }
        OnJoinGameAttempt?.Invoke(this, EventArgs.Empty);

        NetworkManager.Singleton.OnClientDisconnectCallback += MultiplayerManager_Client_OnClientDisconnectCallback;
        NetworkManager.Singleton.StartClient();
    }

    public void Shutdown()
    {
        if (IsServer)
        {
            //Não executar codigo do server se for o server a desligarse it goes wrong
            NetworkManager.Singleton.OnClientDisconnectCallback -= NetworkManager_Server_OnClientDisconnectCallback;
        }

        NetworkManager.Shutdown();
    }
    #endregion

    //Funçoes usadas para verificar informação do multiplayer Manager
    #region Miscelaneous
    //Verifica se o um index esta conectado ou não
    public bool IsPlayerIndexConected(int playerIndex)
    {
        return playerIndex < playerDataNetworkList.Count;
    }
    //Receber a player data de um player atraves do index
    //A ordem array é a ordem que os player se juntaram. Host e sempre 0
    public PlayerData GetPlayerDataFromIndex(int playerIndex)
    {
        return playerDataNetworkList[playerIndex];
    }
    #endregion

    #region Events
    //Manda um evento quando a lista é alterada
    private void PlayerDataNetworkList_OnListChanged(NetworkListEvent<PlayerData> changeEvent)
    {
        OnPlayerDataNetworkListChanged?.Invoke(this, EventArgs.Empty);
    }
    //Atualiza a lista de player data quando um player se junta
    private void NetworkManager_Server_OnClientConnectedCallback(ulong clientId)
    {
        playerDataNetworkList.Add(new PlayerData
        {
            clientId = clientId,
        });
    }
    //Lança um evento quando o client se disconecta
    private void MultiplayerManager_Client_OnClientDisconnectCallback(ulong clientId)
    {
        OnJoinGameAttemptFailed?.Invoke(this, EventArgs.Empty);
    }
    private void NetworkManager_Server_OnClientDisconnectCallback(ulong clientId)
    {
        Debug.Log("Player Disconnected");
        for (int i = 0; i < playerDataNetworkList.Count; i++)
        {
            PlayerData playerData = playerDataNetworkList[i];
            if (playerData.clientId == clientId)
            {
                playerDataNetworkList.RemoveAt(i);
            }
        }
    }
    //Autoriza a conection entre o client e o server
    //Estou a verificar se o numero de players que estão conectados são iguais ao numero maximo permitido
    //Se sim a conection é rejeitada e uma mensagem é enviada para o client que falhoe a dizer que o numero maximo de players ja foi atingido
    private void NetworkManager_ConectionApprovalCallback(NetworkManager.ConnectionApprovalRequest connectionApprovalRequest, NetworkManager.ConnectionApprovalResponse connectionApprovalResponse)
    {
        if (NetworkManager.Singleton.ConnectedClientsIds.Count >= MaxPlayers)
        {
            connectionApprovalResponse.Approved = false;
            connectionApprovalResponse.Reason = "Max players reached";
            return;
        }
        connectionApprovalResponse.Approved = true;
    }
    #endregion
}
