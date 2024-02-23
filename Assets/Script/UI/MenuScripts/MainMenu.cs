using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : Menu
{
    [SerializeField] Button LAN_BT;
    [SerializeField] Button ONLINE_BT;
    protected override void VirtualStart()
    {
        base.VirtualStart();
        LAN_BT.onClick.AddListener(() => {
            Loader.Load(Loader.Scene.MenuLAN);
        });
        ONLINE_BT.onClick.AddListener(() => {
            Loader.Load(Loader.Scene.MenuRelay);
        });
    }
}
