using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MelodyInteractable_Multiplayer : Interactable
{
    Vector3 defaultPosition;
    Quaternion defaultRotation;

    LineRenderer lineRenderer;

    [SerializeField] float pullDistance = 3;

    [SerializeField] Melody_Multiplayer melody;

    private void Start()
    {
        defaultPosition = transform.position;
        defaultRotation = transform.rotation;
    }
    public override void Interact(PlayerController playerController)
    {
        StartCoroutine(CordPull());
    }
    private void LateUpdate()
    {
        DrawLine();
    }
    void DrawLine()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, defaultPosition);
        lineRenderer.SetPosition(1, transform.position);
    }
    IEnumerator CordPull()
    {
        while (Vector3.Distance(defaultPosition, transform.position) < pullDistance && PlayerInput_Multiplayer.OwnerInstance.isInteracting)
        {
            transform.position = PlayerController_Multiplayer.OwnerInstance.grabPosition.position;
            transform.LookAt(defaultPosition);
            yield return null;
        }

        if (!PlayerInput_Multiplayer.OwnerInstance.isInteracting)
        {
            transform.SetPositionAndRotation(defaultPosition, defaultRotation);
        }
        else
        {
            transform.SetPositionAndRotation(defaultPosition, defaultRotation);
            melody.GainEnergy_ServerRpc();
        }
    }
}
