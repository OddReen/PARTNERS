using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;

public class SnapshotTest : MonoBehaviour
{
    const string cameraRoomSnapshot = "snapshot:/Camera_Room";
    const string corridorSnapshot = "snapshot:/Corridor";
    const string caveSnapshot = "snapshot:/Cave";
    EventInstance snapshot;

    private enum Snapshot
    {
        cameraRoomSnapshot,
        corridorSnapshot,
        SewerSnapshot
    }
    [SerializeField] Snapshot chooseSnapshot = Snapshot.cameraRoomSnapshot;
    Snapshot currentSnapshot = Snapshot.cameraRoomSnapshot;
    private void Start()
    {
        snapshot = RuntimeManager.CreateInstance(cameraRoomSnapshot);
    }
    private void Update()
    {
        if (currentSnapshot != chooseSnapshot)
        {
            currentSnapshot = chooseSnapshot;
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
        }
    }
}
