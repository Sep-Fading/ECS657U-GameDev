using UI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace InventoryScripts
{
    /// <summary>
    /// A simple Click Handler for the inventory slots.
    /// Allows us to equip items from our inventory.
    /// </summary>

    public class InventoryClickHandler : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        private InventoryManager _inventoryManager;
        [SerializeField] private GameObject playerObject;
        [SerializeField] private int inventoryIndex;
        [SerializeField] private TooltipUI _tooltipUI;

        private void Start()
        {
            _inventoryManager = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<InventoryManager>();
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            InventoryItem item = Inventory.Instance.GetItemFromIndex(inventoryIndex);
            if (item.gameItem.GetItemType() == ItemType.EQUIPMENT)
            {
                _inventoryManager.MoveToEquipment(item, inventoryIndex);
            }
            else if (item.gameItem.GetItemType() == ItemType.CONSUMABLE)
            {
                _inventoryManager.ConsumeItem(item, inventoryIndex);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            InventoryItem item = Inventory.Instance.GetItemFromIndex(inventoryIndex);
            if (item != null)
            {
                _tooltipUI.ShowTooltip(item.gameItem.GetName(), item.GetDescription(),
                    item.gameItem.GetSellPrice().ToString());
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _tooltipUI.HideTooltip();
        }
    }
}