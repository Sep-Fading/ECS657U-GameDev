using System.Collections.Generic;
using System.Linq;
using GameplayMechanics.Effects;
using InventoryScripts;
using UnityEngine;
using UnityEngine.Serialization;

namespace InventoryScripts
{
    // Initialises a piece of equipment as a functional
    // GameObject.
    public class EquipmentInitializer : MonoBehaviour
    {
        public Equipment Equipment;
        [FormerlySerializedAs("ItemPrefab")] [SerializeField] public GameObject itemPrefab;
        [SerializeField] private EquipmentType equipmentType = EquipmentType.NONE;
        [SerializeField] private new string name;
        [SerializeField] private string description;
        [SerializeField] private List<StatInfo> equipmentEffectTypes;
        private readonly List<EquipmentEffect> _equipmentEffects = new List<EquipmentEffect>();
        void Start()
        {
            if (equipmentType != EquipmentType.NONE)
            {
                foreach (StatInfo stat in equipmentEffectTypes)
                {
                    if (stat.equipmentType == EquipmentEffectTypes.FLAT_MELEE_DAMAGE)
                    {
                        _equipmentEffects.Add(new FlatMeleeDamageEffect(stat.flat));
                    }
                    else if (stat.equipmentType == EquipmentEffectTypes.MULTI_MELEE_DAMAGE)
                    {
                        _equipmentEffects.Add(new MultiplierMeleeDamageEffect(stat.multi));
                    }
                    else if (stat.equipmentType == EquipmentEffectTypes.FLAT_ARMOUR)
                    {
                        _equipmentEffects.Add(new FlatArmourEffect(stat.flat));
                    }
                    else if (stat.equipmentType == EquipmentEffectTypes.MULTI_ARMOUR)
                    {
                        _equipmentEffects.Add(new MultiplierArmourEffect(stat.multi));
                    }
                    else if (stat.equipmentType == EquipmentEffectTypes.FLAT_HEALTH)
                    {
                        _equipmentEffects.Add(new FlatHealthEffect(stat.flat));
                    }
                    else if (stat.equipmentType == EquipmentEffectTypes.FLAT_BLOCK_EFFECTIVENESS)
                    {
                        _equipmentEffects.Add(new FlatBlockEffectivenessEffect(stat.flat));
                    }
                }
                Equipment = new Equipment(name, description, equipmentType, itemPrefab,
                    _equipmentEffects);
            }
        }
    }

    public enum EquipmentEffectTypes
    {
        FLAT_MELEE_DAMAGE,
        MULTI_MELEE_DAMAGE,
        FLAT_ARMOUR,
        MULTI_ARMOUR,
        FLAT_HEALTH,
        FLAT_BLOCK_EFFECTIVENESS
    }
    
    [System.Serializable]
    class StatInfo
    {
        [FormerlySerializedAs("_equipmentType")] public EquipmentEffectTypes equipmentType;
        public float multi;
        public float flat;
    }
}
