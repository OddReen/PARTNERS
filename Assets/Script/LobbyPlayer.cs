using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class LobbyPlayer : MonoBehaviour
{
    [SerializeField] int playerIndex;
    [SerializeField] GameObject readyGameObject;
    private void Start()
    {
        MultiplayerManager.Instance.OnPlayerDataNetworkListChanged += MultiplayerManager_OnPlayerDataNetworkListChanged;
        LobbyManager.Instance.OnReadyChange += PlayerReady_OnReadyChange;
        UpdatePlayer();
    }
    private void OnDestroy()
    {
        MultiplayerManager.Instance.OnPlayerDataNetworkListChanged -= MultiplayerManager_OnPlayerDataNetworkListChanged;
        LobbyManager.Instance.OnReadyChange -= PlayerReady_OnReadyChange;
    }
    private void PlayerReady_OnReadyChange(object sender, System.EventArgs e)
    {
        UpdatePlayer();
    }

    private void MultiplayerManager_OnPlayerDataNetworkListChanged(object sender, System.EventArgs e)
    {
        UpdatePlayer();
    }

    private void UpdatePlayer()
    {
        if (MultiplayerManager.Instance.IsPlayerIndexConected(playerIndex))
        {
            Show();
            PlayerData playerData = MultiplayerManager.Instance.GetPlayerDataFromIndex(playerIndex);
            readyGameObject.SetActive(LobbyManager.Instance.IsPlayerReady(playerData.clientId));
        }
        else
        {
            Hide();
            readyGameObject.SetActive(false);
        }
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
    private void Show()
    {
        gameObject.SetActive(true);
    }
}
