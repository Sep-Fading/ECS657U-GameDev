using Unity.VisualScripting;
using UnityEngine;

namespace GameplayMechanics.Character
{
    public class XpManager
    {
        public static XpManager Instance { get; private set; }
        private float CurrentXp { get; set; }
        private float LevelUpThreshold { get; set; }
        private int Level { get; set; }

        private const float BaseXpThreshold = 50f;
        private const float GrowthFactor = 1.4f;

        private XpManager()
        {
            CurrentXp = 0f;
            LevelUpThreshold = BaseXpThreshold;
            Level = 1;
        }

        public static XpManager Initialize()
        {
            if (Instance == null)
            {
                Instance = new XpManager();
            }
            return Instance;
        }

        private static void LevelUp()
        {
            Instance.Level += 1;
            Instance.LevelUpThreshold = BaseXpThreshold * Mathf.Pow(GrowthFactor, Instance.Level);
            Debug.Log($"Leveled up to level {Instance.Level}. New XP threshold: {Instance.LevelUpThreshold}");
        }

        public static void GiveXp(float amount)
        {
            Instance.CurrentXp += amount;
            if (Instance.CurrentXp >= Instance.LevelUpThreshold)
            {
                LevelUp();
            }
        }

        public static float GetCurrentXp()
        {
            return Instance.CurrentXp;
        }
    }
}