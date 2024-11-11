using System.Collections.Generic;
using GameplayMechanics.Effects;
using UnityEngine;

namespace InventoryScripts
{
    public class Equipment
    {
        public GameItem Item;
        public EquipmentType type;
        private List<EquipmentEffect> _equipmentEffects = new List<EquipmentEffect>();
        private GameObject obj;

        public Equipment(string name, string description, EquipmentType type, GameObject obj,
            List<EquipmentEffect> equipmentEffects)
        {
            this.obj = obj;
            this.type = type;
            _equipmentEffects = equipmentEffects;
            Item = new GameItem(name, description,
                1, ItemType.EQUIPMENT);
        }

        public void Equip()
        {
            foreach (EquipmentEffect effect in _equipmentEffects)
            {
                effect.Apply();
            }
        }

        public void Unequip()
        {
            foreach (EquipmentEffect effect in _equipmentEffects)
            {
                effect.Clear();
            }
        }

        public GameItem GetItem()
        {
            return this.Item;
        }

        public GameObject GetGameObject()
        {
            return this.obj;
        }
    }
}