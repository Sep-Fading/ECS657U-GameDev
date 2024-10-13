using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    public PlayerInput.GroundedActions grounded;
    private PlayerMotor playerMotor;
    private PlayerLook look;
    private Weaponmanager weaponMan;
    // Start is called before the first frame update
    void Awake()
    {
        playerInput = new PlayerInput();
        playerMotor = GetComponent<PlayerMotor>();
        look = GetComponent<PlayerLook>();

        grounded = playerInput.grounded;

        grounded.jumping.performed += playerMotor.Jump;
        grounded.sprinting.performed += playerMotor.Sprint;
        grounded.sprinting.canceled += playerMotor.Sprint;
        grounded.crouching.performed += playerMotor.Crouch;

        //grounded.SwordAction.performed += weaponMan.Attack;
        //grounded.SwordAction.canceled += weaponMan.Attack;
        //grounded.ShieldAction.performed += weaponMan.Block;
        //grounded.ShieldAction.canceled += weaponMan.Block;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    //getting the player to move based off of the inputs
    private void FixedUpdate()
    {
        playerMotor.ProcessMove(grounded.movement.ReadValue<Vector2>());
    }
    private void LateUpdate()
    {
        look.ProcessLook(grounded.looking.ReadValue<Vector2>());
    }

    // Update is called once per frame
    private void OnEnable()
    {
        grounded.Enable();
    }
    private void OnDisable()
    {
        grounded.Disable();
    }
}
