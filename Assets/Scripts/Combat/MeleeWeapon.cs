using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
