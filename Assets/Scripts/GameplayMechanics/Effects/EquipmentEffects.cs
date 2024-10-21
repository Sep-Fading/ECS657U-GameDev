using GameplayMechanics.Character;
using UnityEngine;

namespace GameplayMechanics.Effects
{
    public class EquipmentEffects : IEffect
    {
        public EffectType effectType { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public float duration { get; set; }
        private EquipmentType _equipmentType;
        public bool equipped = false;
        
        /* --- Some base values to work with --- */
        private float armourAddedValue = 100f;
        private float evasionAddedValue = 100f;
        private float healthAddedValue = 10f;
        private float damageAddedValue = 10f;
        private float blockAddedValue = 0.1f;

        public EquipmentEffects(EquipmentType equipmentType)
        {
            _equipmentType = equipmentType;
            if (_equipmentType == EquipmentType.ARMOR)
            {
                this.name = "Armour";
                this.description = $"+{armourAddedValue} Armour\n +{evasionAddedValue} evasion";
            }

            if (_equipmentType == EquipmentType.OFFHAND)
            {
                this.name = "Shield";
                this.description = $"+{healthAddedValue} Health";
            }

            if (_equipmentType == EquipmentType.MAINHAND)
            {
                this.name = "Sword";
                this.description = $"+{damageAddedValue} Damage";
            }
        }
        
        
        public void Apply()
        {
            if (_equipmentType == EquipmentType.ARMOR)
            {
                PlayerStatManager.Instance.Armour.SetAdded(
                    PlayerStatManager.Instance.Armour.GetAdded() + armourAddedValue);
                PlayerStatManager.Instance.Evasion.SetAdded(
                    PlayerStatManager.Instance.Evasion.GetAdded() + evasionAddedValue);
            }

            if (_equipmentType == EquipmentType.MAINHAND)
            {
                PlayerStatManager.Instance.MeleeDamage.SetAdded(
                    PlayerStatManager.Instance.MeleeDamage.GetAdded() + damageAddedValue);
            }

            if (_equipmentType == EquipmentType.OFFHAND)
            {
                PlayerStatManager.Instance.Life.SetFlat(
                    PlayerStatManager.Instance.Life.GetAdded() + healthAddedValue);
            }

            equipped = true;
        }

        public void Clear()
        {
            if (_equipmentType == EquipmentType.ARMOR)
            {
                PlayerStatManager.Instance.Armour.SetAdded(
                    PlayerStatManager.Instance.Armour.GetAdded() - armourAddedValue);
                PlayerStatManager.Instance.Evasion.SetAdded(
                    PlayerStatManager.Instance.Evasion.GetAdded() - evasionAddedValue);
            }

            if (_equipmentType == EquipmentType.MAINHAND)
            {
                PlayerStatManager.Instance.MeleeDamage.SetAdded(
                    PlayerStatManager.Instance.MeleeDamage.GetAdded() - damageAddedValue);
            }

            if (_equipmentType == EquipmentType.OFFHAND)
            {
                PlayerStatManager.Instance.Life.SetFlat(
                    PlayerStatManager.Instance.Life.GetAdded() - healthAddedValue);
            }

            equipped = false;
        }
    }

    public enum EquipmentType
    {
        NONE,
        ARMOR,
        MAINHAND,
        OFFHAND
    }
}