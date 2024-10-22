using System;
using UnityEngine.UI;
using UnityEngine;
using Enemy;

    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private GameObject enemy;

        public Slider slider;
        private float _maxHealth;
        private float _health;

        private void Awake()
        {
            _maxHealth = enemy.GetComponent<EnemyController>().maxHealth;
            slider.value = _health;
        }

        private void Update()
        {
            SetHealth(enemy.GetComponent<EnemyController>().currentHealth);
        }

        private void SetHealth(float health)
        {
            _health = health;
            slider.value = _health / _maxHealth;
        }
}
