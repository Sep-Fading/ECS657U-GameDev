using System.Collections;
using System.Collections.Generic;
using GameplayMechanics.Character;
using Player;
using UnityEngine;
using TMPro;
using UI;
using UnityEngine.InputSystem;

public class PlayerUI : MonoBehaviour
{
    private bool UIActive = false;
    [SerializeField] private TextMeshProUGUI promptText;
    [SerializeField] private PlayerSkillTreeManager _playerSkillTreeManager;
    [SerializeField] private GameObject _inventoryUI;
    [SerializeField] private GameObject _StatUI;
    [SerializeField] private GameObject _StatUIText;
    // Start is called before the first frame update
    void Start()
    {
        _playerSkillTreeManager = GetComponent<PlayerSkillTreeManager>();
        _inventoryUI = GameObject.Find("-- Inventory UI");
        _inventoryUI.SetActive(false);
        _StatUI.SetActive(false);
    }

    // Update is called once per frame
    public void UpdateText(string promptMessage)
    {
        promptText.text = promptMessage;
    }

    public void SkillTreeToggle(InputAction.CallbackContext context)
    {

        _playerSkillTreeManager._skillTreeUI.SetActive(
            !_playerSkillTreeManager._skillTreeUI.activeSelf);

        if (_playerSkillTreeManager._skillTreeUI.activeSelf)
        {
            UIActive = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }
        else
        {
            UIActive = false;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
    public bool GetUIActive()
    { 
        return UIActive;
    }

    public void InventoryToggle(InputAction.CallbackContext context)
    { 
        _inventoryUI.SetActive(
            !_inventoryUI.activeSelf);
        
        if (_inventoryUI.activeSelf)
        { 
            UIActive = true; 
            Cursor.visible = true; 
            Cursor.lockState = CursorLockMode.Confined;
        }
        else
        {
            UIActive = false;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void StatMenuToggle(InputAction.CallbackContext context)
    {
        _StatUI.SetActive(
            !_StatUI.activeSelf);
        if (_StatUI.activeSelf)
        {
            UIActive = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            _StatUIText.GetComponent<StatUpdater>().UpdateStatMenu();
        }

        else
        {
            UIActive = false;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
