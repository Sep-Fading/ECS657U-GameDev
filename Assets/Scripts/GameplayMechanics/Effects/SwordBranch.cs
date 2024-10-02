using GameplayMechanics.Character;
using UnityEditor.UI;
using UnityEngine.Networking.PlayerConnection;

namespace GameplayMechanics.Effects
{
    
    
    
    /*------------------
    EFFECTS 
    ------------------*/
    
    // Health Travel Node
    public class IncreasedHPEffect : SkillTreeEffect
    {
        private float buffMultiplier = 0.05f;
        
        IncreasedHPEffect()
        {
            this.name = "5% Increased Maximum Life";
            this.description = "Increases the player's base health by 5%";
            this.duration = -1f;
            this.effectType = EffectType.Buff;
            this.Apply();
        }

        public void Apply()
        {
            PlayerStatManager.life.multiplier += buffMultiplier;
        }

        public void Clear()
        {
            PlayerStatManager.life.multiplier -= buffMultiplier;
        }
    }
    
    // Melee Damage Travel Node
    public class IncreasedMeleeDamageEffect : SkillTreeEffect
    {
        private float buffMultiplier = 0.10f;

        IncreasedMeleeDamageEffect()
        {
            this.name = "10% Increased Melee Damage";
            this.description = "Increases the player's base melee damage by 10%";
            this.duration = -1f;
            this.effectType = EffectType.Buff;
            this.Apply();
        }

        public void Apply()
        {
            PlayerStatManager.meleeDamage.multiplier += buffMultiplier;
        }

        public void Clear()
        {
            PlayerStatManager.meleeDamage.multiplier -= buffMultiplier;
        }
    }
}