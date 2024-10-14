using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Weaponmanager : MonoBehaviour

{
    private static readonly int Blocking = Animator.StringToHash("Blocking");
    private static readonly int Attacking = Animator.StringToHash("Attacking");
    private Animator anim;
    private MeleeWeapon currentWeapon;

    [SerializeField] private GameObject weaponPrefab;

    [SerializeField] private GameObject weaponColliderParent;
    private Collider weaponCollider;

    private Shield currentShield;
    private bool attacking = false;
    private bool blocking = false;

    private void Awake()
    {
        anim = weaponPrefab.GetComponent<Animator>();
        weaponCollider = weaponColliderParent.GetComponent<Collider>();
    }

    public void Attack()
    {
        anim.SetTrigger(Attacking);
    }

    public void Block()
    {
        blocking = !blocking;
        anim.SetBool(Blocking, blocking);
    }

    public void onBlockCancelled()
    {
        blocking = false;
        anim.SetBool(Blocking, blocking);
    }


    public void ChangeWeapon(MeleeWeapon newWeapon)
    {

    }
    public void ChangeShield(Shield newShield)
    {

    }


}
