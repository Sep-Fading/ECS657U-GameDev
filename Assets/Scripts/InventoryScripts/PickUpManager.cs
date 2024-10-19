using Items;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InventoryScripts
{
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
            _inventoryItem = new InventoryItem(item.equipment.GetItem(), item.equipment);
            pickUpPrefab = item.gameObject;
        }
        
        public void HandlePickUp(InputAction.CallbackContext context)
        {
            if (_playerInteract.canPickUp)
            {
                _inventoryManager.Push(_inventoryItem);
                Debug.Log($"{_inventoryItem.gameItem.GetName()} picked up!");
                Destroy(pickUpPrefab);
            }
        }

        
    }
}
