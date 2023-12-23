using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deliver : Interactable
{
    Rigidbody _rb;
    Collider _collider;
    [SerializeField] Transform deliveryDestination;

    private void Start()
    {
        _collider = GetComponent<Collider>();
        _rb = GetComponent<Rigidbody>();
    }
    public override void Interact(PlayerController playerController)
    {
        StartCoroutine(PickUp(playerController));
    }
    IEnumerator PickUp(PlayerController playerController)
    {
        _collider.enabled = false;
        _rb.isKinematic = true;
        while (playerController.playerInput.isInteracting)
        {
            transform.position = playerController.grabPos.position;
            transform.LookAt(playerController.grabPos);
            yield return null;
        }
        Destination();
        _collider.enabled = true;
        _rb.isKinematic = false;
    }
    void Destination()
    {
        if (Vector3.Distance(deliveryDestination.position, transform.position) < 1)
        {
            TaskSucceed();
            Destroy(gameObject);
        }
    }
    void TaskSucceed()
    {
        Manager_SFX.Instance.win.start();
        Debug.Log("Task Succeed");
    }
}
