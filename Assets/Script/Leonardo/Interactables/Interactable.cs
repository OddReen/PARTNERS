using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
[RequireComponent(typeof(Outline))]
public class Interactable : MonoBehaviour
{
    Outline _outline;
    private void Awake()
    {
        _outline = GetComponent<Outline>();
        _outline.OutlineWidth = 10;
        _outline.OutlineColor = Color.green;
        InteractHint(false);
    }
    public virtual void Interact(PlayerController playerController)
    {
    }
    public void InteractHint(bool enable)
    {
        _outline.enabled = enable;
    }
}
