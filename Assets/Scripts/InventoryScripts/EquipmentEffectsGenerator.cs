using System.Collections.Generic;
using System.Security.Cryptography;
using GameplayMechanics.Effects;
// for Random.Range if you want to use UnityEngine's random
// for EquipmentEffectTypes enum if it's located there

// for the effect classes

namespace InventoryScripts
{
    public sealed class EquipmentEffectGenerator
    {
        // 1) Private static instance
        private static EquipmentEffectGenerator _instance;

        // 2) Private constructor to prevent outside instantiation
        private EquipmentEffectGenerator() { }

        // 3) Public static property to access the single instance
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
        /// Generates between minEffects and maxEffects random equipment effects.
        /// </summary>
        public List<EquipmentEffect> GenerateRandomEquipmentEffects(
            EquipmentType equipmentType,
            int minEffects = 1, 
            int maxEffects = 3)
        {
            // 1. First, figure out which effect types are valid for this equipmentType.
            EquipmentEffectTypes[] validEffectTypes;

            switch (equipmentType)
            {
                case EquipmentType.MAINHAND:
                case EquipmentType.AXE:
                case EquipmentType.GREATSWORD:
                    validEffectTypes = new EquipmentEffectTypes[]
                    {
                        EquipmentEffectTypes.FLAT_MELEE_DAMAGE,
                        EquipmentEffectTypes.MULTI_MELEE_DAMAGE,
                        EquipmentEffectTypes.FLAT_BLOCK_EFFECTIVENESS
                    };
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
                        EquipmentEffectTypes.FLAT_BLOCK_EFFECTIVENESS
                    };
                    break;

                default:
                    // If NONE or something else, no effects.
                    validEffectTypes = new EquipmentEffectTypes[0];
                    break;
            }

            // 2. Decide how many effects to generate.
            int numberOfEffects = RandomNumberGenerator.GetInt32(minEffects, maxEffects + 1);

            List<EquipmentEffect> effects = new List<EquipmentEffect>();

            // 3. Pick random effects only from the valid list.
            for (int i = 0; i < numberOfEffects; i++)
            {
                if (validEffectTypes.Length == 0) 
                    break; // No valid effects for this type, so bail out.

                EquipmentEffectTypes effectType = validEffectTypes[
                    RandomNumberGenerator.GetInt32(0, validEffectTypes.Length)];

                // 4. Create the effect with random values, depending on effectType.
                switch (effectType)
                {
                    case EquipmentEffectTypes.FLAT_MELEE_DAMAGE:
                    {
                        float randomFlatDamage = UnityEngine.Random.Range(5f, 20f);
                        effects.Add(new FlatMeleeDamageEffect(randomFlatDamage));
                        break;
                    }
                    case EquipmentEffectTypes.MULTI_MELEE_DAMAGE:
                    {
                        float randomMultiDamage = UnityEngine.Random.Range(0.15f, 1.5f);
                        effects.Add(new MultiplierMeleeDamageEffect(randomMultiDamage));
                        break;
                    }
                    case EquipmentEffectTypes.FLAT_BLOCK_EFFECTIVENESS:
                    {
                        float randomBlockEffectiveness = UnityEngine.Random.Range(0.1f, 0.2f);
                        effects.Add(new FlatBlockEffectivenessEffect(randomBlockEffectiveness));
                        break;
                    }
                    case EquipmentEffectTypes.FLAT_ARMOUR:
                    {
                        float randomFlatArmour = UnityEngine.Random.Range(5f, 20f);
                        effects.Add(new FlatArmourEffect(randomFlatArmour));
                        break;
                    }
                    case EquipmentEffectTypes.MULTI_ARMOUR:
                    {
                        float randomMultiArmour = UnityEngine.Random.Range(1.1f, 2.0f);
                        effects.Add(new MultiplierArmourEffect(randomMultiArmour));
                        break;
                    }
                    case EquipmentEffectTypes.FLAT_HEALTH:
                    {
                        float randomFlatHealth = UnityEngine.Random.Range(10f, 100f);
                        effects.Add(new FlatHealthEffect(randomFlatHealth));
                        break;
                    }
                }
            }

            return effects;
        }
 
    }
}