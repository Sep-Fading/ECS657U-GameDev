using GameplayMechanics.Character;

namespace GameplayMechanics.Effects
{
    /*------------------
    EFFECTS 
    ------------------*/
    
    
    /* ------------------
     TRAVEL NODE EFFECTS
     ------------------*/
    
    // Start Node - is a mastery
    public class SwordShieldStartEffect : SkillTreeEffect
    {
        private float buffMultiplier = 0.1f;
        private float buffAdded = 0.05f;

        public SwordShieldStartEffect()
        {
            this.name = "Sword and Shield Mastery";
            this.description = "10% Increased Melee Damage" +
                               "\n10% Increased Maximum Life" +
                               "\n5% Added Block Effectiveness";
            this.duration = -1f;
            this.effectType = EffectType.Buff;
        }

        public override void Apply()
        {
            PlayerStatManager.Instance.meleeDamage.SetMultiplier(
                PlayerStatManager.Instance.meleeDamage.GetMultiplier()+buffMultiplier);
            PlayerStatManager.Instance.life.SetMultiplier(
                PlayerStatManager.Instance.life.GetMultiplier()+buffMultiplier);
            PlayerStatManager.Instance.blockEffect.SetAdded(
                PlayerStatManager.Instance.blockEffect.GetAdded()+buffAdded);

            this.isActive = true;
        }

        public override void Clear()
        {
            PlayerStatManager.Instance.meleeDamage.SetMultiplier(
                PlayerStatManager.Instance.meleeDamage.GetMultiplier()-buffMultiplier);
            PlayerStatManager.Instance.life.SetMultiplier(
                PlayerStatManager.Instance.life.GetMultiplier()-buffMultiplier);
            PlayerStatManager.Instance.blockEffect.SetAdded(
                PlayerStatManager.Instance.blockEffect.GetAdded()-buffAdded);
            
            this.isActive = false;
        }
    }
    
    // Health Travel Node
    public class IncreasedHPEffect : SkillTreeEffect
    {
        private float buffMultiplier = 0.05f;

        public IncreasedHPEffect()
        {
            this.name = "5% Increased Maximum Life";
            this.description = "Increases the player's base health by 5%";
            this.duration = -1f;
            this.effectType = EffectType.Buff;
        }

        public override void Apply()
        {
            PlayerStatManager.Instance.life.SetMultiplier(
                PlayerStatManager.Instance.life.GetMultiplier()+buffMultiplier);
            
            this.isActive = true;
        }

        public override void Clear()
        {
            PlayerStatManager.Instance.life.SetMultiplier(
                PlayerStatManager.Instance.life.GetMultiplier()-buffMultiplier);
            
            this.isActive = false;
        }
    }
    
    // Melee Damage Travel Node
    public class IncreasedMeleeDamageEffect : SkillTreeEffect
    {
        private float buffMultiplier = 0.10f;

        public IncreasedMeleeDamageEffect()
        {
            this.name = "10% Increased Melee Damage";
            this.description = "Increases the player's base melee damage by 10%";
            this.duration = -1f;
            this.effectType = EffectType.Buff;
        }

        public override void Apply()
        {
            PlayerStatManager.Instance.meleeDamage.SetMultiplier(
                PlayerStatManager.Instance.meleeDamage.GetMultiplier()+buffMultiplier);
            
            this.isActive = true;
        }

        public override void Clear()
        {
            PlayerStatManager.Instance.meleeDamage.SetMultiplier(
                PlayerStatManager.Instance.meleeDamage.GetMultiplier()-buffMultiplier);
            this.isActive = false;
        }
    }
    
    // Block Effectiveness Travel Node
    public class AddedBlockEffect : SkillTreeEffect
    {
        private float addedBlockEffect = 0.01f;

        public AddedBlockEffect()
        {
            this.name = "1% Added Effectiveness of Block";
            this.description = "Increases the player's block effectiveness by 1%";
            this.duration = -1f;
            this.effectType = EffectType.Buff;
        }

        public override void Apply()
        {
            PlayerStatManager.Instance.blockEffect.SetAdded(
                PlayerStatManager.Instance.blockEffect.GetAdded()+addedBlockEffect);

            this.isActive = true;
        }

        public override void Clear()
        {
            PlayerStatManager.Instance.blockEffect.SetAdded(
                PlayerStatManager.Instance.blockEffect.GetAdded()-addedBlockEffect);
            
            this.isActive = false;
        }
    }
    
    /* ------------------
     MASTERY NODE EFFECTS
     ------------------ */
    
    // Armour Mastery
    public class ArmourMasteryEffect : SkillTreeEffect
    {
        float buffMultiplier = 0.35f;

        public ArmourMasteryEffect()
        {
            this.name = "Armour Mastery";
            this.description = "Increases the player's base armour by 35%";
            this.duration = -1f;
            this.effectType = EffectType.Buff;
        }

        public override void Apply()
        {
            PlayerStatManager.Instance.armour.SetMultiplier(
                PlayerStatManager.Instance.armour.GetMultiplier()+buffMultiplier);
            
            this.isActive = true;
        }

        public override void Clear()
        {
            PlayerStatManager.Instance.armour.SetMultiplier(
                PlayerStatManager.Instance.armour.GetMultiplier()-buffMultiplier);
            
            this.isActive = false;
        }
    }
    
    // Versatility Mastery - Converts Evasion into Armour
    public class VersatilityMasteryEffect : SkillTreeEffect
    {
        private float _armourEvMulti = 0.15f;

        public VersatilityMasteryEffect()
        {
            this.name = "Versatile Combatant Mastery";
            this.description = "15% Increased Armour and Evasion";
            this.duration = -1f;
            this.effectType = EffectType.Buff;
        }

        public override void Apply()
        {
            PlayerStatManager.Instance.armour.SetMultiplier(
                PlayerStatManager.Instance.armour.GetMultiplier()+_armourEvMulti);
            PlayerStatManager.Instance.evasion.SetMultiplier(
                PlayerStatManager.Instance.evasion.GetMultiplier()+_armourEvMulti);
            
            this.isActive = true;
        }
        
        public override void Clear()
        {
            PlayerStatManager.Instance.armour.SetMultiplier(
                PlayerStatManager.Instance.armour.GetMultiplier() - _armourEvMulti);
            PlayerStatManager.Instance.evasion.SetMultiplier(
                PlayerStatManager.Instance.evasion.GetMultiplier() - _armourEvMulti); 
            
            this.isActive = false;
        }
    }
}