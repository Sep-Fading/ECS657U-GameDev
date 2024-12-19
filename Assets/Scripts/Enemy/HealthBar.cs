using UnityEngine;
using UnityEngine.UI;

namespace Enemy
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private GameObject enemy;
        public Slider slider;

        private float _maxHealth;
        private float _targetHealth;
        private float _currentDisplayedHealth;
        private float smoothingSpeed = 5f;
        private bool bleeding; // Flag to control lerping only during bleeding
        private EnemyController _enemyController;

        private void Start()
        {
            _enemyController = enemy.GetComponent<EnemyController>();
            if (_enemyController != null)
            {
                if (_enemyController.GetStatManager() == null)
                {
                    _enemyController.setStatManager(new StatManager());
                }

                _maxHealth = _enemyController.GetStatManager().Life.GetAppliedTotal();
                _targetHealth = _currentDisplayedHealth = _enemyController.GetStatManager().Life.GetCurrent();
                slider.value = _currentDisplayedHealth / _maxHealth;
            }
        }

        private void Update()
        {
            // Update target health from enemy's current health
            _targetHealth = _enemyController.GetStatManager().Life.GetCurrent();

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