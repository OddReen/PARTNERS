using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyManager_Singleplayer : MonoBehaviour
{
    public static EnergyManager_Singleplayer Instance;

    [SerializeField] Image energyBar;
    public float Energy { get { return Energy; } private set { Energy = value; } }

    [SerializeField] float MaxEnergy = 100f;

    [SerializeField] float overtimeDecay;
    
    private void Awake()
    {
        Instance = this;
        Energy = MaxEnergy;
        StartCoroutine(DecreaseEnergy());
    }
    IEnumerator DecreaseEnergy()
    {
        while (Energy > 0)
        {
            yield return new WaitForEndOfFrame();
            Energy -= overtimeDecay * Time.deltaTime;
            UpdateEnergyBar();
        }
    }
    private void UpdateEnergyBar()
    {
        energyBar.fillAmount = Energy / MaxEnergy;
    }
    public void ChangeEnergy_ServerRpc(float amount)
    {
        Energy = Mathf.Clamp(Energy + amount, 0, MaxEnergy);
        UpdateEnergyBar();
    }
    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
