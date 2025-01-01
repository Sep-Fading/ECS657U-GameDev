using System.Collections.Generic;
using System.Linq;
using GameplayMechanics.Character;
using GameplayMechanics.Effects;
using Player;
using UnityEngine;
using UnityEngine.UIElements;

namespace InventoryScripts
{
    // A singleton class that holds our inventory items together
    // including our equipment slots.
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

        public static void ResetInstance()
        {
            Instance.EquippedMainHand.Unequip();
            Instance = null;
        }
        
        /* Class Behaviours and Properties */
        Stack<InventoryItem>[] _inventoryArray = new Stack<InventoryItem>[3]; // Inventory Space
        /* --- Equipment on character ---*/
        public Equipment EquippedArmour;
        public Equipment EquippedMainHand;
        public Equipment EquippedOffHand;
        public InventoryItem EquippedArmourItem;
        public InventoryItem EquippedMainHandItem;
        public InventoryItem EquippedOffHandItem;
        private GameObject MainHandItem;
        private GameObject OffHandItem;
        
        /* --- Gold --- */
        private static int Gold = 0;

        public static int GetGold() => Gold;
        private static void SetGold(int gold) => Gold = gold;

        public static void GiveGold(int amount)
        {
            SetGold(GetGold() + amount);
        }

        public static void TakeGold(int amount)
        {
            SetGold(GetGold() - amount);
        }

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
        public EquipmentType Equip(InventoryItem item)
        {
            if (item.equipment.type == EquipmentType.ARMOR)
            {
                EquippedArmourItem = item;
                EquippedArmour = item.equipment;
                EquippedArmour.Equip();
                if (PlayerSkillTreeManager.Instance.ManagerSkillTree.branchSwordShield
                    .GetNodeByName("Versatile Combatant")._effect.isActive)
                {
                    Debug.Log("Vers Off");
                    PlayerSkillTreeManager.Instance.ManagerSkillTree.branchSwordShield
                        .GetNodeByName("Versatile Combatant")._effect.turnOff();
                }

                if (PlayerSkillTreeManager.Instance.ManagerSkillTree.branchSwordShield
                    .GetNodeByName("Juggernaut")._effect.isActive)
                {
                    Debug.Log("Jugg On");
                    PlayerSkillTreeManager.Instance.JuggernautRepeatingInvoke();
                }
                return EquipmentType.ARMOR;
            }

            if (item.equipment.type == EquipmentType.OFFHAND)
            {
                if (EquippedMainHand == null ||
                    Instance.EquippedMainHand.GetEquipmentType() != EquipmentType.GREATSWORD)
                {
                    EquippedOffHandItem = item;
                    EquippedOffHand = item.equipment;
                    EquippedOffHand.Equip();
                    this.OffHandItem = GameObject.Instantiate(EquippedOffHand.GetGameObject(),
                        GameObject.FindWithTag("ShieldSlot").transform);
                    this.OffHandItem.tag = "Shield";
                    
                    return EquipmentType.OFFHAND;
                }
            }

            if (item.equipment.type == EquipmentType.MAINHAND ||
                item.equipment.type == EquipmentType.GREATSWORD || item.equipment.type == EquipmentType.AXE)
            {
                EquippedMainHandItem = item;
                EquippedMainHand = item.equipment;
                EquippedMainHand.Equip();
                //david part
                this.MainHandItem = GameObject.Instantiate(EquippedMainHand.GetGameObject(), GameObject.FindWithTag("WeaponSlot").transform);
                this.MainHandItem.tag = "Weapon";
                return item.equipment.type;
            }

            return EquipmentType.NONE;
        }

        public EquipmentType Unequip(Equipment equipment)
        {
            if (equipment == EquippedArmour)
            {
                EquippedArmour.Unequip();
                EquippedArmour = null;
                
                if (PlayerSkillTreeManager.Instance.ManagerSkillTree
                    .branchSwordShield.GetNodeByName("Versatile Combatant")._effect.isActive)
                {
                    Debug.Log("Vers On");
                    PlayerSkillTreeManager.Instance.ManagerSkillTree
                        .branchSwordShield.GetNodeByName("Versatile Combatant")._effect.Apply();
                }
                return EquipmentType.ARMOR;
            }

            if (equipment == EquippedOffHand)
            {
                EquippedOffHand.Unequip();
                EquippedOffHand = null;
                if(equipment.GetEquipmentType() == EquipmentType.OFFHAND)
                {
                    GameObject.Destroy(GameObject.FindWithTag("Shield"));
                }
                return EquipmentType.OFFHAND;
            }

            if (equipment == EquippedMainHand)
            {
                EquippedMainHand.Unequip();
                EquippedMainHand = null;
                //david part
                if(equipment.GetEquipmentType() == EquipmentType.AXE)
                {
                    GameObject.Destroy(GameObject.FindWithTag("Weapon"));
                }
                GameObject.Destroy(this.MainHandItem);
                return EquipmentType.MAINHAND;
            }

            return EquipmentType.NONE;
        }

        public InventoryItem GetItemFromIndex(int i)
        {
            InventoryItem item;
            if (_inventoryArray[i] != null)
            {
                _inventoryArray[i].TryPeek(out item);
                return item;
            }
            return null;
        }
        
        public Stack<InventoryItem>[] GetStack() => this._inventoryArray;

        public bool IsEmpty(EquipmentType equipmentType)
        {
            if (equipmentType == EquipmentType.ARMOR && EquippedArmour == null)
            {
                return true;
            }

            if ((equipmentType == EquipmentType.MAINHAND ||
                 equipmentType == EquipmentType.GREATSWORD||
                 equipmentType == EquipmentType.AXE) && EquippedMainHand == null)
            {
                return true;
            }

            if (equipmentType == EquipmentType.OFFHAND && EquippedOffHand == null)
            {
                return true;
            }

            return false;
        }
        
        public bool ItemExistsInInventory(InventoryItem item)
        {
            return _inventoryArray.Any(stack => stack != null && stack.Contains(item));
        }

        public List<Stack<InventoryItem>> GetInventory() => this._inventoryArray.ToList();

        public string GetInventoryString()
        {
            string inventoryString = "[";
            foreach (var stack in _inventoryArray)
            {
                if (stack != null)
                {
                    foreach (var item in stack)
                    {
                        inventoryString += item.gameItem.GetName() + " ,";
                    }
                }

                
            }
            char[] arr = inventoryString.ToCharArray();
                            arr[arr.Length - 1] = ']';
                            inventoryString = new string(arr);
            return inventoryString;
        }

        public bool HasSpace()
        {
            for (int i = 0; i < _inventoryArray.Length; i++)
            {
                if (_inventoryArray[i] == null || _inventoryArray[i].Count == 0)
                {
                    return true;
                }

                /*if (_inventoryArray[i].Count < _inventoryArray[i].Peek().gameItem.GetStackSize())
                {
                    return true;
                }*/
            }

            return false;
        }
    }
}


