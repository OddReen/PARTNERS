using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeAmbienceTest_Interactable : Interactable
{
    [SerializeField] GameObject ambienceObject;

    public override void Interact(PlayerController playerController)
    {
        ambienceObject.SetActive(!ambienceObject.activeInHierarchy);
    }
}
