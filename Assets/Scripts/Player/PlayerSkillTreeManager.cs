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

            if (_skillTreeButtonsStatus[index])
            {
                if (_skillTree.branchSwordShield.SkillNodes[index].parent == null
                    || _skillTree.branchSwordShield.SkillNodes[index].parent._effect.isActive)
                {
                    _skillTree.branchSwordShield.SkillNodes[index]._effect.Apply();
                }
                else
                {
                    Debug.Log("Can't assign");
                }
            }
            else
            {
                if (!CheckActiveChildren(index) && _skillTree.branchSwordShield.SkillNodes[index]._effect.isActive)
                {
                    _skillTree.branchSwordShield.SkillNodes[index]._effect.Clear();
                }
                else
                {
                    Debug.Log("Can't unassign, children are active");
                }
            }
            
            UpdateButtonAppearance(index);
            Debug.Log(PlayerStatManager.Instance.GetPlayerStats());
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
