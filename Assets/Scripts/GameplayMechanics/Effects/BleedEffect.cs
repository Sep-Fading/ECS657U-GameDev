using System.Collections;
using Enemy;
using GameplayMechanics.Character;
using UnityEngine;

namespace GameplayMechanics.Effects
{
    public class BleedEffect : IEffect
    {
        public EffectType effectType { get; set; }
        public string name { get; set; } 
        public string description { get; set; } 
        public float duration { get; set; }

        private StatManager _statManager; // For enemies
        private PlayerStatManager _playerStatManager; // For player
        private HealthBar _healthBar;
        private float _totalDamage;
        private float _tickInterval = 0.5f;
        private MonoBehaviour _coroutineStarter; // To start coroutine correctly

        // Constructor for applying bleed to an enemy
        public BleedEffect(float duration, StatManager statManager, HealthBar healthBar, MonoBehaviour coroutineStarter)
        {
            this.effectType = EffectType.Debuff;
            this.name = "Bleeding";
            this.description = $"Taking {PlayerStatManager.Instance.Bleed.GetAppliedTotal().ToString("F1")}" +
                               $" physical damage over {duration.ToString("F1")} seconds.";
            this.duration = duration;
            _statManager = statManager;
            _healthBar = healthBar;
            _coroutineStarter = coroutineStarter;

            _totalDamage = PlayerStatManager.Instance.Bleed.GetAppliedTotal();
            _healthBar.SetBleeding(true);
            Apply();
        }

        // Constructor for applying bleed to the player
        public BleedEffect(float flat, float multiplier, float duration, MonoBehaviour coroutineStarter)
        {
            this.effectType = EffectType.Debuff;
            this.name = "Bleeding";
            this.description = $"Taking {(flat * multiplier).ToString("F1")}" +
                               $" physical damage over {duration.ToString("F1")} seconds.";
            this.duration = duration;
            _playerStatManager = PlayerStatManager.Instance;
            _coroutineStarter = coroutineStarter;

            _totalDamage = flat * multiplier;
            Apply();
        }

        private IEnumerator ApplyBleed()
        {
            float elapsedTime = 0f;
            float damagePerTick = _totalDamage / (duration / _tickInterval);

            while (elapsedTime < duration)
            {
                if (_statManager != null)
                {
                    // Apply bleed damage to enemy
                    _statManager.Life.SetCurrent(_statManager.Life.GetCurrent() - damagePerTick);
                }
                else if (_playerStatManager != null)
                {
                    // Apply bleed damage to player
                    _playerStatManager.Life.SetCurrent(_playerStatManager.Life.GetCurrent() - damagePerTick);
                }

                elapsedTime += _tickInterval;
                yield return new WaitForSeconds(_tickInterval);
            }

            // End bleed effect
            if (_healthBar)
            {
                _healthBar.SetBleeding(false);
            }
        }

        public void Apply()
        {
            _coroutineStarter.StartCoroutine(ApplyBleed()); 
        }

        public void Clear()
        {
            if (_healthBar != null)
            {
                _healthBar.SetBleeding(false);
            }
        }
    }
}
