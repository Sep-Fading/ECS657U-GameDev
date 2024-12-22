using GameplayMechanics.Character;
using InventoryScripts;
using Player;
using UnityEngine;

namespace GameplayMechanics.Effects
{
    /* ------------------
     TRAVEL NODE EFFECTS
     ------------------*/
    
    // Start Node - is a mastery
    public class SwordShieldStartEffect : SkillTreeEffect
    {
        private float _buffMultiplier = 0.1f;
        private float _buffAdded = 0.05f;

        public SwordShieldStartEffect()
        {
            this.name = "Sword and Shield Mastery";
            this.description = "10% Increased Melee Damage" +
                               "\n10% Increased Maximum Life" +
                               "\n5% Added Block Effectiveness";
            duration = -1f;
            effectType = EffectType.Buff;
        }

        public override void Apply()
        {
            PlayerStatManager.Instance.MeleeDamage.SetMultiplier(
                PlayerStatManager.Instance.MeleeDamage.GetMultiplier()+_buffMultiplier);
            PlayerStatManager.Instance.Life.SetMultiplier(
                PlayerStatManager.Instance.Life.GetMultiplier()+_buffMultiplier);
            PlayerStatManager.Instance.BlockEffect.SetAdded(
                PlayerStatManager.Instance.BlockEffect.GetAdded()+_buffAdded);

            this.isActive = true;
        }

        public override void Clear()
        {
            PlayerStatManager.Instance.MeleeDamage.SetMultiplier(
                PlayerStatManager.Instance.MeleeDamage.GetMultiplier()-_buffMultiplier);
            PlayerStatManager.Instance.Life.SetMultiplier(
                PlayerStatManager.Instance.Life.GetMultiplier()-_buffMultiplier);
            PlayerStatManager.Instance.BlockEffect.SetAdded(
                PlayerStatManager.Instance.BlockEffect.GetAdded()-_buffAdded);
            
            this.isActive = false;
        }
    }
    
    // Health Travel Node
    public class IncreasedHpEffect : SkillTreeEffect
    {
        private float _buffMultiplier = 0.05f;

        public IncreasedHpEffect()
        {
            this.name = "5% Increased Maximum Life";
            this.description = "Increases the player's base health by 5%";
            this.duration = -1f;
            this.effectType = EffectType.Buff;
        }

        public override void Apply()
        {
            PlayerStatManager.Instance.Life.SetMultiplier(
                PlayerStatManager.Instance.Life.GetMultiplier()+_buffMultiplier);
            
            this.isActive = true;
        }

        public override void Clear()
        {
            PlayerStatManager.Instance.Life.SetMultiplier(
                PlayerStatManager.Instance.Life.GetMultiplier()-_buffMultiplier);
            
            this.isActive = false;
        }
    }
    
    // Melee Damage Travel Node
    public class IncreasedMeleeDamageEffect : SkillTreeEffect
    {
        private float _buffMultiplier = 0.10f;

        public IncreasedMeleeDamageEffect()
        {
            this.name = "10% Increased Melee Damage";
            this.description = "Increases the player's base melee damage by 10%";
            this.duration = -1f;
            this.effectType = EffectType.Buff;
        }

        public override void Apply()
        {
            PlayerStatManager.Instance.MeleeDamage.SetMultiplier(
                PlayerStatManager.Instance.MeleeDamage.GetMultiplier()+_buffMultiplier);
            
            this.isActive = true;
        }

        public override void Clear()
        {
            PlayerStatManager.Instance.MeleeDamage.SetMultiplier(
                PlayerStatManager.Instance.MeleeDamage.GetMultiplier()-_buffMultiplier);
            this.isActive = false;
        }
    }
    
    // Block Effectiveness Travel Node
    public class AddedBlockEffect : SkillTreeEffect
    {
        private float _addedBlockEffect = 0.025f;

        public AddedBlockEffect()
        {
            this.name = "1% Added Effectiveness of Block";
            this.description = "Increases the player's block effectiveness by 2.5%";
            this.duration = -1f;
            this.effectType = EffectType.Buff;
        }

        public override void Apply()
        {
            PlayerStatManager.Instance.BlockEffect.SetAdded(
                PlayerStatManager.Instance.BlockEffect.GetAdded()+_addedBlockEffect);

            this.isActive = true;
        }

        public override void Clear()
        {
            PlayerStatManager.Instance.BlockEffect.SetAdded(
                PlayerStatManager.Instance.BlockEffect.GetAdded()-_addedBlockEffect);
            
            this.isActive = false;
        }
    }
    
    /* ------------------
     MASTERY NODE EFFECTS
     ------------------ */
    
    // Armour Mastery
    public class ArmourMasteryEffect : SkillTreeEffect
    {
        float _buffMultiplier = 0.35f;

        public ArmourMasteryEffect()
        {
            this.name = "Armour Mastery";
            this.description = "Increases the player's base armour by 35%";
            this.duration = -1f;
            this.effectType = EffectType.Buff;
        }

        public override void Apply()
        {
            PlayerStatManager.Instance.Armour.SetMultiplier(
                PlayerStatManager.Instance.Armour.GetMultiplier()+_buffMultiplier);
            
            this.isActive = true;
        }

        public override void Clear()
        {
            PlayerStatManager.Instance.Armour.SetMultiplier(
                PlayerStatManager.Instance.Armour.GetMultiplier()-_buffMultiplier);
            
            this.isActive = false;
        }
    }
    
    // Versatility Mastery - Converts Evasion into Armour
    public class VersatilityMasteryEffect : SkillTreeEffect
    {
        private float _damageMulti = 0.35f;

        public VersatilityMasteryEffect()
        {
            this.name = "Versatile Combatant";
            this.description = "35% Increased damage while not wearing a body armour";
            this.duration = -1f;
            this.effectType = EffectType.Buff;
        }

        public override void Apply()
        {
            if (Inventory.Instance.EquippedArmour == null)
            {
                PlayerStatManager.Instance.MeleeDamage.SetMultiplier(
                    PlayerStatManager.Instance.MeleeDamage.GetMultiplier() + _damageMulti);
            }

            this.isActive = true;
        }
        
        public override void Clear()
        {
            if (Inventory.Instance.EquippedArmour == null)
            {
                PlayerStatManager.Instance.MeleeDamage.SetMultiplier(
                    PlayerStatManager.Instance.MeleeDamage.GetMultiplier() - _damageMulti);
            }

            this.isActive = false;
        }

        public override void turnOff()
        {
            PlayerStatManager.Instance.MeleeDamage.SetMultiplier(
                PlayerStatManager.Instance.MeleeDamage.GetMultiplier()-_damageMulti);
        }
    }
    
    /* ---------------
     NOTABLES
     -----------------*/
    // Slayer
    public class SlayerEffect : SkillTreeEffect
    {
        private float _bleedMulti = 0.5f;
        private float _bleedChance = 0.5f;
        public SlayerEffect()
        {
            this.name = "Slayer";
            this.description = "Your attacks have a 50% chance to apply bleed, dealing 50% of your physical damage over 5 seconds";
            this.duration = -1f;
            this.effectType = EffectType.Buff;
        }

        public override void Apply()
        {
            PlayerStatManager.Instance.Bleed.SetChance(_bleedChance);
            PlayerStatManager.Instance.Bleed.SetMultiplier(
                PlayerStatManager.Instance.Bleed.GetMultiplier() + _bleedMulti);
            this.isActive = true;
        }

        public override void Clear()
        {
            PlayerStatManager.Instance.Bleed.SetChance(0f);
            PlayerStatManager.Instance.Bleed.SetMultiplier(
                PlayerStatManager.Instance.Bleed.GetMultiplier() - _bleedMulti);
            this.isActive = false;
        }

        public override void turnOff()
        {
            if (this.isActive)
            {
                PlayerStatManager.Instance.Bleed.SetChance(0f);
                PlayerStatManager.Instance.Bleed.SetMultiplier(
                    PlayerStatManager.Instance.Bleed.GetMultiplier() - _bleedMulti);
            }
        }
    }
    
    // Juggernaut
    public class JuggernautEffect : SkillTreeEffect
    {
        private float _damageReductionPerEnemy = 0.05f;
        private float _maxdamageReduction = 0.35f;
        private float _currentdamageReduction = 0f;
        private float _detectionRadius = 10f;

        public JuggernautEffect()
        {
            this.name = "Juggernaut";
            this.description = "Gain up to 5% damage reduction for each enemy nearby up to 35%.";
            this.duration = -1f;
            this.effectType = EffectType.Buff;
            this.isActive = false;
        }

        public override void Apply()
        {
            PlayerSkillTreeManager.Instance.JuggernautRepeatingInvoke();
            this.isActive = true;
        }

        public override void Clear()
        {
            isActive = false;
        }


        public void UpdateDamageReduction(MonoBehaviour manager)
        {
            _currentdamageReduction = 0f;
            Collider[] hitColliders = Physics.OverlapSphere(manager.transform.position, _detectionRadius);

            int enemycount = 0;
            foreach (Collider hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("Enemy") || hitCollider.CompareTag("Boss"))
                {
                    Debug.Log($"Juggernaut : {enemycount} Enemies around you!");
                    enemycount++;
                }
            }

            _currentdamageReduction = Mathf.Min(enemycount * _damageReductionPerEnemy, _maxdamageReduction);
            PlayerStatManager.Instance.DamageReduction.SetMultiplier(_currentdamageReduction);
        }
    }
}