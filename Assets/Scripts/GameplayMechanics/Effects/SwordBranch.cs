using GameplayMechanics.Character;
using Unity.VisualScripting;
using UnityEditor.UI;
using UnityEngine.Networking.PlayerConnection;

namespace GameplayMechanics.Effects
{
    /*------------------
    EFFECTS 
    ------------------*/
    
    
    /* ------------------
     TRAVEL NODE EFFECTS
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
            PlayerStatManager.Instance.life.multiplier += buffMultiplier;
        }

        public void Clear()
        {
            PlayerStatManager.Instance.life.multiplier -= buffMultiplier;
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
            PlayerStatManager.Instance.meleeDamage.multiplier += buffMultiplier;
        }

        public void Clear()
        {
            PlayerStatManager.Instance.meleeDamage.multiplier -= buffMultiplier;
        }
    }
    
    // Block Effectiveness Travel Node
    public class IncreasedBlockEffect : SkillTreeEffect
    {
        private float addedBlockEffect = 0.01f;

        IncreasedBlockEffect()
        {
            this.name = "1% Added Effectiveness of Block";
            this.description = "Increases the player's block effectiveness by 1%";
            this.duration = -1f;
            this.effectType = EffectType.Buff;
            this.Apply();
        }

        public void Apply()
        {
            PlayerStatManager.Instance.blockEffect.added += addedBlockEffect;
        }

        public void Clear()
        {
            PlayerStatManager.Instance.blockEffect.added -= addedBlockEffect;
        }
    }
    
    /* ------------------
     MASTERY NODE EFFECTS
     ------------------ */
    
    // Armour Mastery
    public class ArmourMasteryEffect : SkillTreeEffect
    {
        float buffMultiplier = 0.35f;

        ArmourMasteryEffect()
        {
            this.name = "Armour Mastery";
            this.description = "Increases the player's base armour by 35%";
            this.duration = -1f;
            this.effectType = EffectType.Buff;
            this.Apply();
        }

        public void Apply()
        {
            PlayerStatManager.Instance.armour.multiplier += buffMultiplier;
        }

        public void Clear()
        {
            PlayerStatManager.Instance.armour.multiplier -= buffMultiplier;
        }
    }
    
    // Steel Heart Mastery - Converts Evasion into Armour
    public class SteelHeartMasteryEffect : SkillTreeEffect
    {
        private float _storeEvFlat;
        private float _storeEvMutli;
        SteelHeartMasteryEffect()
        {
            this.name = "Steel Heart Mastery";
            this.description = "Converts all evasion to armour";
            this.duration = -1f;
            this.effectType = EffectType.Buff;
            this.Apply();
        }

        public void Apply()
        {
            float evFlat = PlayerStatManager.Instance.evasion.flat;
            float evMulti = PlayerStatManager.Instance.evasion.multiplier;

            this._storeEvFlat = evFlat;
            this._storeEvMutli = evMulti;
            
            PlayerStatManager.Instance.armour.flat += evFlat;
            PlayerStatManager.Instance.armour.multiplier += evMulti;

            PlayerStatManager.Instance.evasion.flat = 0;
            PlayerStatManager.Instance.evasion.multiplier = 0;
        }

        public void Clear()
        {
            PlayerStatManager.Instance.armour.flat -= this._storeEvFlat;
            PlayerStatManager.Instance.armour.multiplier -= this. _storeEvFlat;
            
            PlayerStatManager.Instance.evasion.flat = this._storeEvFlat;
            PlayerStatManager.Instance.evasion.multiplier = this._storeEvMutli;
        }
    }
}