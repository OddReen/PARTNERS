using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class MenuLobby : Menu
{
    [SerializeField] Button playerReady_BT;
    [SerializeField] Button quitToMainMenu_BT;
    protected override void VirtualStart()
    {
        base.VirtualStart();
        playerReady_BT.onClick.AddListener(() =>
        {
            PlayerReady.Instance.SetPlayerReadyStatus();
        });
        quitToMainMenu_BT.onClick.AddListener(() =>
        {
            MultiplayerManager.Instance.Shutdown();
            Loader.Load(Loader.Scene.MainMenu);
        });
    }
}
