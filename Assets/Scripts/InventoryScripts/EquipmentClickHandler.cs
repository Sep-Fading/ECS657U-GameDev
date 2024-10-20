using GameplayMechanics.Effects;
using Items;
using UnityEngine;
using UnityEngine.EventSystems;

namespace InventoryScripts
{
    public class EquipmentClickHandler : MonoBehaviour, IPointerClickHandler
    {
        private InventoryManager _inventoryManager;
        [SerializeField] private GameObject playerObject;
        [SerializeField] private int equipmentIndex;
        [SerializeField] private EquipmentType equipmentType;

        private void Start()
        {
            _inventoryManager = playerObject.GetComponent<InventoryManager>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _inventoryManager.MoveToInventory(equipmentType, equipmentIndex);
        }
    }
}