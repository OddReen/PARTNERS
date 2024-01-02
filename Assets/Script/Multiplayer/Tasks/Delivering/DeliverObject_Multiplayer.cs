using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliverObject_Multiplayer : Interactable
{
    Rigidbody _rb;
    Collider _collider;
    Transform deliveryDestination;
    [SerializeField] SFX_List sFX_List;
    public void AssignDeliveryDestination(Transform deliveryLocation)
    {
        deliveryDestination = deliveryLocation;
    }
    private void Start()
    {
        _collider = GetComponent<Collider>();
        _rb = GetComponent<Rigidbody>();
    }
    public override void Interact(PlayerController playerController)
    {
        SFX_Manager_Multiplayer.Instance.PlaySoundLocal_ServerRpc(sFX_List.PickupPaper.Path,transform.position);
        StartCoroutine(PickUp());
    }
    IEnumerator PickUp()
    {
        _collider.enabled = false;
        _rb.isKinematic = true;
        //Change to grabpos being inside playerIput
        MultiplayerPlayerInput _input = MultiplayerPlayerInput.OwnerInstance;
        MultiplayerPlayerController _controller = MultiplayerPlayerController.OwnerInstance;
        while (_input.isInteracting)
        {
            transform.position = _controller.grabPosition.position;
            transform.LookAt(_controller.grabPosition);
            yield return new WaitForEndOfFrame();
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
        DeliveringTask_Multiplayer.Instance.Deliver();
    }
}
