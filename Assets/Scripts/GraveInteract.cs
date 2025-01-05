using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraveInteract : Interactable
{
    public override void Interact()
    {
        GameObject.Find("EndCreditsCanvas").GetComponent<Canvas>().enabled = true;
    }
}
