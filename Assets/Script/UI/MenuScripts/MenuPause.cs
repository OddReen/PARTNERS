using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class MenuPause : Menu
{
    [SerializeField] Button quit_BT;
    [SerializeField] Button resume_BT;
    private void Start()
    {
        VirtualStart();
    }
    protected override void VirtualStart()
    {
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
        base.VirtualStart();
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }
}
