using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Used to dictate the features of Shields
/// </summary>
public abstract class Shield : MonoBehaviour
{
    public string shieldName { get; private set; }
    public bool canBlock { get; private set; }
    public bool canParry { get; private set; }
    public float parryWindow { get; private set; }
    public float blockEffectiveness { get; private set; }

    protected Shield(string name, float ParrywindowTime, float blockeffect)
    {
        shieldName = name;
        parryWindow = ParrywindowTime;
        blockEffectiveness = blockeffect;
    }
}
