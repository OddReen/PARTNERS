using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class MultiplayerUI : MonoBehaviour
{
    [SerializeField] private Button hostBT;
    [SerializeField] private Button serverBT;
    [SerializeField] private Button clientBT;
    private void Awake()
    {
        hostBT.onClick.AddListener(() => {
            NetworkManager.Singleton.StartHost();
        });
        serverBT.onClick.AddListener(() => {
            NetworkManager.Singleton.StartServer();
        });
        clientBT.onClick.AddListener(() => {
            NetworkManager.Singleton.StartClient();
        });
    }
}
