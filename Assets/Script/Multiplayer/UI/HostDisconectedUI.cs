using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class HostDisconectedUI : MonoBehaviour
{
    [SerializeField] Button quit_BT;
    private void Start()
    {
        NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectCallback;
        quit_BT.onClick.AddListener(() =>
        {
            Time.timeScale = 1f;
            Loader.Load(Loader.Scene.MainMenu);
        });

        Hide();
    }


    //Se  o client que se desconectou for o Host mostra essa mensagem
    private void NetworkManager_OnClientDisconnectCallback(ulong clientId)
    {
        Debug.Log("Test");
        if (clientId == NetworkManager.ServerClientId)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Time.timeScale = 0f;
            Show();
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
    private void OnDestroy()
    {
        //Isto é so necessario para corrigir um erro do editor na build o NetworkManager é sempre destroido depois desta verificação
        if (NetworkManager.Singleton != null)
        {   
            NetworkManager.Singleton.OnClientDisconnectCallback -= NetworkManager_OnClientDisconnectCallback;
        }

    }
}
