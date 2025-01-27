﻿using System;
using System.Collections.Generic;
using GameplayMechanics.Effects;
using Unity.VisualScripting;

namespace InventoryScripts
{
    /// <summary>
    /// A singleton class that generates item names like:
    /// "Gladiator's MainHand of Strength"
    /// </summary>
    public sealed class EquipmentNameGenerator
    {
        // 1. Private static instance (singleton pattern)
        private static EquipmentNameGenerator _instance;
        
        // 2. Private constructor to prevent outside instantiation
        private EquipmentNameGenerator()
        {
            // 15 example fantasy prefixes
            Prefixes = new List<string>
            {
                "Gladiator's",
                "Warlord's",
                "Dragonforged",
                "Titan's",
                "Shadow-Forged",
                "Elven",
                "Arcane",
                "Dwarven",
                "Valkyrie's",
                "Stormforged",
                "Demonhunter's",
                "Runic",
                "Celestial",
                "Frostbound",
                "Zealot's"
            };

            // 15 example fantasy suffixes
            Suffixes = new List<string>
            {
                "of Strength",
                "of the Giants",
                "of Agility",
                "of Power",
                "of Doom",
                "of the Night",
                "of Fury",
                "of Reckoning",
                "of Valor",
                "of the Wolf",
                "of Unending",
                "of Devastation",
                "of the Abyss"
            };
            
            _random = new Random();
        }

        // 3. Public static property to access the single instance
        public static EquipmentNameGenerator Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new EquipmentNameGenerator();
                }
                return _instance;
            }
        }

        // 4. Lists of prefix/suffix and name options
        private readonly List<string> Prefixes;
        private readonly List<string> Suffixes;

        // 5. Random number generator (for picking random prefixes/suffixes)
        private readonly Random _random;

        /// <summary>
        /// Generates a random prefix and suffix around the provided EquipmentType.
        /// Format: "Prefix EquipmentType Suffix"
        /// </summary>
        /// <param name="equipmentType">The equipment type enum</param>
        /// <returns>Combined name string (e.g. "Gladiator's MainHand of Strength")</returns>
        public static string GenerateEquipmentName(EquipmentType equipmentType)
        {
            var generator = Instance;

            // Pick a random prefix
            var prefixIndex = generator._random.Next(generator.Prefixes.Count);
            var prefix = generator.Prefixes[prefixIndex];

            // Pick a random suffix
            var suffixIndex = generator._random.Next(generator.Suffixes.Count);
            var suffix = generator.Suffixes[suffixIndex];
            
            // Equipment Name
            string actualName;
            if (equipmentType == EquipmentType.MAINHAND)
            {
                actualName = "Shortsword";
            }
            else
            {
                actualName = equipmentType.ToString().ToLower();
                actualName = actualName.FirstCharacterToUpper();
            }

            // Construct the final name
            if (equipmentType == EquipmentType.OFFHAND)
            {
                return $"{prefix} Shield {suffix}";
            }
            if (equipmentType == EquipmentType.ARMOR)
            {
                return $"{prefix} Armor {suffix}";
            }
            if (equipmentType == EquipmentType.MAINHAND
                | equipmentType == EquipmentType.AXE
                | equipmentType == EquipmentType.GREATSWORD)
            {
                return $"{prefix} {actualName} {suffix}";
            }
            return "Unknown Equipment Type";
        }
    }
}