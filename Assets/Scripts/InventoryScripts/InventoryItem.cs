﻿using GameplayMechanics.Effects;
using UnityEngine;

namespace InventoryScripts
{
    /// <summary>
    ///  A wrapper class that is used to cross-reference
    /// GameItems within Equipments and vice versa.
    /// </summary>
    public class InventoryItem
    {
        public GameItem gameItem;
        public Equipment equipment;
        public bool isEquipped;

        public InventoryItem(GameItem gameItem, Equipment equipment)
        {
            this.isEquipped = false;
            this.gameItem = gameItem;
            this.equipment = equipment;
        }

        public string GetDescription()
        {
            if (gameItem.GetDescription().Length > 0)
            {
                return gameItem.GetDescription();
            }

            return GenerateDescriptionFromEquipment();
        }

        private string GenerateDescriptionFromEquipment()
        {
            string description = "";
            description += "Type: " + equipment.GetEquipmentType() + "\n";
            foreach (EquipmentEffect effect in equipment.GetEffects())
            {
                description += effect.GetDisplayDescription() + "\n";
            }

            return description;
        }
    }
}