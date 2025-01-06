using GameplayMechanics.Effects;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace InventoryScripts
{
    /// <summary>
    /// Handles how the Inventory item reacts when pressed
    /// </summary>
    public class EquipmentClickHandler : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        private InventoryManager _inventoryManager;
        [SerializeField] private int equipmentIndex;
        [SerializeField] private EquipmentType equipmentType;
        [SerializeField] private TooltipUI _tooltipUI;

        private void Start()
        {
            _inventoryManager = GameObject.FindWithTag("Player").GetComponent<InventoryManager>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log($"OnPointerClick: {equipmentIndex}");
            _inventoryManager.MoveToInventory(equipmentType, equipmentIndex);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            switch (equipmentIndex)
            {
                case 0:
                    if (Inventory.Instance.EquippedMainHandItem != null)
                    {
                        GameItem item;
                        if (Inventory.Instance.EquippedMainHandItem.gameItem != null)
                        {
                            item = Inventory.Instance.EquippedMainHandItem.gameItem;
                        }
                        else
                        {
                            item = Inventory.Instance.EquippedMainHandItem.equipment.Item;
                            Inventory.Instance.EquippedMainHandItem.gameItem = item;
                        } 
                        _tooltipUI.ShowTooltip(item.GetName(),
                            Inventory.Instance.EquippedMainHandItem.GetDescription(),
                            item.GetSellPrice().ToString());
                    }
                    break;
                case 1:
                    if (Inventory.Instance.EquippedArmourItem != null)
                    {
                        GameItem item;
                        if (Inventory.Instance.EquippedArmourItem.gameItem != null)
                        {
                            item = Inventory.Instance.EquippedArmourItem.gameItem;
                        }
                        else
                        {
                            item = Inventory.Instance.EquippedArmourItem.equipment.Item;
                            Inventory.Instance.EquippedArmourItem.gameItem = item;
                        } 
                        _tooltipUI.ShowTooltip(item.GetName(),
                            Inventory.Instance.EquippedArmourItem.GetDescription(),
                            Inventory.Instance.EquippedArmourItem.gameItem.GetSellPrice().ToString());
                    }
                    break;
                case 2:
                    if (Inventory.Instance.EquippedOffHandItem != null)
                    {
                        GameItem item;
                        if (Inventory.Instance.EquippedOffHandItem.gameItem != null)
                        {
                            item = Inventory.Instance.EquippedOffHandItem.gameItem;
                        }
                        else
                        {
                            item = Inventory.Instance.EquippedOffHandItem.equipment.Item;
                            Inventory.Instance.EquippedOffHandItem.gameItem = item;
                        }

                        _tooltipUI.ShowTooltip(item.GetName(),
                                Inventory.Instance.EquippedOffHandItem.GetDescription(),
                                Inventory.Instance.EquippedOffHandItem.gameItem.GetSellPrice().ToString());
                    }

                    break;
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _tooltipUI.HideTooltip();
        }
    }
}