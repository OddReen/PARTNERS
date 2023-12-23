using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLight : MonoBehaviour
{
    [SerializeField] GameObject torch, energyLight;

    [SerializeField] Color blackOut, lightUp;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            if (energyLight.activeSelf)
            {
                RenderSettings.ambientLight = blackOut;
                torch.SetActive(true);
                energyLight.SetActive(false);
            }
            else
            {
                RenderSettings.ambientLight = lightUp;
                torch.SetActive(false);
                energyLight.SetActive(true);
            }
        }
    }
}
