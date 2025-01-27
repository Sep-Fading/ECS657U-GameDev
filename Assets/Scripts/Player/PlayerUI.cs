using System;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Player
{
    /// <summary>
    /// Handles the player UI, such as
    /// Menu toggles and interact messages.
    /// </summary>
    public class PlayerUI : MonoBehaviour
    {
        private bool _uiActive;
        [SerializeField] private TextMeshProUGUI promptText;
        [FormerlySerializedAs("_playerSkillTreeManager")]
        [SerializeField] private PlayerSkillTreeManager playerSkillTreeManager;
        [FormerlySerializedAs("_inventoryUI")]
        [SerializeField] private GameObject inventoryUI;
        [FormerlySerializedAs("_StatUI")]
        [SerializeField] private GameObject statUI;
        [FormerlySerializedAs("_StatUIText")]
        [SerializeField] private GameObject statUIText;
        [FormerlySerializedAs("_optionsUIText")]
        [SerializeField] private GameObject optionsUI;
        // Start is called before the first frame update
        void Start()
        {
            playerSkillTreeManager = GetComponent<PlayerSkillTreeManager>();
            inventoryUI = GameObject.Find("-- Inventory UI");
            inventoryUI.SetActive(false);
            statUI.SetActive(false);
            optionsUI.SetActive(false); 
            
            UIManager.Initialize();
        }

        // Update is called once per frame
        public void UpdateText(string promptMessage)
        {
            promptText.text = promptMessage;
        }

        public void SkillTreeToggle(InputAction.CallbackContext context)
        {

            playerSkillTreeManager.skillTreeUI.SetActive(
                !playerSkillTreeManager.skillTreeUI.activeSelf);

            if (playerSkillTreeManager.skillTreeUI.activeSelf)
            {
                _uiActive = true;
                UIManager.Instance.PushUI(playerSkillTreeManager.skillTreeUI);
                //playerSkillTreeManager.PrintDebugConnections();
            }
            else
            {
                _uiActive = false;
                UIManager.Instance.PopUIByGameObject(playerSkillTreeManager.skillTreeUI);
            }
        }
        public bool GetUIActive()
        { 
            return _uiActive;
        }

        public void InventoryToggle(InputAction.CallbackContext context)
        { 
            inventoryUI.SetActive(
                !inventoryUI.activeSelf);
        
            if (inventoryUI.activeSelf)
            { 
                _uiActive = true; 
                UIManager.Instance.PushUI(inventoryUI);
            }
            else
            {
                _uiActive = false;
                UIManager.Instance.PopUIByGameObject(inventoryUI);
            }
        }

        public void StatMenuToggle(InputAction.CallbackContext context)
        {
            statUI.SetActive(
                !statUI.activeSelf);
            if (statUI.activeSelf)
            {
                _uiActive = true;
                UIManager.Instance.PushUI(statUI);
                statUIText.GetComponent<StatUpdater>().UpdateStatMenu();
            }

            else
            {
                _uiActive = false;
                UIManager.Instance.PopUIByGameObject(statUI);
            }
        }

        public void OptionsMenuToggle(InputAction.CallbackContext context)
        {
            
            optionsUI.SetActive(
                !optionsUI.activeSelf);
            if (optionsUI.activeSelf)
            {
                _uiActive = true;
                UIManager.Instance.PushUI(optionsUI);
            }

            else
            {
                _uiActive = false;
                UIManager.Instance.PopUIByGameObject(optionsUI);
            }
        }

        public void CloseMenuHandler(InputAction.CallbackContext obj)
        {
            if (!UIManager.Instance.GetIsEmpty())
            {
                _uiActive = true;
                UIManager.Instance.PopUI();
            }
            else
            {
                _uiActive = false;
                OptionsMenuToggle(obj);
            }
        }
    }
}
