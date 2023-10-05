using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraConsole : MonoBehaviour
{
    [SerializeField] Camera[] cctv;

    [SerializeField] int currentCameraIndex = 0;

    private void Start()
    {
        if (cctv.Length > 0)
        {
            cctv[currentCameraIndex].gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            cctv[currentCameraIndex].gameObject.SetActive(false);

            currentCameraIndex = (currentCameraIndex + 1) % cctv.Length;

            cctv[currentCameraIndex].gameObject.SetActive(true);

        }
    }
}
