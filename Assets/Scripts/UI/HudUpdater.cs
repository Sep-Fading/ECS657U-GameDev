using GameplayMechanics.Character;
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

        private void LateUpdate()
        {
            UpdateHUD();
        }
        public void UpdateHUD()
        {
            if (PlayerStatManager.Instance != null & XpManager.Instance != null)
            {
                float currentHealth = PlayerStatManager.Instance.Life.GetCurrent();
                float maxHealth = PlayerStatManager.Instance.Life.GetAppliedTotal();
                HealthBar.value = currentHealth / maxHealth;
                
                if (HealthText != null)
                {
                    HealthText.text = currentHealth + "/" + maxHealth;
                }
                
                float currentXP = XpManager.GetCurrentXp();
                float levelUpThreshold = XpManager.GetLevelUpThreshold();
                XPBar.value = currentXP / levelUpThreshold;
                
                if (LevelText != null)
                {
                    LevelText.text = "Level " + XpManager.GetLevel();
                }
            }
        }
    }
}