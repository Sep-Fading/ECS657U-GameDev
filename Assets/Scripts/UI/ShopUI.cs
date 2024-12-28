using System;
using System.Collections.Generic;
using GameplayMechanics.Effects;
using InventoryScripts;
using Npcs;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class ShopUI : MonoBehaviour
    {
        [SerializeField] private Sprite swordIcon;
        [SerializeField] private Sprite shieldIcon;
        [SerializeField] private Sprite greatswordIcon;
        [SerializeField] private Sprite axeIcon;
        [SerializeField] private Sprite armorIcon;
        [SerializeField] private Sprite potionIcon;
        [SerializeField] private GameObject[] shopItemPrefab;
        [SerializeField] private GameObject[] shopInventoryPrefab;
        [SerializeField] private Button ShopButton;
        [SerializeField] private Button RerollButton;
        
        private NpcShop npcShop;
        private InventoryItem selectedItem;
        private TextMeshProUGUI previouslySelectedItemText;
        
        private InventoryManager _inventoryManager;
        
        [SerializeField] private TooltipUI _tooltipUI;
    
        // Start is called before the first frame update
        void Awake()
        {
            //gameObject.SetActive(false);
            //npcShop = new NpcShop();
        }

        void Start()
        {
            RerollButton.onClick.AddListener(RerollShop);
            _inventoryManager = GameObject.FindWithTag("Player").GetComponent<InventoryManager>();
        }
        
        void OnEnable()
        {
            UpdateShop();
        }
        
        public void SetNpcShop(NpcShop shop)
        {
            npcShop = shop;
        }

        private void AddClickListenerSell(GameObject o, InventoryItem playerItem)
        {
            EventTrigger trigger = o.AddComponent<EventTrigger>();
            
            EventTrigger.Entry clickEntry = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerClick
            };
            clickEntry.callback.AddListener((data) => OnItemClickSell(playerItem,
                o));
            trigger.triggers.Add(clickEntry);
            
            // Add pointer enter event for showing tooltip
            EventTrigger.Entry enterEntry = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerEnter
            };
            enterEntry.callback.AddListener((data) => _tooltipUI.ShowTooltip(playerItem.gameItem.GetName(),
                playerItem.GetDescription(),
                playerItem.gameItem.GetSellPrice().ToString()));
            trigger.triggers.Add(enterEntry);

            // Add pointer exit event for hiding tooltip
            EventTrigger.Entry exitEntry = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerExit
            };
            exitEntry.callback.AddListener((data) => _tooltipUI.HideTooltip());
            trigger.triggers.Add(exitEntry);
        }

        private void OnItemClickSell(InventoryItem playerItem, GameObject o)
        {
            if (previouslySelectedItemText != null)
            {
                previouslySelectedItemText.color = Color.white;
            }
            TextMeshProUGUI text = o.GetComponentInChildren<TextMeshProUGUI>();
            text.color = Color.yellow;
            selectedItem = playerItem;
            previouslySelectedItemText = text;

            UpdateShopButton(playerItem, "SELL");
        }

        private void AddClickListenerBuy(GameObject prefab, InventoryItem item)
        {
            EventTrigger trigger = prefab.AddComponent<EventTrigger>();
            
            EventTrigger.Entry clickEntry = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerClick
            };
            clickEntry.callback.AddListener((data) => OnItemClickBuy(item,
                prefab));
            trigger.triggers.Add(clickEntry);
            
            // Add pointer enter event for showing tooltip
            EventTrigger.Entry enterEntry = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerEnter
            };
            enterEntry.callback.AddListener((data) => _tooltipUI.ShowTooltip(item.gameItem.GetName(),
                item.GetDescription(),
                item.gameItem.GetBuyPrice().ToString()));
            trigger.triggers.Add(enterEntry);

            // Add pointer exit event for hiding tooltip
            EventTrigger.Entry exitEntry = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerExit
            };
            exitEntry.callback.AddListener((data) => _tooltipUI.HideTooltip());
            trigger.triggers.Add(exitEntry); 
        }
        
        private void OnItemClickBuy(InventoryItem item, GameObject prefab)
        {
            if (previouslySelectedItemText != null)
            {
                previouslySelectedItemText.color = Color.white;
            }
            TextMeshProUGUI text = prefab.GetComponentInChildren<TextMeshProUGUI>();
            text.color = Color.yellow;
            selectedItem = item;
            previouslySelectedItemText = text;
            
            UpdateShopButton(item, "BUY");
            
            //Debug.Log($"Selected item: {selectedItem.gameItem.GetName()}, {selectedItem.equipment.GetEquipmentType()}");
        }

        private void UpdateShopButton(InventoryItem item, string action)
        {
            if (action == "BUY")
            {
                ShopButton.onClick.RemoveAllListeners();
                ShopButton.onClick.AddListener(() => Buy(item));
                ShopButton.GetComponentInChildren<TextMeshProUGUI>().text =
                    $"Buy for {item.gameItem.GetBuyPrice()} Gold";
            }
            else if (action == "SELL")
            {
                ShopButton.onClick.RemoveAllListeners();
                ShopButton.onClick.AddListener(() => Sell(item));
                ShopButton.GetComponentInChildren<TextMeshProUGUI>().text = $"Sell for {item.gameItem.GetSellPrice()} Gold";
            }
        }

        private void Sell(InventoryItem item)
        {
            npcShop.Sell(item);
            UpdateShop();
            _inventoryManager.UpdateInventoryUI();
            selectedItem = null;
            previouslySelectedItemText.color = Color.white;
            previouslySelectedItemText = null;
        }

        private void Buy(InventoryItem item)
        {
            if (Inventory.GetGold() >= item.gameItem.GetBuyPrice())
            {
                InventoryItem bought = npcShop.Buy(item);
                Inventory.Instance.Push(bought);
                selectedItem = null;
                previouslySelectedItemText.color = Color.white;
                previouslySelectedItemText = null;
                _inventoryManager.UpdateInventoryUI();
            }
            UpdateShop();
        }

        private void UpdateShop()
        {
             // Setup items in shop
            List<InventoryItem> shopItems = npcShop.GetForSale();
            for (int i = 0; i < shopItemPrefab.Length; i++)
            {
                // Check if shop items are in bounds
                if (i >= shopItems.Count)
                {
                    shopItemPrefab[i].gameObject.SetActive(false);
                }
                else
                {
                    shopItemPrefab[i].gameObject.SetActive(true);
                    shopItemPrefab[i].gameObject.GetComponentInChildren<TextMeshProUGUI>().text =
                        shopItems[i].gameItem.GetName();

                    Image itemSprite = shopItemPrefab[i].gameObject.GetComponentInChildren<Image>();
                    if (shopItems[i].equipment == null)
                    {
                        itemSprite.sprite = potionIcon;
                    }
                    else if (shopItems[i].equipment.GetEquipmentType() == EquipmentType.MAINHAND)
                    {
                        itemSprite.sprite = swordIcon;
                    }
                    else if (shopItems[i].equipment.GetEquipmentType() == EquipmentType.AXE)
                    {
                        itemSprite.sprite = axeIcon;
                    }
                    else if (shopItems[i].equipment.GetEquipmentType() == EquipmentType.GREATSWORD)
                    {
                        itemSprite.sprite = greatswordIcon;
                    }
                    else if (shopItems[i].equipment.GetEquipmentType() == EquipmentType.OFFHAND)
                    {
                        itemSprite.sprite = shieldIcon;
                    }
                    else if (shopItems[i].equipment.GetEquipmentType() == EquipmentType.ARMOR)
                    {
                        itemSprite.sprite = armorIcon;
                    }

                    AddClickListenerBuy(shopItemPrefab[i], shopItems[i]);
                }
            }
            
            // Inventory items from player
            List<InventoryItem> playerItems = Inventory.Instance.GetInventory();
            for (int i = 0; i < shopInventoryPrefab.Length; i++)
            {
                if (i >= playerItems.Count)
                {
                    shopInventoryPrefab[i].gameObject.SetActive(false);
                }
                else
                {
                    shopInventoryPrefab[i].gameObject.SetActive(true);
                    shopInventoryPrefab[i].gameObject.GetComponentInChildren<TextMeshProUGUI>().text =
                        playerItems[i].gameItem.GetName();
                    AddClickListenerSell(shopInventoryPrefab[i], playerItems[i]);
                }
            }
        }

        private void RerollShop()
        {
            if (Inventory.GetGold() >= 100)
            {
                npcShop.RerollShop();
            }
            UpdateShop();
        }

        public void OpenShop()
        {
            gameObject.SetActive(true);
            UIManager.Instance.PushUI(gameObject);
        }
        
        public void CloseShop()
        {
            gameObject.SetActive(false);
            UIManager.Instance.PopUIByGameObject(gameObject);
        }
    }
}
