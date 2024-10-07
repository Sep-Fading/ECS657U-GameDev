using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxInteract : Interactable
{
    // Start is called before the first frame update
    protected override void Interact()
    {
        Debug.Log("Interacted");
    }
}
