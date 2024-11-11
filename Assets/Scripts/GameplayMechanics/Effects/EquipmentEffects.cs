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
        private float FlatMeleeDamage;
        private string _text;

        internal FlatMeleeDamageEffect(float flat)
        {
            _text = $"{FlatMeleeDamage} Added Physical Damage";
            FlatMeleeDamage = flat;
        }

        public override void Apply()
        {
            PlayerStatManager.Instance.MeleeDamage.SetAdded(
                PlayerStatManager.Instance.MeleeDamage.GetAdded() + FlatMeleeDamage);
        }

        public override void Clear()
        {
            PlayerStatManager.Instance.MeleeDamage.SetAdded(
                PlayerStatManager.Instance.MeleeDamage.GetAdded() - FlatMeleeDamage);
        }
        
        public string GetDisplayDescription() => _text;
    }

    public class MultiplierMeleeDamageEffect : EquipmentEffect
    {
        private float MultiplierMeleeDamage;
        private string _text;

        internal MultiplierMeleeDamageEffect(float multi)
        {
            _text = $"{MultiplierMeleeDamage*100}% Increased Physical Damage";
            MultiplierMeleeDamage = multi;
        }

        public override void Apply()
        {
            PlayerStatManager.Instance.MeleeDamage.SetMultiplier(
                PlayerStatManager.Instance.MeleeDamage.GetMultiplier() + MultiplierMeleeDamage);
        }

        public override void Clear()
        {
            PlayerStatManager.Instance.MeleeDamage.SetMultiplier(
                PlayerStatManager.Instance.MeleeDamage.GetMultiplier() - MultiplierMeleeDamage);
        }
        
        public string GetDisplayDescription() => _text;
    }

    public class FlatArmourEffect : EquipmentEffect
    {
        private float FlatArmour;
        private string _text;

        internal FlatArmourEffect(float flat)
        {
            _text = $"Armour : {FlatArmour}";
            FlatArmour = flat;
        }

        public override void Apply()
        {
            PlayerStatManager.Instance.Armour.SetFlat(
                PlayerStatManager.Instance.Armour.GetAdded() + FlatArmour);
        }

        public override void Clear()
        {
            PlayerStatManager.Instance.Armour.SetFlat(
                PlayerStatManager.Instance.Armour.GetFlat() - FlatArmour);
        }
        
        public string GetDisplayDescription() => _text;
    }
    
    public class MultiplierArmourEffect : EquipmentEffect
    {
        private float MultiplierArmour;
        private string _text;

        internal MultiplierArmourEffect(float multi)
        {
            _text = $"{MultiplierArmour*100}% Increased Armour";
            MultiplierArmour = multi;
        }

        public override void Apply()
        {
            PlayerStatManager.Instance.Armour.SetMultiplier(
                PlayerStatManager.Instance.Armour.GetMultiplier() + MultiplierArmour);
        }

        public override void Clear()
        {
            PlayerStatManager.Instance.Armour.SetMultiplier(
                PlayerStatManager.Instance.Armour.GetMultiplier() - MultiplierArmour);
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