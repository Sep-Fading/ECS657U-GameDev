using UnityEngine;

namespace GameplayMechanics.Effects
{
    /// <summary>
    /// Interface for all bufss or debuffs within the game,
    /// used to standardise the code and provide a framework to
    /// build skill effects etc...
    /// </summary>

    public interface IEffect
    {
        EffectType effectType { get; set; }
        string name { get; set; }
        string description { get; set; }
        float duration { get; set; }
        public void Apply();
        public void Clear();
    }
    
    /* -------
     Simple Enum to determine if an effect will be a 
     buff or a debuff.
     -------- */
    public enum EffectType
    {
        Buff,
        Debuff
    }
}