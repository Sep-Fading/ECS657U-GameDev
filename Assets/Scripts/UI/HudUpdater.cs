using GameplayMechanics.Character;
using TMPro;
using UnityEngine;

namespace UI
{
    public class HudUpdater : MonoBehaviour
    {
        [SerializeField] private GameObject _healthObject;
        private TextMeshProUGUI _healthText;

        private void Start()
        {
            _healthText = _healthObject.GetComponentInChildren<TextMeshProUGUI>();
        }
        private void LateUpdate()
        {
            UpdateHUD();
        }
        public void UpdateHUD()
        {
            if (PlayerStatManager.Instance != null & XpManager.Instance != null)
            {
                string txtLife = "Life : " + PlayerStatManager.Instance.Life.GetCurrent();
                string txtLevel = "Level: " + XpManager.GetLevel().ToString();
                string txtXP = "Exp: " + XpManager.GetCurrentXp().ToString("F1") + " / " +
                               XpManager.GetLevelUpThreshold().ToString("F1");

                string txt = txtLife + "\n" + txtLevel + "\n" + txtXP;
                _healthText.SetText(txt);
            }
        }
    }
}