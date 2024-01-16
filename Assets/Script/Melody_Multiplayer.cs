using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class Melody_Multiplayer : NetworkBehaviour
{
    //WARNING: Isto é uma zona de guerra entre mim e o netcode dei delete a porrada de coisa que não estava
    //em uso a tentar matar um bug. Qualquer coisa temos a outra versão ou escrevese um novo codigo n tinha muita coisa que iriamos usar no final

    [SerializeField] float energyGain;

    [ServerRpc(RequireOwnership = false)]
    public void GainEnergy_ServerRpc()
    {
        EnergyManager_Multiplayer.Instance.ChangeEnergy_ServerRpc(energyGain);
    }
}
