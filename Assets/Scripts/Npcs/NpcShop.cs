using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using GameplayMechanics.Effects;
using InventoryScripts;

namespace Npcs
{
    public class NpcShop
    {
        List<InventoryItem> _forSale = new List<InventoryItem>();
        NpcShop(List<InventoryItem> forSale = null)
        {
            if (forSale != null)
            {
                this._forSale = forSale;
                return;
            }
            // Generate a random list of items
            GenerateRandomShopItems(5);
        }

        // <summary>
        /// Generates a random list of items for the shop.
        /// It gets rid of the old items.
        // </summary>
        private void GenerateRandomShopItems(int numberOfItems)
        {
            _forSale.Clear();
            for (int i = 0; i < numberOfItems; i++)
            {
                int sellPrice = RandomNumberGenerator.GetInt32(0, 300);
                int buyPrice = RandomNumberGenerator.GetInt32(0, 900);
                EquipmentType[] equipmentTypes = (EquipmentType[])Enum.GetValues(typeof(EquipmentType));
                EquipmentType equipmentType = equipmentTypes[RandomNumberGenerator.GetInt32(0, equipmentTypes.Length)];
                ItemType[] itemTypes = (ItemType[])Enum.GetValues(typeof(ItemType));
                ItemType itemType = itemTypes[RandomNumberGenerator.GetInt32(0, itemTypes.Length)];

                String name = EquipmentNameGenerator.GenerateEquipmentName(equipmentType);

                GameItem item; 
                if (itemType == ItemType.CONSUMABLE)
                {
                    item = new GameItem("Healing Potion" + i,
                        "Consuming this potion recovers 50 health over 1 second.", 1,
                        ItemType.CONSUMABLE, 50, 100);
                }
                else
                {
                    item = new GameItem(name, "", 1,
                        ItemType.EQUIPMENT, sellPrice, buyPrice);
                }

                Equipment equipment;
                // Need some  equipment effects generator?
                if (equipmentType != EquipmentType.NONE)
                {
                    List<EquipmentEffect> effects = EquipmentEffectGenerator.Instance.GenerateRandomEquipmentEffects(
                        equipmentType);
                    equipment = new Equipment(name, "", equipmentType,
                        null, effects);
                }
                else
                {
                    equipment = null;
                }

                InventoryItem inventoryItem = new InventoryItem(item, equipment);
                
                _forSale.Add(inventoryItem);
            }
        }
        
        

        public InventoryItem Buy(InventoryItem item)
        {
            if (_forSale.Contains(item))
            {
                InventoryItem toBuy = _forSale[_forSale.IndexOf(item)];
                Inventory.TakeGold(item.gameItem.GetBuyPrice());
                _forSale.Remove(item);
                return toBuy;
            }

            return null;
        }

        public void Sell(InventoryItem item)
        {
            if (Inventory.Instance.ItemExistsInInventory(item))
            {
                InventoryItem toSell = Inventory.Instance.Pop(item);
                Inventory.GiveGold(item.gameItem.GetSellPrice());
                _forSale.Add(item);
            }
        }

        public void RerollShop()
        {
            GenerateRandomShopItems(5);
        }
    }
}
