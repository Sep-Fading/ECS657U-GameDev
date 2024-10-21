using System.Collections.Generic;
using GameplayMechanics.Effects;
using UnityEngine;

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
        public Equipment EquippedArmour;
        public Equipment EquippedMainHand;
        public Equipment EquippedOffHand;

        public int Push(InventoryItem item)
        {
            for (int i = 0; i < _inventoryArray.Length; i++)
            {
                if (_inventoryArray[i] == null || _inventoryArray[i].Count == 0)
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
                    InventoryItem temp = _inventoryArray[i].Pop();
                    if (_inventoryArray[i].Count == 0)
                    {
                        _inventoryArray[i] = new Stack<InventoryItem>();
                    }
                    return temp;
                }
            }
            return null;
        }
        
        // Equipment Setters
        public EquipmentType Equip(Equipment equipment)
        {
            if (equipment.type == EquipmentType.ARMOR)
            {
                EquippedArmour = equipment;
                EquippedArmour.Equip();
                return EquipmentType.ARMOR;
            }

            if (equipment.type == EquipmentType.OFFHAND)
            {
                EquippedOffHand = equipment;
                EquippedOffHand.Equip();
                return EquipmentType.OFFHAND;
            }

            if (equipment.type == EquipmentType.MAINHAND)
            {
                EquippedMainHand = equipment;
                EquippedMainHand.Equip();
                return EquipmentType.MAINHAND;
            }

            return EquipmentType.NONE;
        }

        public EquipmentType Unequip(Equipment equipment)
        {
            if (equipment == EquippedArmour)
            {
                EquippedArmour.Unequip();
                EquippedArmour = null;
                return EquipmentType.ARMOR;
            }

            if (equipment == EquippedOffHand)
            {
                EquippedOffHand.Unequip();
                EquippedOffHand = null;
                return EquipmentType.OFFHAND;
            }

            if (equipment == EquippedMainHand)
            {
                EquippedMainHand.Unequip();
                EquippedMainHand = null;
                return EquipmentType.MAINHAND;
            }

            return EquipmentType.NONE;
        }

        public InventoryItem GetItemFromIndex(int i)
        {
            return _inventoryArray[i].Peek();
        }
        
        public Stack<InventoryItem>[] GetStack() => this._inventoryArray;

        public bool IsEmpty(EquipmentType equipmentType)
        {
            if (equipmentType == EquipmentType.ARMOR && EquippedArmour == null)
            {
                return true;
            }

            if (equipmentType == EquipmentType.MAINHAND && EquippedMainHand == null)
            {
                return true;
            }

            if (equipmentType == EquipmentType.OFFHAND && EquippedOffHand == null)
            {
                return true;
            }

            return false;
        }
    }
}


