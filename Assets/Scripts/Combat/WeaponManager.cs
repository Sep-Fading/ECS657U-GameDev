using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Weaponmanager : MonoBehaviour

{
    private static readonly int UIACTIVE = Animator.StringToHash("UIActive");
    private static readonly int Blocking = Animator.StringToHash("Blocking");
    private static readonly int Attacking = Animator.StringToHash("Attacking");

    private Animator anim;
    private MeleeWeapon currentWeapon;
    private Collider weaponCollider;
    private Shield currentShield;
    [SerializeField] private GameObject player;

    [SerializeField] private GameObject weaponPrefab;
    [SerializeField] private GameObject weaponColliderParent;
    

    private bool blocking = false;

    private void Awake()
    {
        anim = weaponPrefab.GetComponent<Animator>();
        
        weaponCollider = weaponColliderParent.GetComponent<Collider>();

    }

    public void Attack()
    {
        if (! player.GetComponent<PlayerUI>().GetUIActive())
        {
            anim.SetTrigger(Attacking);
        }
    }

    public void Block()
    {
        if (! player.GetComponent<PlayerUI>().GetUIActive())
        {
            blocking = !blocking;
            anim.SetBool(Blocking, blocking);
        }

    }

    public void onBlockCancelled()
    {
        if (!player.GetComponent<PlayerUI>().GetUIActive())
        {
            blocking = false;
            anim.SetBool(Blocking, blocking);
        }
    }


    public void ChangeWeapon(MeleeWeapon newWeapon)
    {

    }
    public void ChangeShield(Shield newShield)
    {

    }


}
