using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Shield : MonoBehaviour
{
    private bool canBlock;
    private bool canParry;
    private float parryWindow;
    private float blockEffectiveness;




    ////////////// GETTERS AND SETTERS/////////////////
    public bool GetCanBlock()
    {
        return canBlock;
    }
    public void SetCanBlock(bool state)
    {
        canBlock = state;
    }

    public bool GetCanParry()
    {
        return canParry;
    }
    public void SetCanParry(bool state)
    {
        canParry = state;
    }

    public float GetParryWindow()
    {
        return parryWindow;
    }
    public void SetParryWindow(float value)
    {   
        parryWindow = value;
    }

    public float GetBlockEffectiveness()
    {
        return blockEffectiveness;
    }
    public void Set(float value)
    {
        blockEffectiveness = value;
    }

}
