using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public string HoverMessage;

    //all are templates to be overwritten by subclasses
    protected virtual void Interact(){} 

    public void BaseInteract()
    {
        Interact();
    }
}
