using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.UI;

public class MenuLAN : Menu
{
    [SerializeField] Button quitToMenu_BT;
    [Header("Host")]
    [SerializeField] Button createHost_BT;
    [Header("Client Join Info")]
    [SerializeField] Button createClient_BT;
    [SerializeField] TMP_InputField ipv4Input;
    [Header("ConnectingUI")]
    [SerializeField] ConnectingUI connectingUI;
    [SerializeField] ConnectionFailedUI connectionFailedUI;
    [Header("Hover Text")]
    [SerializeField] TextMeshProUGUI hoverText;
    protected override void VirtualStart()
    {
        base.VirtualStart();
        AddButtonListeners(); 
        MultiplayerManager.Instance.OnJoinGameAttempt += MultiplayerManager_OnJoinGameAttempt;
        MultiplayerManager.Instance.OnJoinGameAttemptFailed += MultiplayerManager_OnJoinGameAttemptFailed;
    }
    private void AddButtonListeners()
    {
        createHost_BT.onClick.AddListener(() =>
        {
            MultiplayerManager.Instance.StartHost();
            Loader.LoadNetwork(Loader.Scene.MenuLobby);
        });
        createClient_BT.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.GetComponent<UnityTransport>().ConnectionData.Address = ipv4Input.text;
            MultiplayerManager.Instance.StartClient();
        });
        quitToMenu_BT.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.MainMenu);
        });
    }
    private void OnDestroy()
    {
        MultiplayerManager.Instance.OnJoinGameAttempt -= MultiplayerManager_OnJoinGameAttempt;
        MultiplayerManager.Instance.OnJoinGameAttemptFailed -= MultiplayerManager_OnJoinGameAttemptFailed;
    }
    private void MultiplayerManager_OnJoinGameAttemptFailed(object sender, System.EventArgs e)
    {
        connectingUI.Hide();
        connectionFailedUI.Show(NetworkManager.Singleton.DisconnectReason);
    }
    private void MultiplayerManager_OnJoinGameAttempt(object sender, System.EventArgs e)
    {
        Debug.Log("Joining...");
        connectingUI.Show();
    }
    public void ShowHelpText(string hoverString)
    {
        hoverText.text = hoverString;
    }
    public void HideHelpText()
    {
        hoverText.text = "";
    }
}
