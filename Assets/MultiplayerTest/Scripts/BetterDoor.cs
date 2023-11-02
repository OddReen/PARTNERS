using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterDoor : Interactable
{

    Quaternion initialRotation;
    [SerializeField] bool isOpened = false;

    private void Start()
    {
        initialRotation = transform.rotation;
    }
    public override void Interact()
    {
        isOpened = !isOpened;
        if (isOpened)
        {
           transform.rotation = Quaternion.Euler(initialRotation.x, initialRotation.y + 90f, initialRotation.z);
        }
        else
        { 
            transform.rotation = Quaternion.Euler(initialRotation.x, initialRotation.y, initialRotation.z);  
        }
    }
}
