using System.Collections;
using System.Collections.Generic;
using Enemy;
using GameplayMechanics.Character;
using GameplayMechanics.Effects;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace InventoryScripts
{
    /// <summary>
    /// Handles how the inventory items move within the UI
    /// </summary>
    public class InventoryManager : MonoBehaviour
    {
        [SerializeField] public GameObject[] inventoryItemsUI; // Inventory slots
        [SerializeField] public GameObject[] equippedItemsUI;  // Equipment slots

        [SerializeField] private int CheatGold;

        [SerializeField] private Sprite swordIcon;
        [SerializeField] private Sprite shieldIcon;
        [SerializeField] private Sprite greatswordIcon;
        [SerializeField] private Sprite axeIcon;
        [SerializeField] private Sprite armorIcon;
        [SerializeField] private Sprite potionIcon;

        void Start()
        {
            Inventory.Initialize();
            Inventory.GiveGold(CheatGold);
            
            for (int i = 0; i < inventoryItemsUI.Length; i++)
            {
                inventoryItemsUI[i].GetComponent<Image>().color = Color.clear;
            }
            for (int i = 0; i < equippedItemsUI.Length; i++)
            {
                equippedItemsUI[i].GetComponent<Image>().color = Color.clear;
            }

            // Add EventTriggers to inventory and equipment slots
            //SetupInventorySlotTriggers();
            //SetupEquipmentSlotTriggers();
        }

        /// <summary>
        /// Adds an inventory item to a slot.
        /// </summary>
        public void Push(InventoryItem inventoryItem)
        {
            int i = Inventory.Instance.Push(inventoryItem);
            /*
            Debug.LogWarning($"Pushed {inventoryItem.equipment.GetItem().GetName()} " +
                             $"to inventory ui slot {i}");
            Debug.LogWarning($"Inventory array: {Inventory.Instance.GetInventoryString()}");
            */
            AddToInventoryUI(inventoryItem, i);
        }

        /// <summary>
        /// Updates the inventory slot UI and assigns click handling.
        /// </summary>
        private void AddToInventoryUI(InventoryItem inventoryItem, int i)
        {
            if (i < 0 || i >= inventoryItemsUI.Length) return;

            // Update the UI with the item's sprite
            Image imageComponent = inventoryItemsUI[i].GetComponent<Image>();
            if (imageComponent != null)
            {
                imageComponent.sprite = GetCorrectSprite(inventoryItem);
                /*
                Debug.LogWarning($"Replaced sprite for {inventoryItem.equipment.GetItem().GetName()} " +
                                 $"in inventory ui slot {i} with {imageComponent.sprite.name}");
                */
                imageComponent.color = Color.white;
                imageComponent.enabled = true;
            }
        }

        /// <summary>
        /// Moves an inventory item to equipment.
        /// </summary>
        public void MoveToEquipment(InventoryItem inventoryItem, int index)
        {
            if (Inventory.Instance.IsEmpty(inventoryItem.equipment.type))
            {
                if (Inventory.Instance.EquippedMainHand.GetEquipmentType() == EquipmentType.GREATSWORD &&
                    Inventory.Instance.EquippedOffHand != null)
                { 
                    return;
                } 
                if (Inventory.Instance.EquippedMainHand.GetEquipmentType() == EquipmentType.OFFHAND  && 
                    Inventory.Instance.EquippedMainHand.GetEquipmentType() == EquipmentType.GREATSWORD)
                { 
                    return;
                }
                /*
                Debug.LogWarning($"{inventoryItem.equipment.GetEquipmentType()}" +
                                 $" slot is empty");
                */
                InventoryItem item = Inventory.Instance.Pop(inventoryItem);
                /*
                Debug.LogWarning($"Popped {item.equipment.GetItem().GetName()} " +
                                 $"from inventory ui slot {index}");
                Debug.LogWarning($"Inventory array: {Inventory.Instance.GetInventoryString()}");
                */
                int equipmentIndex = GetEquipmentIndex(inventoryItem.equipment.type);
                if (equipmentIndex >= 0)
                {
                    Inventory.Instance.Equip(inventoryItem);

                    // Clear inventory slot UI
                    ClearSlot(inventoryItemsUI[index]);

                    // Update equipment slot UI
                    UpdateEquipmentSlot(equipmentIndex, inventoryItem);
                }
            }
            
            UpdateInventoryUI();
            
            // Disable the collider after equipping
            var mainHand = GameObject.FindGameObjectWithTag("Weapon");
            if (mainHand != null)
            {
                var collider = mainHand.GetComponent<Collider>();
                if (collider != null)
                {
                    collider.enabled = false;
                    Debug.Log("Collider initially disabled after equipping.");
                }
                else
                {
                    Debug.LogWarning("BoxCollider component not found on the equipped main hand.");
                }
            }
            else
            {
                Debug.LogWarning("EquippedMainHand is null.");
            }
        }

        /// <summary>
        /// Moves an equipment item back to the inventory.
        /// </summary>
        public void MoveToInventory(EquipmentType equipmentType, int index)
        {
            
            Equipment equipment = equipmentType switch
            {
                EquipmentType.MAINHAND or EquipmentType.AXE or EquipmentType.GREATSWORD => Inventory.Instance.EquippedMainHand,
                EquipmentType.ARMOR => Inventory.Instance.EquippedArmour,
                EquipmentType.OFFHAND => Inventory.Instance.EquippedOffHand,
                _ => null
            };

            if (equipment != null)
            {
                Inventory.Instance.Unequip(equipment);
                InventoryItem inventoryItem = new InventoryItem(equipment.GetItem(), equipment);

                int inventoryIndex = Inventory.Instance.Push(inventoryItem);
                if (inventoryIndex >= 0)
                {
                    UpdateInventorySlot(inventoryIndex, inventoryItem);
                    ClearSlot(equippedItemsUI[index]);
                }
            }
            
            UpdateInventoryUI();
        }


        /// <summary>
        /// Updates an inventory slot with an item's sprite.
        /// </summary>
        private void UpdateInventorySlot(int index, InventoryItem item)
        {
            Image imageComponent = inventoryItemsUI[index].GetComponent<Image>();
            if (imageComponent != null)
            {
                if (item != null)
                {
                    imageComponent.sprite = GetCorrectSprite(item);
                    imageComponent.enabled = true;
                }
                else
                {
                    ClearSlot(inventoryItemsUI[index]);
                }
            }
        }

        /// <summary>
        /// Updates an equipment slot with an item's sprite.
        /// </summary>
        private void UpdateEquipmentSlot(int index, InventoryItem item)
        {
            Image imageComponent = equippedItemsUI[index].GetComponent<Image>();
            if (imageComponent != null)
            {
                if (item != null)
                {
                    imageComponent.sprite = GetCorrectSprite(item);
                    imageComponent.color = Color.white;
                    imageComponent.enabled = true;
                }
                else
                {
                    ClearSlot(equippedItemsUI[index]);
                }
            }
        }

        /// <summary>
        /// Clears a UI slot.
        /// </summary>
        private void ClearSlot(GameObject slot)
        {
            Image imageComponent = slot.GetComponent<Image>();
            if (imageComponent != null)
            {
                imageComponent.sprite = null;
                imageComponent.color = Color.clear;
                imageComponent.enabled = false;
            }
        }

        /// <summary>
        /// Gets the correct sprite for an inventory item.
        /// </summary>
        private Sprite GetCorrectSprite(InventoryItem item)
        {
            return item.equipment?.GetEquipmentType() switch
            {
                EquipmentType.MAINHAND => swordIcon,
                EquipmentType.AXE => axeIcon,
                EquipmentType.GREATSWORD => greatswordIcon,
                EquipmentType.OFFHAND => shieldIcon,
                EquipmentType.ARMOR => armorIcon,
                _ => potionIcon
            };
        }

        /// <summary>
        /// Maps EquipmentType to equipment slot index.
        /// </summary>
        private int GetEquipmentIndex(EquipmentType type)
        {
            return type switch
            {
                EquipmentType.MAINHAND or EquipmentType.AXE or EquipmentType.GREATSWORD => 0,
                EquipmentType.ARMOR => 1,
                EquipmentType.OFFHAND => 2,
                _ => -1
            };
        }
        
        public void UpdateInventoryUI()
        {
            // Get the current inventory list
            List<Stack<InventoryItem>> playerItems = Inventory.Instance.GetInventory();

            // Update the inventory UI slots
            for (int i = 0; i < inventoryItemsUI.Length; i++)
            {
                Image imageComponent = inventoryItemsUI[i].GetComponent<Image>();
                if (playerItems[i] == null || i >= playerItems.Count || playerItems[i].Count == 0)
                {
                    ClearSlot(inventoryItemsUI[i]);
                }
                else
                {
                    InventoryItem item = playerItems[i].Peek();
                    imageComponent.sprite = GetCorrectSprite(item);
                    imageComponent.color = Color.white;
                    imageComponent.enabled = true;
                }
            }
        }


        public void ConsumeItem(InventoryItem item, int inventoryIndex)
        {
            if (item != null &&
                item.gameItem.GetItemType() == ItemType.CONSUMABLE)
            {
                InventoryItem temp = Inventory.Instance.Pop(item);
                UpdateInventoryUI();
                StartCoroutine(Heal(temp.gameItem.ConsumeHpPotion(), 1f));
            }
        }

        private IEnumerator Heal(int consumeHpPotion, float f)
        {
            float startHealth = PlayerStatManager.Instance.Life.GetCurrent();
            float targetHealth = Mathf.Clamp(startHealth + consumeHpPotion, 0,
                PlayerStatManager.Instance.Life.GetAppliedTotal());
            float elapsedTime = 0f;
            
            while (elapsedTime < f)
            {
                elapsedTime += Time.deltaTime;
                PlayerStatManager.Instance.Life.SetCurrent(Mathf.Lerp(startHealth, targetHealth, elapsedTime / f));
                yield return null;
            }
            
            PlayerStatManager.Instance.Life.SetCurrent(targetHealth);
        }
    }
}
