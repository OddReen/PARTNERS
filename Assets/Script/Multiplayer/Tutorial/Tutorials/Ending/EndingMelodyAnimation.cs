using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingMelodyAnimation : MonoBehaviour
{
    CinemachineVirtualCamera animationCamera;

    [SerializeField] Transform melodyLookAtPosition;
    [SerializeField] GameObject EndingVignette;
    [SerializeField] GameObject blackOut;
    [SerializeField] GameObject melodyPrefab;
    [SerializeField] Quaternion melodyRotation;
    [SerializeField] GameObject gameOver;
    [SerializeField] GameObject melodyEyes;
    GameObject melody;
    float _cinemachineTargetPitch;
    const float _threshold = 0.01f;
    private float _rotationVelocity;
    public void EndingAnimation()
    {
        PlayerController_Multiplayer.OwnerInstance.isActive = false;
        PlayerController_Multiplayer.OwnerInstance.Freeze();
        animationCamera = PlayerController_Multiplayer.OwnerInstance.animationCamera;
        animationCamera.LookAt = melodyLookAtPosition;
        PlayerController_Multiplayer.OwnerInstance.cinemachineCamera.gameObject.SetActive(false);
        animationCamera.gameObject.SetActive(true);
        Invoke(nameof(StarVignete), 3f);
    }
    private void StarVignete()
    {
        StartCoroutine(MelodyKillScene());
    }
    IEnumerator MelodyKillScene()
    {
        EndingVignette.SetActive(true);
        yield return new WaitForSeconds(1f);
        blackOut.SetActive(true);
        melody = Instantiate(melodyPrefab,melodyLookAtPosition.position,melodyRotation);
        yield return new WaitForSeconds(0.5f);
        blackOut.SetActive(false);
        yield return new WaitForSeconds(1f);
        melodyEyes.SetActive(true);
        yield return new WaitForSeconds(5f);
        Cursor.lockState = CursorLockMode.Confined;
        gameOver.SetActive(true);
    }
}
