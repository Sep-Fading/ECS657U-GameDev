using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalInteract : Interactable
{
    string[] scenes = { "Scenes/Tutorial", "Scenes/World-v0.1", "Scenes/World-v0.2", "Scenes/World-v0.3" };
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
