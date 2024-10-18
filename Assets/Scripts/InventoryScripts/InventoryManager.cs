using GameplayMechanics.Effects;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace InventoryScripts
{
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
                Debug.Log("Inventory out of space!");
                return;
            }
            inventoryItemsUI[i].GetComponent<TextMeshProUGUI>().
                SetText(inventoryItem.gameItem.GetName()[0].ToString());
        }

        public void MoveToEquipment(InventoryItem inventoryItem, int i)
        {
            inventoryItemsUI[i].GetComponent<TextMeshProUGUI>().SetText("");
            InventoryItem item = Inventory.Instance.Pop(inventoryItem);
            EquipmentType type = Inventory.Instance.Equip(item.equipment);
            Debug.Log(type);
            Debug.Log(item.gameItem.GetName());

            if (type == EquipmentType.NONE)
            {
                Debug.Log("Item is not equippable.");
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
}
