using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalInteract : Interactable
{

    // Start is called before the first frame update
    void Start()
    {
        HoverMessage = "Are You Ready to Move On [F]";
    }
    public override void Interact()
    {
        GameStateManager.Instance.SetTransitionState(true);
    }
}
