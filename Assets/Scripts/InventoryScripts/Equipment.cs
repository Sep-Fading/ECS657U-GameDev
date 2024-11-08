using GameplayMechanics.Effects;
using UnityEngine;

namespace InventoryScripts
{
    public class Equipment
    {
        public GameItem Item;
        private EquipmentEffects _effects;
        public EquipmentType type;
        private GameObject obj;

        public Equipment(EquipmentType type, GameObject obj)
        {
            this.type = type;
            this.obj = obj;
            _effects = new EquipmentEffects(type);
            Item = new GameItem(_effects.name, _effects.description,
                1, ItemType.EQUIPMENT);
        }

        public void Equip()
        {
            _effects.Apply();
        }

        public void Unequip()
        {
            _effects.Clear();
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