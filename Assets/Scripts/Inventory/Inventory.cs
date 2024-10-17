using System.Collections.Generic;
using GameplayMechanics.Effects;

namespace Inventory
{
    public class Inventory
    { 
        public static Inventory Instance { get; private set; }

        public static Inventory Initialize()
        {
            if (Instance == null)
            {
                Instance = new Inventory();
            }
            return Instance;
        }
        
        /* Class Behaviours and Properties */
        Stack<GameItem>[] _inventoryArray = new Stack<GameItem>[3]; // Inventory Space
        /* --- Equipment on character ---*/
        private Equipment _equippedArmour;
        private Equipment _equippedMainHand;
        private Equipment _equippedOffHand;

        public void Push(GameItem item)
        {
            for (int i = 0; i < _inventoryArray.Length; i++)
            {
                if (_inventoryArray[i] == null)
                {
                    _inventoryArray[i] = new Stack<GameItem>();
                    _inventoryArray[i].Push(item);
                    return;
                }
            }
        }

        public GameItem Pop(GameItem item)
        {
            for (int i = 0; i < _inventoryArray.Length; i++)
            {
                if (_inventoryArray[i] != null && _inventoryArray[i].Contains(item))
                {
                    return _inventoryArray[i].Pop();
                }
            }
            return null;
        }
        
        // Equipment Setters
        public void Equip(Equipment equipment)
        {
            if (equipment.type == EquipmentType.ARMOR)
            {
                _equippedArmour = equipment;
                _equippedArmour.Equip();
            }
            else if (equipment.type == EquipmentType.OFFHAND)
            {
                _equippedOffHand = equipment;
                _equippedOffHand.Equip();
            }
            else if (equipment.type == EquipmentType.MAINHAND)
            {
                _equippedMainHand = equipment;
                _equippedMainHand.Equip();
            }
        }

        public void Unequip(Equipment equipment)
        {
            if (equipment == _equippedArmour)
            {
                _equippedArmour.Unequip();
                _equippedArmour = null;
            }
            else if (equipment == _equippedOffHand)
            {
                _equippedOffHand.Unequip();
                _equippedOffHand = null;
            }
            else if (equipment == _equippedMainHand)
            {
                _equippedMainHand.Unequip();
                _equippedMainHand = null;
            }
        }
    }
}


