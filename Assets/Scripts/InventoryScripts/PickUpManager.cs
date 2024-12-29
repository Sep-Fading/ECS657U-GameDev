using Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InventoryScripts
{
    // A simple script that adds the capability
    // to pick up an item the player is looking at,
    // sending them to the inventory manager for handling.
    public class PickUpManager : MonoBehaviour
    {
        private InventoryItem _inventoryItem;
        private PlayerInteract _playerInteract;
        private InventoryManager _inventoryManager;
        public GameObject pickUpPrefab;
    
        // Start is called before the first frame update
        void Start()
        {
            _playerInteract = GetComponent<PlayerInteract>();
            _inventoryManager = GetComponent<InventoryManager>();
        }
        
        public void SetItemToPickUp(EquipmentInitializer item)
        {
            _inventoryItem = new InventoryItem(item.Equipment.GetItem(), item.Equipment);
            pickUpPrefab = item.gameObject;
        }
        
        public void HandlePickUp(InputAction.CallbackContext context)
        {
            if (_playerInteract.canPickUp)
            {
                _inventoryManager.Push(_inventoryItem);
                Destroy(pickUpPrefab);
            }
        }

        
    }
}
