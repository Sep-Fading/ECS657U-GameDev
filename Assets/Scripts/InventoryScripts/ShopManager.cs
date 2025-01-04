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
            DontDestroyOnLoad(gameObject);
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

        public void PrepareForSceneTransition()
        {
            if (shopUI != null)
            {
                shopUI.SetActive(true); // Enable the shopUI before transitioning scenes
            }
        }

        public void InitializeAfterSceneLoad()
        {
            if (shopUI != null)
            {
                shopUI.SetActive(false); // Disable the shopUI after the new scene is loaded
            }
        }
    }
}