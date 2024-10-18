using GameplayMechanics.Effects;

namespace InventoryScripts
{
    public class Equipment
    {
        public GameItem Item;
        private EquipmentEffects _effects;
        public EquipmentType type;

        public Equipment(EquipmentType type)
        {
            this.type = type;
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
    }
}