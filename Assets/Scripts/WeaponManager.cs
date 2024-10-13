using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Weaponmanager : MonoBehaviour
{
    private Animator anim;
    private MeleeWeapon currentWeapon;

    private Shield currentShield;
    private bool attacking = false;
    private bool blocking = false;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {


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
    }

    public void Attack(InputAction.CallbackContext context)
    {
        attacking = !attacking;
        Debug.Log("Attacking");
    }

    public void Block(InputAction.CallbackContext context)
    {
        blocking = !blocking;
        Debug.Log("Blocking");
    }


    public void ChangeWeapon(MeleeWeapon newWeapon)
    {

    }
    public void ChangeShield(Shield newShield)
    {

    }

}
