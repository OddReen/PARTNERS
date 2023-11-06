using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetsReference : MonoBehaviour
{
    public static AssetsReference Instance;

   
    public GameObject interactionHint;
    private void Awake()
    {
        Instance = this;
    }
}
