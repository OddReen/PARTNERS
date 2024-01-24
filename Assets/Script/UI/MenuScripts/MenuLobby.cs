using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class MenuLobby : Menu
{
    [SerializeField] Button playerReady_BT;
    [SerializeField] Button quitToMainMenu_BT;
    private void Start()
    {
        VirtualStart();
    }
    protected override void VirtualStart()
    {
        Debug.Log("Menu Lobby Starting...");
        Debug.Log("Player Ready Button adding Listeners...");
        playerReady_BT.onClick.AddListener(() =>
        {
            LobbyManager.Instance.SetPlayerReadyStatus();
        });
        Debug.Log("Player Ready Button Listeners Added"); 
        Debug.Log("Quit to main menu Button adding Listeners...");
        quitToMainMenu_BT.onClick.AddListener(() =>
        {
            MultiplayerManager.Instance.Shutdown();
            Loader.Load(Loader.Scene.MainMenu);
        });
        Debug.Log("Quit to main menu Button Listeners Added");
        base.VirtualStart();
        Debug.Log("Menu Lobby Start Complete");
    }
}
