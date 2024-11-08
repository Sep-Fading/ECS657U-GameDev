using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace InventoryScripts
{
    // A simple Click Handler for the inventory slots.
    // Allows us to equip items from our inventory.
    public class InventoryClickHandler : MonoBehaviour, IPointerClickHandler
    {
        private InventoryManager _inventoryManager;
        [SerializeField] private GameObject playerObject;
        [SerializeField] private int inventoryIndex;

        private void Start()
        {
            _inventoryManager = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<InventoryManager>();
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            InventoryItem item = Inventory.Instance.GetItemFromIndex(inventoryIndex);
            _inventoryManager.MoveToEquipment(item, inventoryIndex);
        }
    }
}