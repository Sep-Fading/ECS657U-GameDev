using GameplayMechanics.Character;
using InventoryScripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HudUpdater : MonoBehaviour
    {
        [SerializeField] private Slider HealthBar;
        [SerializeField] private Slider XPBar;
        [SerializeField] private TextMeshProUGUI HealthText;
        [SerializeField] private TextMeshProUGUI LevelText;
        [SerializeField] private TextMeshProUGUI GoldText;
        
        private void Start()
        {
            Debug.Log(PlayerStatManager.Instance.Life.GetCurrent());
        }

        private void LateUpdate()
        {
            UpdateHUD();
        }
        public void UpdateHUD()
        {
            if (PlayerStatManager.Instance != null && XpManager.Instance != null)
            {
                float currentHealth = PlayerStatManager.Instance.Life.GetCurrent();
                float maxHealth = PlayerStatManager.Instance.Life.GetAppliedTotal();
                HealthBar.value = currentHealth / maxHealth;
                
                if (HealthText != null)
                {
                    HealthText.text = currentHealth.ToString("N0") +
                                      "/" + maxHealth.ToString("N0");
                }
                
                float currentXP = XpManager.GetCurrentXp();
                float levelUpThreshold = XpManager.GetLevelUpThreshold();
                XPBar.value = currentXP / levelUpThreshold;
                
                if (LevelText != null)
                {
                    LevelText.text = "Level " + XpManager.GetLevel();
                }
                
                if (GoldText != null)
                {
                    GoldText.text = Inventory.GetGold().ToString();
                }
            }
        }
    }
}