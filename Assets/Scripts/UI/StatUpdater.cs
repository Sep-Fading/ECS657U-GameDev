using System;
using GameplayMechanics.Character;
using TMPro;
using UnityEngine;

namespace UI
{
    public class StatUpdater : MonoBehaviour
    {
        private string _statString;
        private string _prefix;
        private void Start()
        {
            _prefix = "Stats: \n";
            GetComponent<TextMeshProUGUI>().SetText(_statString);
        }

        public void UpdateStatMenu()
        {
            _statString = _prefix + PlayerStatManager.Instance.GetPlayerStats();
            GetComponent<TextMeshProUGUI>().SetText(_statString);
        }
    }
}