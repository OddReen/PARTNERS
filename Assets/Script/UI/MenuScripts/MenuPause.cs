using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class MenuPause : Menu
{
    [SerializeField] Button quit_BT;
    [SerializeField] Button resume_BT;
    protected override void VirtualStart()
    {
        base.VirtualStart();
        quit_BT.onClick.AddListener(() =>
        {
            MultiplayerManager.Instance.Shutdown();
            Loader.Load(Loader.Scene.MainMenu);
        });
        resume_BT.onClick.AddListener(() =>
        {
            InGameManager.Instance.TogglePauseGame();
        });
        Hide();
    }
    public void Hide()
    {
        Debug.Log("Hide PauseMenu");
        gameObject.SetActive(false);
    }
    public void Show()
    {
        Debug.Log("Show PauseMenu");
        gameObject.SetActive(true);
    }
}
