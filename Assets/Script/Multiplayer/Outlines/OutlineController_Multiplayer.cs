using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

[RequireComponent(typeof(Outline))]
public class OutlineController_Multiplayer : NetworkBehaviour
{
    Outline _outline;

    void Awake()
    {
        _outline = GetComponent<Outline>();
        _outline.enabled = false;
    }

    [ServerRpc(RequireOwnership = false)]
    public void ActivateOutline_ServerRpc(bool isActive)
    {
        ActivateOutline_ClientRpc(isActive);
    }
    [ClientRpc]
    private void ActivateOutline_ClientRpc(bool isActive)
    {
        _outline.enabled = isActive;
    }

    [ServerRpc(RequireOwnership = false)]
    public void StartOutlineTimer_ServerRpc(float time)
    {
        StartCoroutine(OutlineTimer(time));
    }
    //Isto coroutina so deve ser executada server side visto conter client Rpcs
    IEnumerator OutlineTimer(float time)
    {
        ActivateOutline_ClientRpc(true);
        yield return new WaitForSeconds(time);
        ActivateOutline_ClientRpc(false);
    }


}
