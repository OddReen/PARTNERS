using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Multiplayer;
using Unity.Netcode.Transports.UTP;
using TMPro;

public class ConectionManager : NetworkBehaviour
{

    [SerializeField] TMP_InputField ipv4Input;
    [SerializeField] TextMeshPro ipv4DisplayText;
    public void SetLanData()
    {
        UnityTransport transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        transport.SetConnectionData(ipv4Input.text, transport.ConnectionData.Port);
        ipv4DisplayText.text = "Current Ipv4 set to:" + ipv4Input.text;
    }
}
