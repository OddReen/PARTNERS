using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Netcode;

public class ConnectionFailedUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI messageTxt;
    [SerializeField] Button close_BT;

    private void Awake()
    {
        close_BT.onClick.AddListener(Hide);
    }
    private void Start()
    {
        Hide();
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
    public void Show(string message)
    {
        gameObject.SetActive(true);
        messageTxt.text = NetworkManager.Singleton.DisconnectReason;

        if (messageTxt.text == "")
        {
            messageTxt.text = "Connection Failed";
        }
    }
}
