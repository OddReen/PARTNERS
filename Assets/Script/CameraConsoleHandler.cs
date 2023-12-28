using Unity.Netcode;
using UnityEngine;

public class CameraConsoleHandler : NetworkBehaviour
{
    //Isto não precisa de ser uma static Instance so que depois iamos ter que meter uma referencia a cada botão e isso é chato
    public static CameraConsoleHandler Instance;

    [SerializeField] GameObject[] cameraArray;

    GameObject activeCamera;

    int currentCamIndex;
    private void Start()
    {
        Instance = this;

        activeCamera = cameraArray[0];

        //Not necessary just a precaution you can delete if you want to
        foreach (GameObject camera in cameraArray)
        {
            camera.SetActive(false);

        }
        activeCamera.SetActive(true);
    }
    public void NextCam()
    {
        currentCamIndex++;
        if (currentCamIndex >= cameraArray.Length)
        {
            currentCamIndex = 0;
        }
        //Yes i know that this means im assigning the same value two times to currentCamIndex but i need to assign it to the other client and this is the easiest way
        ChangeCameraServerRpc(currentCamIndex);
    }
    public void PreviousCam()
    {
        currentCamIndex--;
        if (currentCamIndex <= -1)
        {
            currentCamIndex = cameraArray.Length - 1;
        }
        ChangeCameraServerRpc(currentCamIndex);
    }
    [ServerRpc(RequireOwnership = false)]
    public void ChangeCameraServerRpc(int cameraIndex)
    {
        ChangeCameraClientRpc(cameraIndex);
    }
    [ClientRpc]
    private void ChangeCameraClientRpc(int cameraIndex)
    {
        activeCamera.SetActive(false);
        activeCamera = cameraArray[cameraIndex];
        cameraArray[cameraIndex].SetActive(true);
        currentCamIndex = cameraIndex;
    }
}
