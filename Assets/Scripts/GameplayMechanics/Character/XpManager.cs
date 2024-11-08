using Unity.VisualScripting;
using UnityEngine;

namespace GameplayMechanics.Character
{
    // A single ton class that handles the Level Up 
    // system using a levelling formula that scales 
    // over levels.
    public class XpManager
    {
        public static XpManager Instance { get; private set; }
        private float CurrentXp { get; set; }
        private float LevelUpThreshold { get; set; }
        private int Level { get; set; }

        private int CurrentSkillPoints { get; set; }

        private float BaseXpThreshold = 50f;
        private const float GrowthFactor = 1.4f;

        private XpManager()
        {
            CurrentXp = 0f;
            CurrentSkillPoints = 0;
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

        public static void ResetInstance()
        {
            Instance = null;
        }
        private static void LevelUp()
        {
            Instance.Level += 1;
            Instance.CurrentSkillPoints += 1;
            Instance.BaseXpThreshold = GetCurrentXp();
            Instance.LevelUpThreshold = Instance.BaseXpThreshold * Mathf.Pow(GrowthFactor, Instance.Level);
            // Heal to full after Level Up
            PlayerStatManager.Instance.Life.SetCurrent(
                PlayerStatManager.Instance.Life.GetAppliedTotal());
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

        public static void SetCurrentSkillPoints(int points)
        {
            Instance.CurrentSkillPoints = points;
        }

        public static float GetLevelUpThreshold() => Instance.LevelUpThreshold;
        public static int GetLevel() => Instance.Level;
        public static int GetCurrentSkillPoints() => Instance.CurrentSkillPoints;
    }
}