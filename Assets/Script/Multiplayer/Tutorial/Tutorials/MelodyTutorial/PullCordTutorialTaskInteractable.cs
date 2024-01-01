using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullCordTutorialTaskInteractable : Interactable
{
    Vector3 defaultPosition;
    Quaternion defaultRotation;

    LineRenderer lineRenderer;

    [SerializeField] float pullDistance = 3;

    [SerializeField] InteractWithMelodyTask task;
    private void Start()
    {
        defaultPosition = transform.position;
        defaultRotation = transform.rotation;
    }
    public override void Interact(PlayerController playerController)
    {
        if (!task.isTaskActive)
        {
            return;
        }
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
        while (Vector3.Distance(defaultPosition, transform.position) < pullDistance && MultiplayerPlayerInput.OwnerInstance.isInteracting)
        {
            transform.position = MultiplayerPlayerController.OwnerInstance.grabPosition.position;
            transform.LookAt(defaultPosition);
            yield return null;
        }

        if (!MultiplayerPlayerInput.OwnerInstance.isInteracting)
        {
            transform.SetPositionAndRotation(defaultPosition, defaultRotation);
        }
        else
        {
            transform.SetPositionAndRotation(defaultPosition, defaultRotation);
            task.TaskCompleted();
        }
    }
}
