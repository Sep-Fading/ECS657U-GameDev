using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using TMPro;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;

public class OptionsMenuManagerMenu : MonoBehaviour
{
        
    [SerializeField] private GameObject settingMenu;
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private GameObject player;
    
    [SerializeField] private TextMeshProUGUI helpText;
    
    [SerializeField] private Button jumpButton;
    [SerializeField] private TextMeshProUGUI jumpButtonText;
    
    [SerializeField] private Button crouchButton;
    [SerializeField] private TextMeshProUGUI crouchButtonText;
    
    [SerializeField] private Button sprintButton;
    [SerializeField] private TextMeshProUGUI sprintButtonText;
    
    [SerializeField] private Button interactButton;
    [SerializeField] private TextMeshProUGUI interactButtonText;
    
    [SerializeField] private Button pickupButton;
    [SerializeField] private TextMeshProUGUI pickupButtonText;
    
    [SerializeField] private Button skillTreeButton;
    [SerializeField] private TextMeshProUGUI skillTreeButtonText;
    
    [SerializeField] private Button inventoryButton;
    [SerializeField] private TextMeshProUGUI inventoryButtonText;
    
    [SerializeField] private Button statMenuButton;
    [SerializeField] private TextMeshProUGUI statMenuButtonText;
    
    
    
    private PlayerInput _playerInput;

    public InputActionReference jumpActionReference;
    public InputActionReference crouchActionReference;
    public InputActionReference sprintActionReference;
    public InputActionReference interactActionReference;
    public InputActionReference pickupActionReference;
    public InputActionReference skillTreeActionReference;
    public InputActionReference inventoryActionReference;
    public InputActionReference statMenuActionReference;
    
    
    private InputActionReference currentActionReference;
    
    private InputActionRebindingExtensions.RebindingOperation _rebindingOperation;
    
    private PlayerLook _playerLook;
    

    public enum ButtonBind
    {
        JUMP,
        CROUCH,
        SPRINT,
        INTERACT,       
        PICKUP,
        SKILLTREE,
        INVENTORY,
        STATMENU,
        NONE
    }       

    void Start()
    {
        _playerLook = player.GetComponent<PlayerLook>();
        
        _playerInput = player.GetComponent<InputManager>().getPlayerInput();
        
        jumpButton.onClick.AddListener(() => StartBinding(ButtonBind.JUMP));
        crouchButton.onClick.AddListener(() => StartBinding(ButtonBind.CROUCH));
        sprintButton.onClick.AddListener(() => StartBinding(ButtonBind.SPRINT));
        interactButton.onClick.AddListener(() => StartBinding(ButtonBind.INTERACT));
        pickupButton.onClick.AddListener(() => StartBinding(ButtonBind.PICKUP));
        skillTreeButton.onClick.AddListener(() => StartBinding(ButtonBind.SKILLTREE));
        inventoryButton.onClick.AddListener(() => StartBinding(ButtonBind.INVENTORY));
        statMenuButton.onClick.AddListener(() => StartBinding(ButtonBind.STATMENU));

        jumpButtonText.text = KeyOnly(_playerInput.grounded.jumping.bindings[0].effectivePath);
        crouchButtonText.text = KeyOnly(_playerInput.grounded.crouching.bindings[0].effectivePath);
        sprintButtonText.text = KeyOnly(_playerInput.grounded.sprinting.bindings[0].effectivePath);
        interactButtonText.text = KeyOnly(_playerInput.grounded.Interacting.bindings[0].effectivePath);
        pickupButtonText.text = KeyOnly(_playerInput.UI.ItemPickUp.bindings[0].effectivePath);
        skillTreeButtonText.text = KeyOnly(_playerInput.UI.OpenSkillTree.bindings[0].effectivePath);
        inventoryButtonText.text = KeyOnly(_playerInput.UI.OpenInventory.bindings[0].effectivePath);
        statMenuButtonText.text = KeyOnly(_playerInput.UI.StatMenu.bindings[0].effectivePath);
        
        
        
        slider.onValueChanged.AddListener((v) =>
        {
            text.text = v.ToString("0.00");
            _playerLook.UpdateSensitivity(v);
        });

    }
    
    public void StartBinding(ButtonBind activeButton)
    {
        InputAction action = null;
        SetAll(false);
        switch (activeButton)
        {
            case ButtonBind.JUMP: 
                action = _playerInput.grounded.jumping;
                jumpButtonText.text = "Awaiting Input";
                currentActionReference = jumpActionReference;
                break;
            case ButtonBind.CROUCH:
                action = _playerInput.grounded.crouching;
                crouchButtonText.text = "Awaiting Input";
                currentActionReference = crouchActionReference;
                break;
            case ButtonBind.SPRINT:
                action = _playerInput.grounded.sprinting;
                sprintButtonText.text = "Awaiting Input";
                currentActionReference = sprintActionReference;
                break;
            case ButtonBind.INTERACT:
                action = _playerInput.grounded.Interacting;
                interactButtonText.text = "Awaiting Input";
                currentActionReference = interactActionReference;
                break;
            case ButtonBind.PICKUP:
                action = _playerInput.UI.ItemPickUp;
                pickupButtonText.text = "Awaiting Input";
                currentActionReference = pickupActionReference;
                break;
            case ButtonBind.SKILLTREE:
                action = _playerInput.UI.OpenSkillTree;
                skillTreeButtonText.text = "Awaiting Input";
                currentActionReference = skillTreeActionReference;
                break;
            case ButtonBind.INVENTORY:
                action = _playerInput.UI.OpenInventory;
                inventoryButtonText.text = "Awaiting Input";
                currentActionReference = inventoryActionReference;
                break;
            case ButtonBind.STATMENU:
                action = _playerInput.UI.StatMenu;
                statMenuButtonText.text = "Awaiting Input";
                currentActionReference = statMenuActionReference;
                break;
            
        }

        if (action != null)
        {
            action.Disable();
            _rebindingOperation = action.PerformInteractiveRebinding().WithControlsExcluding("Mouse");
            foreach (string activeKey in GetActiveKeys())
            {
                //Debug.Log(activeKey);
                _rebindingOperation.WithControlsExcluding(activeKey);
            }
            _rebindingOperation.OnMatchWaitForAnother(0.1f).OnPotentialMatch(operation => 
                {
                    var binding = NormalizePath(operation.selectedControl.name);
                    
                    //Debug.Log(binding);
                    
                    // Check if the binding is already used
                    if (GetActiveKeys().Contains(binding) || binding == "<keyboard>/anyKey")
                    {
                        helpText.text = "Input is already bound to another action.";
                        operation.Cancel();
                        IncompleteButton(activeButton);
                    }
                })
                .OnMatchWaitForAnother(0.1f)
                .OnComplete(operation =>
                {
                    FinishRebinding(activeButton); 
                    action.Enable();              
                })
                .Start();
                //FinishRebinding(activeButton)).Start();
        }
    }
    string NormalizePath(string controlPath)
    {
        //Debug.Log(controlPath);
        // Convert "Key:/Keyboard/q" to "<keyboard>/q"
        //different pathing notation caused errors in comparing
        return $"<keyboard>/{controlPath}";
        
    }

    public void FinishRebinding(ButtonBind activeButton)
    {
        
        string bindingPath = _rebindingOperation.action.bindings[0].effectivePath;
        
        
        _rebindingOperation.Dispose();
        _rebindingOperation = null;
        
        string readablebinding = InputControlPath.ToHumanReadableString(bindingPath, InputControlPath.HumanReadableStringOptions.OmitDevice);
        
        switch (activeButton)
        {
            case ButtonBind.JUMP: 
                jumpButtonText.text = readablebinding;
                break;
            case ButtonBind.CROUCH:
                crouchButtonText.text = readablebinding;
                break;
            case ButtonBind.SPRINT:
                sprintButtonText.text = readablebinding;
                break;
            case ButtonBind.INTERACT:
                interactButtonText.text = readablebinding;
                break;
            case ButtonBind.PICKUP:
                pickupButtonText.text = readablebinding;
                break;
            case ButtonBind.SKILLTREE:
                skillTreeButtonText.text = readablebinding;
                break;
            case ButtonBind.INVENTORY:
                inventoryButtonText.text = readablebinding;
                break;
            case ButtonBind.STATMENU:
                statMenuButtonText.text = readablebinding;
                break;
        }
        SetAll(true);
        helpText.text = "";
    }

    private void IncompleteButton(ButtonBind activeButton)
    {
        _rebindingOperation.Dispose();
        _rebindingOperation = null;
        
        switch (activeButton)
        {
            case ButtonBind.JUMP: 
                jumpButtonText.text = "Unbound";
                break;
            case ButtonBind.CROUCH:
                crouchButtonText.text = "Unbound";
                break;
            case ButtonBind.SPRINT:
                sprintButtonText.text = "Unbound";
                break;
            case ButtonBind.INTERACT:
                interactButtonText.text = "Unbound";
                break;
            case ButtonBind.PICKUP:
                pickupButtonText.text = "Unbound";
                break;
            case ButtonBind.SKILLTREE:
                skillTreeButtonText.text = "Unbound";
                break;
            case ButtonBind.INVENTORY:
                inventoryButtonText.text = "Unbound";
                break;
            case ButtonBind.STATMENU:
                statMenuButtonText.text = "Unbound";
                break;
        }

        SetAll(true);

    }
    private string KeyOnly(string effectivePath)
    {
        
        string[] parts = effectivePath.Split('/'); // get input after the / part
        return parts[parts.Length - 1];
    }

    private List<string> GetActiveKeys()
    {
        player.GetComponent<InputManager>().RefreshBindings(_playerInput);
        List<string> keys = new List<string>();
        foreach (var actionMap in _playerInput)
        {
            //Debug.Log($"Action Map: {actionMap.name}");
            foreach (var action in actionMap.actionMap.actions)
            {
                //Debug.Log($" Action: {action.name}");
                // Get all bindings for this action
                foreach (var binding in action.bindings)
                {
                    //Debug.Log($" Binding: {binding.effectivePath}");
                    keys.Add(binding.effectivePath);
                }
            }
        }
        return keys;
    }

    private void SetAll(bool state)
    {
        jumpButton.enabled = state;
        crouchButton.enabled = state;
        sprintButton.enabled = state;
        interactButton.enabled = state;
        pickupButton.enabled = state;
        skillTreeButton.enabled = state;
        inventoryButton.enabled = state;
        statMenuButton.enabled = state;
    }
}
