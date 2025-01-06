using System;
using System.Collections.Generic;
using GameplayMechanics.Effects;

namespace InventoryScripts
{
    /// <summary>
    /// Handles the Effects Generation to be applied on the weapons
    /// </summary>
    public sealed class EquipmentEffectGenerator
    {
        private static EquipmentEffectGenerator _instance;

        private EquipmentEffectGenerator() { }

        public static EquipmentEffectGenerator Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new EquipmentEffectGenerator();
                }
                return _instance;
            }
        }

        /// <summary>
        /// Generates equipment effects with predefined mods based on equipment type.
        /// </summary>
        public List<EquipmentEffect> GenerateRandomEquipmentEffects(EquipmentType equipmentType)
        {
            var effects = new List<EquipmentEffect>();

            switch (equipmentType)
            {
                case EquipmentType.MAINHAND:
                case EquipmentType.AXE:
                    effects.Add(CreateEffect(EquipmentEffectTypes.FLAT_MELEE_DAMAGE, equipmentType));
                    effects.Add(CreateEffect(EquipmentEffectTypes.MULTI_MELEE_DAMAGE, equipmentType));
                    break;

                case EquipmentType.GREATSWORD:
                    effects.Add(CreateEffect(EquipmentEffectTypes.FLAT_MELEE_DAMAGE, equipmentType, 2f)); // Greatsword higher damage
                    effects.Add(CreateEffect(EquipmentEffectTypes.MULTI_MELEE_DAMAGE, equipmentType, 2f));
                    effects.Add(CreateEffect(EquipmentEffectTypes.FLAT_BLOCK_EFFECTIVENESS, equipmentType)); // Block can roll on greatsword
                    break;

                case EquipmentType.ARMOR:
                    effects.Add(CreateEffect(EquipmentEffectTypes.FLAT_HEALTH, equipmentType));
                    effects.Add(CreateEffect(EquipmentEffectTypes.FLAT_ARMOUR, equipmentType));
                    effects.Add(CreateEffect(EquipmentEffectTypes.MULTI_ARMOUR, equipmentType));
                    break;

                case EquipmentType.OFFHAND: // Shield
                    effects.Add(CreateEffect(EquipmentEffectTypes.FLAT_HEALTH, equipmentType));
                    effects.Add(CreateEffect(EquipmentEffectTypes.FLAT_ARMOUR, equipmentType));
                    effects.Add(CreateEffect(EquipmentEffectTypes.FLAT_BLOCK_EFFECTIVENESS, equipmentType, 2f)); // Higher block on shields
                    break;

                default:
                    // No effects for undefined equipment types
                    break;
            }

            return effects;
        }

        /// <summary>
        /// Creates an equipment effect with randomized values based on the type and equipment type.
        /// </summary>
        private EquipmentEffect CreateEffect(EquipmentEffectTypes effectType, EquipmentType equipmentType, float multiplier = 1f)
        {
            switch (effectType)
            {
                case EquipmentEffectTypes.FLAT_MELEE_DAMAGE:
                    return new FlatMeleeDamageEffect(UnityEngine.Random.Range(15f, 45f) * multiplier);
                case EquipmentEffectTypes.MULTI_MELEE_DAMAGE:
                    return new MultiplierMeleeDamageEffect(UnityEngine.Random.Range(0.15f, 1.5f) * multiplier);
                case EquipmentEffectTypes.FLAT_BLOCK_EFFECTIVENESS:
                    return new FlatBlockEffectivenessEffect(UnityEngine.Random.Range(0.1f, 0.2f) * multiplier);
                case EquipmentEffectTypes.FLAT_ARMOUR:
                    return new FlatArmourEffect(UnityEngine.Random.Range(20f, 100f));
                case EquipmentEffectTypes.MULTI_ARMOUR:
                    return new MultiplierArmourEffect(UnityEngine.Random.Range(1.1f, 2.0f));
                case EquipmentEffectTypes.FLAT_HEALTH:
                    return new FlatHealthEffect(UnityEngine.Random.Range(10f, 100f));
                default:
                    throw new ArgumentOutOfRangeException(nameof(effectType), $"Unhandled effect type: {effectType}");
            }
        }
    }
}
