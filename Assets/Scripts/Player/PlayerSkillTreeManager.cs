using GameplayMechanics.Character;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    // This script initialises and manages our skill tree
    // and uses XpManager singleton to manage skill points
    // with respect to player's usage of the skill tree.
    public class PlayerSkillTreeManager : MonoBehaviour
    {
        private SkillTree _skillTree;
        [SerializeField] public GameObject _skillTreeUI;
        [SerializeField] private Button[] skillTreeButtons;
        private bool[] _skillTreeButtonsStatus;
        [SerializeField] private int SkillCheats;
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
            
            // Cheats
            XpManager.SetCurrentSkillPoints(SkillCheats);
        }

        private void ToggleButtonState(int index)
        {
            _skillTreeButtonsStatus[index] = !_skillTreeButtonsStatus[index];
            SkillNode currentNode = _skillTree.branchSwordShield.SkillNodes[index];
            if (_skillTreeButtonsStatus[index])
            {
                bool parentIsActive = false;
                
                
                // Check if parent exists and at least one is active
                if (currentNode.parent.Count > 0)
                {
                    foreach (SkillNode p in currentNode.parent)
                    {
                        if (p._effect.isActive)
                        {
                            parentIsActive = true;
                            break;
                        }
                    }
                }
                if (parentIsActive ||
                    currentNode.parent == null || currentNode.parent.Count == 0)
                { 
                    if (XpManager.GetCurrentSkillPoints() > 0) 
                    {
                        currentNode._effect.Apply(); 
                        XpManager.SetCurrentSkillPoints(
                            XpManager.GetCurrentSkillPoints() - 1);
                    }
                }
            }
            else
            {
                if (!CheckActiveChildren(index) && _skillTree.branchSwordShield.SkillNodes[index]._effect.isActive)
                {
                    _skillTree.branchSwordShield.SkillNodes[index]._effect.Clear();
                    XpManager.SetCurrentSkillPoints(
                        XpManager.GetCurrentSkillPoints() + 1);
                }
            }
            
            UpdateButtonAppearance(index);
        }

        private bool CheckActiveChildren(int index)
        {
            if (_skillTree.branchSwordShield.SkillNodes[index].children == null)
            {
                return false;
            }
            foreach (SkillNode node in _skillTree.branchSwordShield.SkillNodes[index].children)
            {
                if (node._effect.isActive)
                {
                    return true;
                }
            }

            return false;
        }

        private void UpdateButtonAppearance(int index)
        {
            if (_skillTree.branchSwordShield.SkillNodes[index]._effect.isActive)
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
