using System;
using GameplayMechanics.Character;
using TMPro;
using UnityEngine;

namespace UI
{
    /// <summary>
    /// Updates the stats of the player
    /// </summary>
    public class StatUpdater : MonoBehaviour
    {
        private string _statString;
        private string _prefix;
        [SerializeField] private TextMeshProUGUI statText;

        public void UpdateStatMenu()
        {
            _statString = PlayerStatManager.Instance.GetPlayerStats();
            statText.SetText(_statString);
        }
    }
}