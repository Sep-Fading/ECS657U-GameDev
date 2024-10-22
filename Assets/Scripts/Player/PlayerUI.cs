using TMPro;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Player
{
    // Handles the player UI, such as
    // Menu toggles and interact messages.
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
        // Start is called before the first frame update
        void Start()
        {
            playerSkillTreeManager = GetComponent<PlayerSkillTreeManager>();
            inventoryUI = GameObject.Find("-- Inventory UI");
            inventoryUI.SetActive(false);
            statUI.SetActive(false);
        }

        // Update is called once per frame
        public void UpdateText(string promptMessage)
        {
            promptText.text = promptMessage;
        }

        public void SkillTreeToggle(InputAction.CallbackContext context)
        {

            playerSkillTreeManager._skillTreeUI.SetActive(
                !playerSkillTreeManager._skillTreeUI.activeSelf);

            if (playerSkillTreeManager._skillTreeUI.activeSelf)
            {
                _uiActive = true;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
            }
            else
            {
                _uiActive = false;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
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
                Cursor.visible = true; 
                Cursor.lockState = CursorLockMode.Confined;
            }
            else
            {
                _uiActive = false;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        public void StatMenuToggle(InputAction.CallbackContext context)
        {
            statUI.SetActive(
                !statUI.activeSelf);
            if (statUI.activeSelf)
            {
                _uiActive = true;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
                statUIText.GetComponent<StatUpdater>().UpdateStatMenu();
            }

            else
            {
                _uiActive = false;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }
}
