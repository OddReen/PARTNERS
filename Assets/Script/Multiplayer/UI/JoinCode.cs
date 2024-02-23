using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class JoinCode : MonoBehaviour
{
    public static JoinCode Instance;

    [SerializeField] TextMeshProUGUI text;
    private void Awake()
    {
        if (MultiplayerManager.Instance.joinCode != "")
        {
            text.text = $"Join Code:{MultiplayerManager.Instance.joinCode}";
        }
        else
        {
            text.gameObject.SetActive(false);
        }
    }
}
