using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using TMPro;

public class LAN : MonoBehaviour
{
    [Header("Join Lan")]
    [SerializeField] TMP_InputField ipv4Input;
    [SerializeField] TextMeshProUGUI ipv4DisplayText;
    public void SetLanData()
    {
        UnityTransport transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        transport.SetConnectionData(ipv4Input.text, transport.ConnectionData.Port);
        ipv4DisplayText.text = "Current Ipv4 set to:" + ipv4Input.text;
    }
    public void JoinClient() {
        NetworkManager.Singleton.StartClient();
    }
    public void JoinHost()
    {
        NetworkManager.Singleton.StartHost();
    }
}
