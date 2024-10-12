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
        }

        public override void Clear()
        {
            PlayerStatManager.Instance.meleeDamage.SetMultiplier(
                PlayerStatManager.Instance.meleeDamage.GetMultiplier()-buffMultiplier);
            PlayerStatManager.Instance.life.SetMultiplier(
                PlayerStatManager.Instance.life.GetMultiplier()-buffMultiplier);
            PlayerStatManager.Instance.blockEffect.SetAdded(
                PlayerStatManager.Instance.blockEffect.GetAdded()-buffAdded);
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
        }

        public override void Clear()
        {
            PlayerStatManager.Instance.life.SetMultiplier(
                PlayerStatManager.Instance.life.GetMultiplier()-buffMultiplier);
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
            PlayerStatManager.Instance.meleeDamage.SetMultiplier(buffMultiplier);
        }

        public override void Clear()
        {
            PlayerStatManager.Instance.meleeDamage.SetMultiplier(-buffMultiplier);
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
            PlayerStatManager.Instance.blockEffect.SetAdded(addedBlockEffect);
        }

        public override void Clear()
        {
            PlayerStatManager.Instance.blockEffect.SetAdded(-addedBlockEffect);
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
            PlayerStatManager.Instance.armour.SetMultiplier(buffMultiplier);
        }

        public override void Clear()
        {
            PlayerStatManager.Instance.armour.SetMultiplier(-buffMultiplier);
        }
    }
    
    // Versatility Mastery - Converts Evasion into Armour
    public class VersatilityMasteryEffect : SkillTreeEffect
    {
        private float _storeEvFlat;
        private float _storeEvMutli;

        public VersatilityMasteryEffect()
        {
            this.name = "Steel Heart Mastery";
            this.description = "Converts all evasion to armour";
            this.duration = -1f;
            this.effectType = EffectType.Buff;
        }

        public override void Apply()
        {
            float evFlat = PlayerStatManager.Instance.evasion.GetFlat();
            float evMulti = PlayerStatManager.Instance.evasion.GetMultiplier();

            this._storeEvFlat = evFlat;
            this._storeEvMutli = evMulti;
            
            PlayerStatManager.Instance.armour.SetFlat(
                PlayerStatManager.Instance.armour.GetFlat()+evFlat);
            PlayerStatManager.Instance.armour.SetMultiplier(
                PlayerStatManager.Instance.armour.GetMultiplier()+evMulti);

            PlayerStatManager.Instance.evasion.SetFlat(0);
            PlayerStatManager.Instance.evasion.SetMultiplier(0);
        }
        
        public override void Clear()
        {
            PlayerStatManager.Instance.armour.SetFlat(
                PlayerStatManager.Instance.armour.GetFlat()-this._storeEvFlat);
            PlayerStatManager.Instance.armour.SetMultiplier(
                PlayerStatManager.Instance.armour.GetMultiplier()-this._storeEvMutli);
            
            PlayerStatManager.Instance.evasion.SetFlat(
                PlayerStatManager.Instance.evasion.GetFlat()+this._storeEvFlat);
            PlayerStatManager.Instance.evasion.SetMultiplier(
                PlayerStatManager.Instance.evasion.GetMultiplier()+this._storeEvMutli);
        }
    }
}