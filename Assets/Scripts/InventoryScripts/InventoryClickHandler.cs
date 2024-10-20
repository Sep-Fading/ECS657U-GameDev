using UnityEngine;
using UnityEngine.EventSystems;

namespace InventoryScripts
{
    public class InventoryClickHandler : MonoBehaviour, IPointerClickHandler
    {
        private InventoryManager _inventoryManager;
        [SerializeField] private GameObject playerObject;
        [SerializeField] private int inventoryIndex;

        private void Start()
        {
            _inventoryManager = playerObject.GetComponent<InventoryManager>();
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            InventoryItem item = Inventory.Instance.GetItemFromIndex(inventoryIndex);
            _inventoryManager.MoveToEquipment(item, inventoryIndex);
        }
    }
}