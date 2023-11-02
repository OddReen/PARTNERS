using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : Menu
{
    [SerializeField] Button singlePlayerPlay_BT;
    [SerializeField] Button LAN_BT;
    protected override void VirtualStart()
    {
        base.VirtualStart();
        singlePlayerPlay_BT.onClick.AddListener(() => {
            Loader.Load(Loader.Scene.Test);
        });
        LAN_BT.onClick.AddListener(() => {
            Loader.Load(Loader.Scene.MenuLAN);
        });
    }
}
