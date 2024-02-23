using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameComplete : MonoBehaviour
{
    [SerializeField] Button quit_BT;
    // Start is called before the first frame update
    void Start()
    {
        quit_BT.onClick.AddListener(() =>
        {
            MultiplayerManager.Instance.Shutdown();
            Loader.Load(Loader.Scene.MainMenu);
        });
    }
}
