using GameplayMechanics.Effects;
using InventoryScripts;
using UnityEngine;

namespace Items
{
    // Initialises a piece of equipment as a functional
    // GameObject.
    public class EquipmentInitializer : MonoBehaviour
    {
        public Equipment equipment;
        [SerializeField] public GameObject ItemPrefab;
        [SerializeField] private EquipmentType equipmentType = EquipmentType.NONE;
        void Start()
        {
            if (equipmentType != EquipmentType.NONE)
            {
                equipment = new Equipment(equipmentType,ItemPrefab);
            }
        }
        
    }
}
