using System.Collections.Generic;
using GameplayMechanics.Effects;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace InventoryScripts
{
    // This script initialises and manages the related GameObjects
    // in the scene for Inventory.
    public class InventoryManager : MonoBehaviour
    {
        [FormerlySerializedAs("_inventoryItemsUI")] public GameObject[] inventoryItemsUI = new GameObject[3];
        [FormerlySerializedAs("_equippedItemsUI")] public GameObject[] equippedItemsUI = new GameObject[3];
        // Start is called before the first frame update
        void Start()
        {
            Inventory.Initialize();
        }

        public void Push(InventoryItem inventoryItem)
        {
            int i = Inventory.Instance.Push(inventoryItem);
            AddToInventoryUI(inventoryItem, i);
        }

        private void AddToInventoryUI(InventoryItem inventoryItem, int i)
        {
            if (i < 0 || i >= inventoryItemsUI.Length)
            {
                return;
            }
            inventoryItemsUI[i].GetComponent<TextMeshProUGUI>().
                SetText(inventoryItem.gameItem.GetName()[0].ToString());
        }

        public void MoveToEquipment(InventoryItem inventoryItem, int i)
        {
            if (Inventory.Instance.IsEmpty(inventoryItem.equipment.type))
            {
                inventoryItemsUI[i].GetComponent<TextMeshProUGUI>().SetText("");
                InventoryItem item = Inventory.Instance.Pop(inventoryItem);
                EquipmentType type = Inventory.Instance.Equip(item.equipment);
                if (type == EquipmentType.NONE)
                {
                    
                }
                else if (type == EquipmentType.MAINHAND)
                {
                    equippedItemsUI[0].GetComponent<TextMeshProUGUI>()
                        .SetText(item.gameItem.GetName()[0].ToString());
                }
                else if (type == EquipmentType.ARMOR)
                {
                    equippedItemsUI[1].GetComponent<TextMeshProUGUI>()
                        .SetText(item.gameItem.GetName()[0].ToString());
                }
                else if (type == EquipmentType.OFFHAND)
                {
                    equippedItemsUI[2].GetComponent<TextMeshProUGUI>()
                        .SetText(item.gameItem.GetName()[0].ToString());
                }
            }
        }

        public void MoveToInventory(EquipmentType equipmentType, int i)
        {
            Equipment equipment;
            if (equipmentType == EquipmentType.MAINHAND)
            {
                equipment = Inventory.Instance.EquippedMainHand;
            }
            else if (equipmentType == EquipmentType.ARMOR)
            {
                equipment = Inventory.Instance.EquippedArmour;
            }
            else if (equipmentType == EquipmentType.OFFHAND)
            {
                equipment = Inventory.Instance.EquippedOffHand;
            }
            else
            {
                equipment = null;
            }
            if (equipment != null)
            {
                InventoryItem item = new InventoryItem(equipment.GetItem(), equipment);
                Inventory.Instance.Unequip(equipment);
                equippedItemsUI[i].GetComponent<TextMeshProUGUI>()
                    .SetText("");
                this.PrintInventoryStack();
                this.Push(item);
            }
        }

        private void PrintInventoryStack()
        {
            Stack<InventoryItem>[] itemStack = Inventory.Instance.GetStack();
            bool[] results = new bool[itemStack.Length];

            for (int i = 0; i < itemStack.Length; i++)
            {
                if (itemStack[i] != null)
                {
                    results[i] = true;
                }
                else
                {
                    results[i] = false;
                }
            }
        }
    }
}
