using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    [SerializeField] List<GameObject> lights;
    void Light(bool value)
    {
        for (int i = 0; i < lights.Count; i++)
        {
            lights[i].SetActive(value);
        }
    }
}
