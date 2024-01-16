using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSnapshotArea : MonoBehaviour
{
    [SerializeField] SnapshotManager.Snapshot snapshot;
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("PlayerExited");
        if (other.CompareTag("Player"))
        {
            SnapshotManager.Instance.ChangeSpapshot(snapshot);
        }
    }
}
