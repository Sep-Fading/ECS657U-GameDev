namespace GameplayMechanics.Character
{
    public interface CharacterStats
    {
        float health { get; set; }
        float stamina { get; set; }
        float blockEffectiveness { get; set; }
        int evasion { get; set; }
        float actionSpeed { get; set; }
        float attackSpeed { get; set; }
        float runSpeed { get; set; }
        float crouchSpeed { get; set; }
        float stunMultiplier { get; set; }
        float meleeDamageMultiplier { get; set; }
        float projectileDamageMultiplier { get; set; }
        float projectileSpeedMultiplier { get; set; }
        int armour { get; set; }
        float magicFindMultiplier { get; set; }
        float detectionRadiusMultiplier { get; set; }
    }
}