using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapshotManager : MonoBehaviour
{
    public static SnapshotManager Instance;
    const string cameraRoomSnapshot = "snapshot:/Camera_Room";
    const string corridorSnapshot = "snapshot:/Corridor";
    const string caveSnapshot = "snapshot:/Cave";
    EventInstance snapshot;
    public enum Snapshot
    {
        cameraRoomSnapshot,
        corridorSnapshot,
        SewerSnapshot
    }
    private Snapshot currentSnapshot;
    private void Awake()
    {
        Instance = this;
        snapshot = RuntimeManager.CreateInstance(corridorSnapshot);
        snapshot.start();
    }
    public void ChangeSpapshot(Snapshot choosenSnapshot)
    {
        if (currentSnapshot != choosenSnapshot)
        {
            currentSnapshot = choosenSnapshot;
            switch (currentSnapshot)
            {
                case Snapshot.cameraRoomSnapshot:
                    snapshot.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                    snapshot.release();
                    snapshot = RuntimeManager.CreateInstance(cameraRoomSnapshot);
                    snapshot.start();
                    break;
                case Snapshot.corridorSnapshot:
                    snapshot.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                    snapshot.release();
                    snapshot = RuntimeManager.CreateInstance(corridorSnapshot);
                    snapshot.start();
                    break;
                case Snapshot.SewerSnapshot:
                    snapshot.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                    snapshot.release();
                    snapshot = RuntimeManager.CreateInstance(caveSnapshot);
                    snapshot.start();
                    break;
                default:
                    break;
            }
            Debug.Log($"Current Snapshot:{currentSnapshot}");
        }
    }

}
