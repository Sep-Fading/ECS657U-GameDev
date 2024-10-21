using GameplayMechanics.Character;
using TMPro;
using UnityEngine;

namespace UI
{
    public class HudUpdater : MonoBehaviour
    {
        [SerializeField] private GameObject _healthObject;

        private void Update()
        {
            UpdateHealth();
        }
        public void UpdateHealth()
        {
            string txt = "Life : " + PlayerStatManager.Instance.GetHealth();
            _healthObject.GetComponent<TextMeshProUGUI>().SetText(txt);
        }
    }
}