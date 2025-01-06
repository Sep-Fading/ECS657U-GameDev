using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    /// <summary>
    /// Handles how Interactable objects work
    /// all are templates to be overwritten by subclasses
    /// </summary>
    public string HoverMessage;

    
    public virtual void Interact(){} 

    public void BaseInteract()
    {
        Interact();
    }
}
