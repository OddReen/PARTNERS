using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    Quaternion initialRotation;
    [SerializeField] bool isOpened;

    private void Start()
    {
        initialRotation = transform.rotation;
    }
    public void ExecuteAction()
    {
        if (isOpened)
        {
            isOpened = false;
            transform.rotation = Quaternion.Euler(initialRotation.x, initialRotation.y, initialRotation.z);
        }
        else
        {
            isOpened = true;
            transform.rotation = Quaternion.Euler(initialRotation.x, initialRotation.y + 90f, initialRotation.z);
        }
    }
}
