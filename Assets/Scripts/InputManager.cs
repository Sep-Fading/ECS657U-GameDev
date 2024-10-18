using System.Collections;
using System.Collections.Generic;
using InventoryScripts;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public PlayerInput playerInput;
    private PlayerMotor playerMotor;
    private PlayerLook look;
    [SerializeField] private Weaponmanager _weaponmanager;
    [SerializeField] private PlayerUI _playerUI;
    [SerializeField] private PickUpManager _pickupManager;
    
    void Awake()
    {
        playerInput = new PlayerInput();
        playerMotor = GetComponent<PlayerMotor>();
        look = GetComponent<PlayerLook>();
        _playerUI = GetComponent<PlayerUI>();
        _weaponmanager = GetComponent<Weaponmanager>();
        _pickupManager = GetComponent<PickUpManager>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    
    private void OnEnable()
    {
        playerInput.Enable();
        playerInput.grounded.jumping.performed += playerMotor.Jump;
        playerInput.grounded.sprinting.performed += playerMotor.Sprint;
        playerInput.grounded.sprinting.canceled += playerMotor.Sprint;
        playerInput.grounded.crouching.performed += playerMotor.Crouch;
        playerInput.UI.OpenSkillTree.performed += _playerUI.SkillTreeToggle;
        playerInput.UI.OpenInventory.performed += _playerUI.InventoryToggle;
        playerInput.UI.ItemPickUp.performed += _pickupManager.HandlePickUp;
        playerInput.grounded.SwordAction.performed += i => _weaponmanager.Attack();
        playerInput.grounded.ShieldAction.performed += i => _weaponmanager.Block();
        playerInput.grounded.ShieldAction.canceled += i => _weaponmanager.onBlockCancelled();
    }
    private void OnDisable()
    {
        playerInput.grounded.jumping.performed -= playerMotor.Jump;
        playerInput.grounded.sprinting.performed -= playerMotor.Sprint;
        playerInput.grounded.sprinting.canceled -= playerMotor.Sprint;
        playerInput.grounded.crouching.performed -= playerMotor.Crouch;
        playerInput.UI.OpenSkillTree.performed -= _playerUI.SkillTreeToggle;
        playerInput.UI.OpenSkillTree.performed -= _playerUI.InventoryToggle;
        playerInput.UI.ItemPickUp.performed -= _pickupManager.HandlePickUp;
        playerInput.Disable();
    }

    //getting the player to move based off of the inputs
    private void FixedUpdate()
    {
        playerMotor.ProcessMove(playerInput.grounded.movement.ReadValue<Vector2>());
    }
    private void LateUpdate()
    {
        look.ProcessLook(playerInput.grounded.looking.ReadValue<Vector2>());
    }
}
