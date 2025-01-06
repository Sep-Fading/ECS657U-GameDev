using UnityEngine;
using UnityEngine.UI;

namespace Enemy
{
    /// <summary>
    /// Handles how the health bar functions for the Bosses when fighting them
    /// </summary>
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private GameObject enemy;
        public Slider slider;

        private float _maxHealth;
        private float _targetHealth;
        private float _currentDisplayedHealth;
        private float smoothingSpeed = 5f;
        private bool bleeding; // Flag to control lerping only during bleeding
        private AbstractEnemy abstractEnemy;

        private void Awake()
        {
            slider = GetComponent<Slider>();
        }
        private void Start()
        {
            abstractEnemy = transform.GetComponentInParent<AbstractEnemy>();
            if (abstractEnemy != null)
            {
                if (abstractEnemy.GetStatManager() == null)
                {
                    abstractEnemy.stats = (new StatManager());
                }

                _maxHealth = abstractEnemy.GetStatManager().Life.GetAppliedTotal();
                _targetHealth = _currentDisplayedHealth = abstractEnemy.GetStatManager().Life.GetCurrent();
                slider.value = _currentDisplayedHealth / _maxHealth;
            }
        }

        private void Update()
        {
            // Update target health from enemy's current health
            _targetHealth = abstractEnemy.GetStatManager().Life.GetCurrent();

            // Smoothly transition slider value only if bleeding
            if (bleeding)
            {
                _currentDisplayedHealth = Mathf.Lerp(_currentDisplayedHealth, _targetHealth, smoothingSpeed * Time.deltaTime);
            }
            else
            {
                _currentDisplayedHealth = _targetHealth; // Instantly set without lerp
            }

            slider.value = _currentDisplayedHealth / _maxHealth;
        }

        public void SetBleeding(bool isBleeding)
        {
            bleeding = isBleeding;
        }
        
        public bool GetBleeding() => bleeding;
    }
}