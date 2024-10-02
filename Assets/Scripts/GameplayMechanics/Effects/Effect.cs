using UnityEngine;

namespace GameplayMechanics.Effects
{
    public interface IEffect
    {
        EffectType EffectType { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        float Duration { get; set; }
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