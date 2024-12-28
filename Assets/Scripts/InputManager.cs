using System;
using Combat;
using InventoryScripts;
using Player;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class InputManager : MonoBehaviour
{
    public PlayerInput PlayerInput;
    private PlayerMotor _playerMotor;
    private PlayerLook _look;
    [FormerlySerializedAs("_weaponmanager")]
    [SerializeField] private Weaponmanager weaponmanager;
    [FormerlySerializedAs("_playerUI")]
    [SerializeField] private PlayerUI playerUI;
    [FormerlySerializedAs("_pickupManager")]
    [SerializeField] private PickUpManager pickupManager;
    
    void Awake()
    {
        PlayerInput = new PlayerInput();
        _playerMotor = GetComponent<PlayerMotor>();
        _look = GetComponent<PlayerLook>();
        playerUI = GetComponent<PlayerUI>();
        weaponmanager = GetComponent<Weaponmanager>();
        pickupManager = GetComponent<PickUpManager>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        UIManager.Initialize();
    }
    
    private void OnEnable()
    {
        PlayerInput.Enable();
        PlayerInput.grounded.jumping.performed += _playerMotor.Jump;
        PlayerInput.grounded.sprinting.performed += _playerMotor.Sprint;
        PlayerInput.grounded.sprinting.canceled += _playerMotor.Sprint;
        PlayerInput.grounded.crouching.performed += _playerMotor.Crouch;
        PlayerInput.UI.OpenSkillTree.performed += playerUI.SkillTreeToggle;
        PlayerInput.UI.OpenInventory.performed += playerUI.InventoryToggle;
        PlayerInput.UI.ItemPickUp.performed += pickupManager.HandlePickUp;
        PlayerInput.UI.StatMenu.performed += playerUI.StatMenuToggle;
        PlayerInput.UI.OpenOptions.performed += playerUI.OptionsMenuToggle;
        PlayerInput.UI.CloseMenu.performed += playerUI.CloseMenuHandler;
        PlayerInput.grounded.SwordAction.performed += _ => weaponmanager.Attack();
        PlayerInput.grounded.ShieldAction.performed += _ => weaponmanager.Block();
        PlayerInput.grounded.ShieldAction.canceled += _ => weaponmanager.OnBlockCancelled();
    }
    private void OnDisable()
    {
        PlayerInput.grounded.jumping.performed -= _playerMotor.Jump;
        PlayerInput.grounded.sprinting.performed -= _playerMotor.Sprint;
        PlayerInput.grounded.sprinting.canceled -= _playerMotor.Sprint;
        PlayerInput.grounded.crouching.performed -= _playerMotor.Crouch;
        PlayerInput.UI.OpenSkillTree.performed -= playerUI.SkillTreeToggle;
        PlayerInput.UI.OpenSkillTree.performed -= playerUI.InventoryToggle;
        PlayerInput.UI.ItemPickUp.performed -= pickupManager.HandlePickUp;
        PlayerInput.UI.StatMenu.performed -= playerUI.StatMenuToggle;
        PlayerInput.UI.OpenOptions.performed -= playerUI.OptionsMenuToggle;
        PlayerInput.Disable();
    }

    //getting the player to move based off of the inputs
    private void FixedUpdate()
    {
        if (!GameStateManager.Instance.GetTransitionState())
        {
            _playerMotor.ProcessMove(PlayerInput.grounded.movement.ReadValue<Vector2>());
        }
        else
        {
            gameObject.transform.position = new Vector3(250f, 10f, 380f);
            GameStateManager.Instance.SetTransitionState(false);
            GameStateManager.Instance.MoveToNextScene("World-v0.2");
        }
    }
    private void LateUpdate()
    {
        if(UIManager.Instance.GetIsEmpty())
        {
            _look.ProcessLook(PlayerInput.grounded.looking.ReadValue<Vector2>());
        }
    }

    public void RefreshBindings(PlayerInput action)
    {
        PlayerInput.grounded.jumping.performed -= _playerMotor.Jump;
        PlayerInput.grounded.jumping.performed += _playerMotor.Jump;
    }

    public PlayerInput getPlayerInput()
    {
        return PlayerInput;
    }
}
