using GameplayMechanics.Character;
using InventoryScripts;
using UnityEngine;

namespace GameplayMechanics.Effects
{
    // This class extends IEffect interface
    // to create some effects for equipment such as 
    // the stats on that piece of equipment.
    
    public class EquipmentEffect : IEffect
    {
        public EffectType effectType {get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public float duration { get; set; }
        protected string _text;
        
        public virtual void Apply()
        {
            throw new System.NotImplementedException();
        }

        public virtual void Clear()
        {
            throw new System.NotImplementedException();
        }

        private EquipmentType _equipmentType;
        public bool Equipped = false;
        public string GetDisplayDescription() => _text;
    }

    public class FlatMeleeDamageEffect : EquipmentEffect
    {
        private readonly float _flatMeleeDamage;
        internal FlatMeleeDamageEffect(float flat) 
        {
            _flatMeleeDamage = flat;
            _text = $"{_flatMeleeDamage:F1} Added Physical Damage";
        }

        public override void Apply()
        {
            PlayerStatManager.Instance.MeleeDamage.SetAdded(
                PlayerStatManager.Instance.MeleeDamage.GetAdded() + _flatMeleeDamage);
            Debug.LogWarning($"{PlayerStatManager.Instance.MeleeDamage.GetAppliedTotal()} to Melee Damage");
        }

        public override void Clear()
        {
            PlayerStatManager.Instance.MeleeDamage.SetAdded(
                PlayerStatManager.Instance.MeleeDamage.GetAdded() - _flatMeleeDamage);
        }
    }

    public class MultiplierMeleeDamageEffect : EquipmentEffect
    {
        private readonly float _multiplierMeleeDamage;

        internal MultiplierMeleeDamageEffect(float multi)
        {
            _multiplierMeleeDamage = multi;
            _text = $"{_multiplierMeleeDamage*100:F1}% Increased Physical Damage";
        }

        public override void Apply()
        {
            PlayerStatManager.Instance.MeleeDamage.SetMultiplier(
                PlayerStatManager.Instance.MeleeDamage.GetMultiplier() + _multiplierMeleeDamage);
        }

        public override void Clear()
        {
            PlayerStatManager.Instance.MeleeDamage.SetMultiplier(
                PlayerStatManager.Instance.MeleeDamage.GetMultiplier() - _multiplierMeleeDamage);
        }
    }

    public class FlatArmourEffect : EquipmentEffect
    {
        private readonly float _flatArmour;

        internal FlatArmourEffect(float flat) 
        {
            _flatArmour = flat;
            _text = $"{_flatArmour:N0} Added Armour";
        }

        public override void Apply()
        {
            PlayerStatManager.Instance.Armour.SetAdded(
                PlayerStatManager.Instance.Armour.GetAdded() + _flatArmour);
        }

        public override void Clear()
        {
            PlayerStatManager.Instance.Armour.SetAdded(
                PlayerStatManager.Instance.Armour.GetAdded() - _flatArmour);
        }
    }
    
    public class MultiplierArmourEffect : EquipmentEffect
    {
        private readonly float _multiplierArmour;

        internal MultiplierArmourEffect(float multi)
        {
            _multiplierArmour = multi;
            _text = $"{_multiplierArmour*100:F1}% Increased Armour";
        }

        public override void Apply()
        {
            PlayerStatManager.Instance.Armour.SetMultiplier(
                PlayerStatManager.Instance.Armour.GetMultiplier() + _multiplierArmour);
        }

        public override void Clear()
        {
            PlayerStatManager.Instance.Armour.SetMultiplier(
                PlayerStatManager.Instance.Armour.GetMultiplier() - _multiplierArmour);
        }

    }
    
    public class FlatBlockEffectivenessEffect : EquipmentEffect
    {
        private readonly float _flatBlockEffectiveness;
        internal FlatBlockEffectivenessEffect(float flat)
        {
            _flatBlockEffectiveness = flat;
            _text = $"{_flatBlockEffectiveness*100:F1}% to Block Effectiveness";
        }
        public override void Apply()
        {
            PlayerStatManager.Instance.BlockEffect.SetAdded(
                PlayerStatManager.Instance.BlockEffect.GetAdded() + _flatBlockEffectiveness);
        }
        public override void Clear()
        {
            PlayerStatManager.Instance.BlockEffect.SetAdded(
                PlayerStatManager.Instance.BlockEffect.GetAdded() - _flatBlockEffectiveness);
        }

    }
    
    public class FlatHealthEffect : EquipmentEffect
    {
        private readonly float _flatHealth;
        internal FlatHealthEffect(float flat) 
        {
            _flatHealth = flat;
            _text = $"{_flatHealth:N0} Added Health";
        }
        public override void Apply()
        {
            PlayerStatManager.Instance.Life.SetAdded(
                PlayerStatManager.Instance.Life.GetAdded() + _flatHealth);
        }
        public override void Clear()
        {
            PlayerStatManager.Instance.Life.SetAdded(
                PlayerStatManager.Instance.Life.GetAdded() - _flatHealth);
        }
    }

    public enum EquipmentType
    {
        NONE,
        ARMOR,
        MAINHAND,
        OFFHAND,
        GREATSWORD,
        AXE,
    }

}