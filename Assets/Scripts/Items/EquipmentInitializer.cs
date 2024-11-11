using System.Collections.Generic;
using System.Linq;
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
        [SerializeField] private string name;
        [SerializeField] private string description;
        [SerializeField] private List<StatInfo> equipmentEffectTypes;
        private List<EquipmentEffect> equipmentEffects = new List<EquipmentEffect>();
        void Start()
        {
            if (equipmentType != EquipmentType.NONE)
            {
                foreach (StatInfo stat in equipmentEffectTypes)
                {
                    if (stat._equipmentType == EquipmentEffectTypes.FLAT_MELEE_DAMAGE)
                    {
                        equipmentEffects.Add(new FlatMeleeDamageEffect(stat.flat));
                    }
                    else if (stat._equipmentType == EquipmentEffectTypes.MULTI_MELEE_DAMAGE)
                    {
                        equipmentEffects.Add(new MultiplierMeleeDamageEffect(stat.multi));
                    }
                    else if (stat._equipmentType == EquipmentEffectTypes.FLAT_ARMOUR)
                    {
                        equipmentEffects.Add(new FlatArmourEffect(stat.flat));
                    }
                    else if (stat._equipmentType == EquipmentEffectTypes.MULTI_ARMOUR)
                    {
                        equipmentEffects.Add(new MultiplierArmourEffect(stat.multi));
                    }
                }
                equipment = new Equipment(name, description, equipmentType, ItemPrefab,
                    equipmentEffects);
            }
        }
    }

    public enum EquipmentEffectTypes
    {
        FLAT_MELEE_DAMAGE,
        MULTI_MELEE_DAMAGE,
        FLAT_ARMOUR,
        MULTI_ARMOUR
    }
    
    [System.Serializable]
    class StatInfo
    {
        public EquipmentEffectTypes _equipmentType;
        public float multi;
        public float flat;
    }
}
