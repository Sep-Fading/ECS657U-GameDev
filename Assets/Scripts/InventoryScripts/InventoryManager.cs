using TMPro;
using UnityEditor;
using UnityEngine;

namespace InventoryScripts
{
    public class InventoryManager : MonoBehaviour
    {
        [SerializeField] private GameObject[] _inventoryItemsUI = new GameObject[3];
        [SerializeField] private GameObject[] _equippedItemsUI = new GameObject[3];
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
            if (i < 0 || i >= _inventoryItemsUI.Length)
            {
                Debug.Log("Inventory out of space!");
                return;
            }
            _inventoryItemsUI[i].GetComponent<TextMeshProUGUI>().
                SetText(inventoryItem.gameItem.GetName()[0].ToString());
        }
    }
}
