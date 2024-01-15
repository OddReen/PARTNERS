using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSnapshot_Interactable : Interactable
{
    [SerializeField] SnapshotManager.Snapshot snapshot;
    public override void Interact(PlayerController playerController)
    {
        SnapshotManager.Instance.ChangeSpapshot(snapshot);
    }
}
