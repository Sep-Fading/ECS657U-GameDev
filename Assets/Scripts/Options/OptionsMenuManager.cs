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
    
    [SerializeField] private Button exitButton;

    [SerializeField] private Button jumpButton;
    [SerializeField] private TextMeshProUGUI jumpButtonText;
    
    private PlayerInput _playerInput;

    public InputActionReference actionReference;
    private InputActionRebindingExtensions.RebindingOperation _rebindingOperation;


    private PlayerLook _playerLook;
    



    void Start()
    {
        _playerLook = player.GetComponent<PlayerLook>();
        
        _playerInput = player.GetComponent<InputManager>().getPlayerInput();
            
        jumpButton.onClick.AddListener(StartBinding);
        
        slider.onValueChanged.AddListener((v) =>
        {
            text.text = v.ToString("0.00");
            _playerLook.UpdateSensitivity(v);
        });

    }
    
    public void StartBinding()
    {
        InputAction action = _playerInput.grounded.jumping;
        action.Disable();
        jumpButtonText.text = "Awaiting Input";
        _rebindingOperation = action.PerformInteractiveRebinding().WithControlsExcluding("Mouse").OnMatchWaitForAnother(0.1f).OnComplete(operation => FinishRebinding()).Start();
        action.Enable();
    }
    

    public void FinishRebinding()
    {
        _rebindingOperation.Dispose();
        _rebindingOperation = null;
        
        string bindingPath = actionReference.action.bindings[0].effectivePath;
        string readablebinding = InputControlPath.ToHumanReadableString(bindingPath, InputControlPath.HumanReadableStringOptions.OmitDevice);
        
        jumpButtonText.text = readablebinding;
        player.GetComponent<InputManager>().RefreshBindings(_playerInput);
    }
    
}
