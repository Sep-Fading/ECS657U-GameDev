using System;
using GameplayMechanics.Character;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GameOverScreen : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI PlayerPerformance;
        [SerializeField] private Button RestartButton;
        
        void Start()
        {
            PlayerPerformance.text = String.Empty;    
            RestartButton.onClick.AddListener(() => PlayAgain());
        }

        
        public void DisplayGameOverStats()
        {
            PlayerPerformance.text = $"Here are the stats of the fallen warrior: \nLevel: {XpManager.GetLevel()},\nXP: {XpManager.GetCurrentXp()} / {XpManager.GetLevelUpThreshold()}";
        }

        public void PlayAgain()
        {
            //add restart functionality here
        }

    }
}