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
        Stack<GameItem>[] _inventoryArray = new Stack<GameItem>[4]; // Inventory Space
        /* --- Equipment on character ---*/
        private Equipment EquippedArmour;
        private Equipment EquippedMainHand;
        private Equipment EquippedOffHand;

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
                EquippedArmour = equipment;
                EquippedArmour.Equip();
            }
            else if (equipment.type == EquipmentType.OFFHAND)
            {
                EquippedOffHand = equipment;
                EquippedOffHand.Equip();
            }
            else if (equipment.type == EquipmentType.MAINHAND)
            {
                EquippedMainHand = equipment;
                EquippedMainHand.Equip();
            }
        }

        public void Unequip(Equipment equipment)
        {
            if (equipment == EquippedArmour)
            {
                EquippedArmour.Unequip();
                EquippedArmour = null;
            }
            else if (equipment == EquippedOffHand)
            {
                EquippedOffHand.Unequip();
                EquippedOffHand = null;
            }
            else if (equipment == EquippedMainHand)
            {
                EquippedMainHand.Unequip();
                EquippedMainHand = null;
            }
        }
    }
}


