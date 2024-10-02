using UnityEngine;

namespace GameplayMechanics.Effects
{
    public interface IEffect
    {
        EffectType effectType { get; set; }
        string name { get; set; }
        string description { get; set; }
        float duration { get; set; }
        GameObject target { get; set; }
        public void Apply();
        public void Clear();
    }

    public enum EffectType
    {
        Buff,
        Debuff
    }
}