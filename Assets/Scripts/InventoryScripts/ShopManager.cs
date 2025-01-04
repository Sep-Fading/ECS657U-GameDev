using UnityEngine;

namespace InventoryScripts
{
    public class ShopManager : MonoBehaviour
    {
        public static ShopManager Instance { get; private set; }

        private GameObject shopUI;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        public static ShopManager EnsureInstanceExists()
        {
            if (Instance == null)
            {
                GameObject shopManagerObject = new GameObject("ShopManager");
                Instance = shopManagerObject.AddComponent<ShopManager>();
                DontDestroyOnLoad(shopManagerObject);
            }
            return Instance;
        }

        public void RegisterShopUI(GameObject ui)
        {
            shopUI = ui;
        }

        public GameObject GetShopUI()
        {
            if (shopUI == null)
            {
                Debug.LogWarning("ShopUI is not registered. Ensure it's properly initialized.");
            }
            return shopUI;
        }
    }
}