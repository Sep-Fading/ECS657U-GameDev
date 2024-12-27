using GameplayMechanics.Character;
using UnityEngine;

namespace GameplayMechanics.Effects
{
    // This class extends IEffect interface
    // to create some effects for equipment such as 
    // the stats on that piece of equipment.
    
    public class EquipmentEffect : IEffect
    {
        public EffectType effectType { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public float duration { get; set; }
        
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
        
    }

    public class FlatMeleeDamageEffect : EquipmentEffect
    {
        private readonly float _flatMeleeDamage;
        private readonly string _text;

        internal FlatMeleeDamageEffect(float flat)
        {
            _text = $"{_flatMeleeDamage} Added Physical Damage";
            _flatMeleeDamage = flat;
        }

        public override void Apply()
        {
            PlayerStatManager.Instance.MeleeDamage.SetAdded(
                PlayerStatManager.Instance.MeleeDamage.GetAdded() + _flatMeleeDamage);
        }

        public override void Clear()
        {
            PlayerStatManager.Instance.MeleeDamage.SetAdded(
                PlayerStatManager.Instance.MeleeDamage.GetAdded() - _flatMeleeDamage);
        }
        
        public string GetDisplayDescription() => _text;
    }

    public class MultiplierMeleeDamageEffect : EquipmentEffect
    {
        private readonly float _multiplierMeleeDamage;
        private readonly string _text;

        internal MultiplierMeleeDamageEffect(float multi)
        {
            _text = $"{_multiplierMeleeDamage*100}% Increased Physical Damage";
            _multiplierMeleeDamage = multi;
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
        
        public string GetDisplayDescription() => _text;
    }

    public class FlatArmourEffect : EquipmentEffect
    {
        private readonly float _flatArmour;
        private readonly string _text;

        internal FlatArmourEffect(float flat)
        {
            _text = $"Armour : {_flatArmour}";
            _flatArmour = flat;
        }

        public override void Apply()
        {
            PlayerStatManager.Instance.Armour.SetFlat(
                PlayerStatManager.Instance.Armour.GetAdded() + _flatArmour);
        }

        public override void Clear()
        {
            PlayerStatManager.Instance.Armour.SetFlat(
                PlayerStatManager.Instance.Armour.GetFlat() - _flatArmour);
        }
        
        public string GetDisplayDescription() => _text;
    }
    
    public class MultiplierArmourEffect : EquipmentEffect
    {
        private readonly float _multiplierArmour;
        private readonly string _text;

        internal MultiplierArmourEffect(float multi)
        {
            _text = $"{_multiplierArmour*100}% Increased Armour";
            _multiplierArmour = multi;
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
        
        public string GetDisplayDescription() => _text;
    }
    
    public class FlatBlockEffectivenessEffect : EquipmentEffect
    {
        private readonly float _flatBlockEffectiveness;
        private readonly string _text;
        internal FlatBlockEffectivenessEffect(float flat)
        {
            _text = $"{_flatBlockEffectiveness*100}% to Block Effectiveness";
            _flatBlockEffectiveness = flat;
        }
        public override void Apply()
        {
            PlayerStatManager.Instance.BlockEffect.SetFlat(
                PlayerStatManager.Instance.BlockEffect.GetFlat() + _flatBlockEffectiveness);
        }
        public override void Clear()
        {
            PlayerStatManager.Instance.BlockEffect.SetFlat(
                PlayerStatManager.Instance.BlockEffect.GetFlat() - _flatBlockEffectiveness);
        }
        
        public string GetDisplayDescription() => _text;
    }
    
    public class FlatHealthEffect : EquipmentEffect
    {
        private readonly float _flatHealth;
        private readonly string _text;
        internal FlatHealthEffect(float flat)
        {
            _text = $"{_flatHealth} Added Health";
            _flatHealth = flat;
        }
        public override void Apply()
        {
            PlayerStatManager.Instance.Life.SetFlat(
                PlayerStatManager.Instance.Life.GetFlat() + _flatHealth);
        }
        public override void Clear()
        {
            PlayerStatManager.Instance.Life.SetFlat(
                PlayerStatManager.Instance.Life.GetFlat() - _flatHealth);
        }
        
        public string GetDisplayDescription() => _text;
    }

    public enum EquipmentType
    {
        NONE,
        ARMOR,
        MAINHAND,
        OFFHAND
    }

}