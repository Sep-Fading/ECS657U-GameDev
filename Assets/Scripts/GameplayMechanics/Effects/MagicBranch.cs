using GameplayMechanics.Character;

namespace GameplayMechanics.Effects
{
    /// <summary>
    /// Handles the magic functionalities of the Effects
    /// </summary>
    public class MagicStartEffect : SkillTreeEffect
    {
        private readonly float _moveSpeedMulti = 0.1f;
        private readonly float _magicDamageMulti = 0.2f;
        private readonly float _healPowerMulti = 0.2f;

        public MagicStartEffect()
        {
            this.name = "Path of the Magi";
            this.description = "10% Increased Walk Speed \n" +
                               "20% Increased Magic Damage \n" +
                               "20% Increased Healing Power";
            this.effectType = EffectType.Buff;
            this.duration = -1f;
        }

        public override void Apply()
        {
            PlayerStatManager.Instance.MoveSpeed.SetMultiplier(
                PlayerStatManager.Instance.MoveSpeed.GetMultiplier() + _moveSpeedMulti);
            PlayerStatManager.Instance.MagicDamage.SetMultiplier(
                PlayerStatManager.Instance.MagicDamage.GetMultiplier() + _magicDamageMulti);
            PlayerStatManager.Instance.HealPower.SetMultiplier(
                PlayerStatManager.Instance.HealPower.GetMultiplier() + _healPowerMulti);

            this.isActive = true;
        }

        public override void Clear()
        {
            PlayerStatManager.Instance.MoveSpeed.SetMultiplier(
                PlayerStatManager.Instance.MoveSpeed.GetMultiplier() - _moveSpeedMulti);
            PlayerStatManager.Instance.MagicDamage.SetMultiplier(
                PlayerStatManager.Instance.MagicDamage.GetMultiplier() - _magicDamageMulti);
            PlayerStatManager.Instance.HealPower.SetMultiplier(
                PlayerStatManager.Instance.HealPower.GetMultiplier() - _healPowerMulti);
            
            this.isActive = false;
        }
    }

    public class IncreasedMagicDamageEffect : SkillTreeEffect
    {
        private readonly float _magicDamageMulti = 0.2f;

        public IncreasedMagicDamageEffect()
        {
            this.name = "Magic Damage";
            this.description = "20% Increased Magic Damage";
            this.effectType = EffectType.Buff;
            this.duration = -1f;
        }

        public override void Apply()
        {
            PlayerStatManager.Instance.MagicDamage.SetMultiplier(
                PlayerStatManager.Instance.MagicDamage.GetMultiplier() + _magicDamageMulti);
            this.isActive = true;
        }

        public override void Clear()
        {
            PlayerStatManager.Instance.MagicDamage.SetMultiplier(
                PlayerStatManager.Instance.MagicDamage.GetMultiplier() - _magicDamageMulti);
            this.isActive = false;
        }
    }
}