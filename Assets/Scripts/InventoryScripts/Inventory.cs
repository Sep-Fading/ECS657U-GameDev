using System.Collections.Generic;
using GameplayMechanics.Effects;

namespace InventoryScripts
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
        Stack<InventoryItem>[] _inventoryArray = new Stack<InventoryItem>[3]; // Inventory Space
        /* --- Equipment on character ---*/
        private Equipment _equippedArmour;
        private Equipment _equippedMainHand;
        private Equipment _equippedOffHand;

        public int Push(InventoryItem item)
        {
            for (int i = 0; i < _inventoryArray.Length; i++)
            {
                if (_inventoryArray[i] == null)
                {
                    _inventoryArray[i] = new Stack<InventoryItem>();
                    _inventoryArray[i].Push(item);
                    return i;
                }

                if (_inventoryArray[i].Contains(item))
                {
                    if (_inventoryArray[i].Count < item.gameItem.GetStackSize())
                    {
                        _inventoryArray[i].Push(item);
                    }
                }
            }

            return -1;
        }

        public InventoryItem Pop(InventoryItem item)
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


