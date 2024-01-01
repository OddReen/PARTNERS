using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using Unity.Netcode;

public class SFX_Manager_Multiplayer : NetworkBehaviour
{
    //Script is cursed dont execute functions imediately after scene start or it wont work
    //\\Ja tentei mudar a ordem de execução still doesnt work o network usa uma diferente
    public static SFX_Manager_Multiplayer Instance;

    public void Awake()
    {
        Instance = this;
    }
    //Perguntar ao tales como fazer os soms serem globais
    [ServerRpc(RequireOwnership = false)]
    public void PlaySound_ServerRpc(string eventReferencePath)
    {
        PlaySound_ClientRpc( eventReferencePath);
    }
    [ClientRpc]
    private void PlaySound_ClientRpc(string eventReferencePath)
    {
        RuntimeManager.PlayOneShot(eventReferencePath);
    }
    [ServerRpc(RequireOwnership = false)]
    public void PlaySoundLocal_ServerRpc(string eventReferencePath,Vector3 position)
    {
        PlaySoundLocal_ClientRpc(eventReferencePath, position);
    }
    [ClientRpc]
    private void PlaySoundLocal_ClientRpc(string eventReferencePath,Vector3 position)
    {
        RuntimeManager.PlayOneShot(eventReferencePath,position);
    }
}
