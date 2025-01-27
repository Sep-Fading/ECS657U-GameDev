using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to dictate the features of weapons
/// </summary>
public abstract class MeleeWeapon : MonoBehaviour
{
    public string weaponName { get; private set; }
    public float attackDistance { get; private set; }
    public bool attacking { get; private set; }
    public float attackSpeed { get; private set; }
    public float attackDamage { get; private set; }
    public LayerMask attackLayer;

    
    protected MeleeWeapon(string name, float distance, float speed, float damage)
    {
        weaponName = name;
        attackDistance = distance;
        attackSpeed = speed;
        attackDamage = damage;
    }

    public virtual void StartAttack() { }
    public virtual void StopAttack() { }
}
