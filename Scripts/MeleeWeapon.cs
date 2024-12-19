using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MeleeWeapon : MonoBehaviour
{
    private string weaponName;
    private float attackDistance;
    private bool attacking;
    private float attackSpeed;
    private float attackDamage;
    public LayerMask attackLayer;






////////////// GETTERS AND SETTERS/////////////////
    public string GetWeaponName()
    {
        return weaponName;
    }
    private void SetWeaponName(string name)
    {
        weaponName = name;
    }

    public float GetAttackDistance()
    {
        return attackDistance;
    }
    public void SetAttackDistance(float distance)
    {
        attackDistance = distance;
    }

    public bool GetAttackingState()
    {
        return attacking;
    }
    public void SetAttackingState(bool state)
    {
        attacking = state;
    }

    public float GetAttackSpeed()
    {
        return attackSpeed;
    }
    public void SetAttackSpeed(float seconds)
    {
        attackSpeed = seconds;
    }

    public float GetAttackDamage()
    {
        return attackDamage;
    }
    public void SetAttackDamage(float damage)
    {
        attackDamage = damage;
    }

}
