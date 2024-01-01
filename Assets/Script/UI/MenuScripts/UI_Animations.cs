using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UIAutoAnimation))]
public class UI_Animations : MonoBehaviour
{
    UIAutoAnimation anim;
    private void Awake()
    {
        anim = GetComponent<UIAutoAnimation>();
    }
    private void OnEnable()
    {
        anim.EntranceAnimation();
    }
}
