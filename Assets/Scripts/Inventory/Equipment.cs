using GameplayMechanics.Effects;
using UnityEditor;

namespace Inventory
{
    public class Equipment
    {
        private GameItem _item;
        private EquipmentEffects _effects;
        public EquipmentType type;

        Equipment(EquipmentType type)
        {
            this.type = type;
            _effects = new EquipmentEffects(type);
            _item = new GameItem(_effects.name, _effects.description,
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
    }
}