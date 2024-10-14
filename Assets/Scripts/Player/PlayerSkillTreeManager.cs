using GameplayMechanics.Character;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class PlayerSkillTreeManager : MonoBehaviour
    {
        private SkillTree _skillTree;
        [SerializeField] public GameObject _skillTreeUI;
        [SerializeField] private Button[] skillTreeButtons;
        private bool[] _skillTreeButtonsStatus;
        void Start()
        {
            _skillTree = new SkillTree();
            _skillTreeButtonsStatus = new bool[skillTreeButtons.Length];
            _skillTreeUI.SetActive(false);
        
            // Default state of nodes (DISABLED)
            for (int i = 0; i < _skillTreeButtonsStatus.Length; i++)
            {
                _skillTreeButtonsStatus[i] = false;
            }
        
            // Listeners for each button:
            for (int i = 0; i < skillTreeButtons.Length; i++)
            {
                int index = i;
                skillTreeButtons[i].onClick.AddListener(() => ToggleButtonState(index));
            }
        }

        private void ToggleButtonState(int index)
        {
            _skillTreeButtonsStatus[index] = !_skillTreeButtonsStatus[index];
            UpdateButtonAppearance(index);

            if (_skillTreeButtonsStatus[index])
            {
                _skillTree.branchSwordShield.SkillNodes[index]._effect.Apply();
            }
            else
            {
                _skillTree.branchSwordShield.SkillNodes[index]._effect.Clear();
            }
            
            Debug.Log(PlayerStatManager.Instance.GetPlayerStats());
        }

        private void UpdateButtonAppearance(int index)
        {
            if (_skillTreeButtonsStatus[index])
            {
                skillTreeButtons[index].GetComponent<Image>().color = Color.green;
            }
            else
            {
                skillTreeButtons[index].GetComponent<Image>().color = Color.white;
            }
        }
    }
}
