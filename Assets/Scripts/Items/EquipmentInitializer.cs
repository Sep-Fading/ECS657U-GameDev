using GameplayMechanics.Effects;
using InventoryScripts;
using UnityEngine;

namespace Items
{
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
                Debug.Log($"{equipment.GetItem().GetName()}");
            }
            else
            {
                Debug.LogWarning("No equipment type selected in the inspector!");
            }
        }
        
    }
}
