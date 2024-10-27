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
        
        private StatManager _statManager;
        private PlayerStatManager _playerStatManager;
        
        BleedEffect(float duration, StatManager statManager)
        {
            this.effectType = EffectType.Debuff;
            this.name = "Bleeding";
            this.description = $"Taking {PlayerStatManager.Instance.Bleed.GetAppliedTotal().ToString("1F")}" +
                               $" physical damage over {duration.ToString("F1")} seconds.";
            this.duration = duration;
            this._statManager = statManager;
            Apply();
        }
        
        
        BleedEffect(float flat, float multiplier, float duration)
        {
            this.effectType = EffectType.Debuff;
            this.name = "Bleeding";
            this.description = $"Taking {(flat * multiplier).ToString("1F")}" +
                               $" physical damage over {duration.ToString("F1")} seconds.";
            this.duration = duration;
            _playerStatManager = PlayerStatManager.Instance;
            Apply();
        }
        
        public void Apply()
        {
            if (this._playerStatManager == null)
            {
                _statManager.Life.SetCurrent(
                    Mathf.Lerp(_statManager.Life.GetCurrent(),
                        _statManager.Life.GetCurrent() - PlayerStatManager.Instance.Bleed.GetAppliedTotal(),
                        duration));
            }
            else
            {
                PlayerStatManager.Instance.Life.SetCurrent(
                    Mathf.Lerp(PlayerStatManager.Instance.Life.GetCurrent(),
                        PlayerStatManager.Instance.Life.GetCurrent() - _statManager.Bleed.GetAppliedTotal(),
                        duration));
            }
        }

        public void Clear()
        {
            throw new System.NotImplementedException();
        }
    }
}