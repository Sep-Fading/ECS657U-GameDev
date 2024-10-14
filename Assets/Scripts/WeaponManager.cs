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

    private Shield currentShield;
    private bool attacking = false;
    private bool blocking = false;

    private void Awake()
    {
        anim = weaponPrefab.GetComponent<Animator>();
    }

    private void Update()
    {

/*
        if (Input.GetMouseButtonDown(0))
        {
            anim.SetBool("Attacking", true);
        }
        else if (Input.GetMouseButtonDown(1))
        {
            blocking = !blocking;
            anim.SetBool("Blocking", blocking);
        }
        else
        {
            anim.SetBool("Attacking", false);
        }
*/
    }

    public void Attack()
    {
        attacking = !attacking;
        anim.SetBool("Attacking", attacking);
        Debug.Log("Attacking");
    }

    public void Block()
    {
        blocking = !blocking;
        anim.SetBool("Blocking", blocking);
        Debug.Log("Blocking");
    }


    public void ChangeWeapon(MeleeWeapon newWeapon)
    {

    }
    public void ChangeShield(Shield newShield)
    {

    }

}
