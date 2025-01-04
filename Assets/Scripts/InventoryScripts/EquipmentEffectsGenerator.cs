using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using GameplayMechanics.Effects;

namespace InventoryScripts
{
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
        /// Generates random equipment effects with constraints based on equipment type.
        /// </summary>
        public List<EquipmentEffect> GenerateRandomEquipmentEffects(
            EquipmentType equipmentType,
            int minEffects = 1,
            int maxEffects = 3)
        {
            EquipmentEffectTypes[] validEffectTypes;
            List<EquipmentEffect> effects = new List<EquipmentEffect>();

            // 1. Define valid effect types based on the equipment type.
            switch (equipmentType)
            {
                case EquipmentType.MAINHAND:
                case EquipmentType.AXE:
                case EquipmentType.GREATSWORD:
                    validEffectTypes = new EquipmentEffectTypes[]
                    {
                        EquipmentEffectTypes.FLAT_MELEE_DAMAGE,
                        EquipmentEffectTypes.MULTI_MELEE_DAMAGE
                    };
                    if (equipmentType == EquipmentType.GREATSWORD)
                    {
                        validEffectTypes = AppendEffect(validEffectTypes, EquipmentEffectTypes.FLAT_BLOCK_EFFECTIVENESS);
                    }
                    break;

                case EquipmentType.ARMOR:
                    validEffectTypes = new EquipmentEffectTypes[]
                    {
                        EquipmentEffectTypes.FLAT_ARMOUR,
                        EquipmentEffectTypes.MULTI_ARMOUR,
                        EquipmentEffectTypes.FLAT_HEALTH
                    };
                    break;

                case EquipmentType.OFFHAND:
                    validEffectTypes = new EquipmentEffectTypes[]
                    {
                        EquipmentEffectTypes.FLAT_ARMOUR,
                        EquipmentEffectTypes.MULTI_ARMOUR,
                        EquipmentEffectTypes.FLAT_BLOCK_EFFECTIVENESS,
                        EquipmentEffectTypes.FLAT_HEALTH
                    };
                    break;

                default:
                    validEffectTypes = Array.Empty<EquipmentEffectTypes>();
                    break;
            }

            // 2. Guarantee at least one specific effect based on equipment type.
            if (equipmentType == EquipmentType.MAINHAND || equipmentType == EquipmentType.AXE || equipmentType == EquipmentType.GREATSWORD)
            {
                effects.Add(CreateEffect(EquipmentEffectTypes.FLAT_MELEE_DAMAGE, equipmentType));
            }
            else if (equipmentType == EquipmentType.ARMOR)
            {
                effects.Add(CreateEffect(EquipmentEffectTypes.FLAT_ARMOUR, equipmentType));
            }
            else if (equipmentType == EquipmentType.OFFHAND)
            {
                effects.Add(CreateEffect(EquipmentEffectTypes.FLAT_BLOCK_EFFECTIVENESS, equipmentType));
            }

            // 3. Decide how many additional effects to generate.
            int numberOfAdditionalEffects = RandomNumberGenerator.GetInt32(Math.Max(0, minEffects - effects.Count), maxEffects - effects.Count + 1);

            // 4. Add random effects from the valid list, ensuring no duplicates.
            for (int i = 0; i < numberOfAdditionalEffects; i++)
            {
                EquipmentEffectTypes effectType;
                do
                {
                    effectType = validEffectTypes[RandomNumberGenerator.GetInt32(0, validEffectTypes.Length)];
                } while (effects.Exists(e => e.effectTypeEnum == effectType));

                effects.Add(CreateEffect(effectType, equipmentType));
            }

            return effects;
        }

        /// <summary>
        /// Creates an equipment effect with randomized values based on the type and equipment type.
        /// </summary>
        private EquipmentEffect CreateEffect(EquipmentEffectTypes effectType, EquipmentType equipmentType)
        {
            float damageMultiplier = equipmentType == EquipmentType.GREATSWORD ? 2f : 1f; // Greatsword has significantly higher damage.
            float blockMulti = equipmentType == EquipmentType.OFFHAND ? 2f : 1f; // Shields have higher block effectiveness.
            switch (effectType)
            {
                case EquipmentEffectTypes.FLAT_MELEE_DAMAGE:
                    return new FlatMeleeDamageEffect(UnityEngine.Random.Range(15f, 45f) * damageMultiplier);
                case EquipmentEffectTypes.MULTI_MELEE_DAMAGE:
                    return new MultiplierMeleeDamageEffect(UnityEngine.Random.Range(0.15f, 1.5f) * damageMultiplier);
                case EquipmentEffectTypes.FLAT_BLOCK_EFFECTIVENESS:
                    return new FlatBlockEffectivenessEffect(UnityEngine.Random.Range(0.1f, 0.2f) * blockMulti);
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

        /// <summary>
        /// Helper method to append an effect type to an array.
        /// </summary>
        private EquipmentEffectTypes[] AppendEffect(EquipmentEffectTypes[] array, EquipmentEffectTypes newEffect)
        {
            EquipmentEffectTypes[] result = new EquipmentEffectTypes[array.Length + 1];
            Array.Copy(array, result, array.Length);
            result[array.Length] = newEffect;
            return result;
        }
    }
}
