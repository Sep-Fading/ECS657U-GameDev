using System;
using System.Collections.Generic;
using System.Linq;
using GameplayMechanics.Character;
using GameplayMechanics.Effects;
using InventoryScripts;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Player
{
    // This script initialises and manages our skill tree
    // and uses XpManager singleton to manage skill points
    // with respect to player's usage of the skill tree.
    public class PlayerSkillTreeManager : MonoBehaviour
    {
        public static PlayerSkillTreeManager Instance;
        public SkillTree ManagerSkillTree;
        [FormerlySerializedAs("_skillTreeUI")] [SerializeField] public GameObject skillTreeUI;
        [SerializeField] private Button[] skillTreeButtons;
        private bool[] _skillTreeButtonsStatus;
        [SerializeField] private int SkillCheats;
        [SerializeField] private Transform connectionsParentTransform;
        [SerializeField] private Transform[] connectionsChildTransforms;
        public List<NodeConnection> connections;
        void Start()
        {
            Instance = this;
            ManagerSkillTree = new SkillTree();
            _skillTreeButtonsStatus = new bool[skillTreeButtons.Length];
            skillTreeUI.SetActive(false);

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
            SkillNode currentNode = ManagerSkillTree.branchSwordShield.SkillNodes[index];
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
                if (!CheckActiveChildren(index) && ManagerSkillTree.branchSwordShield.SkillNodes[index]._effect.isActive)
                {
                    ManagerSkillTree.branchSwordShield.SkillNodes[index]._effect.Clear();
                    XpManager.SetCurrentSkillPoints(
                        XpManager.GetCurrentSkillPoints() + 1);
                }
            }
            
            UpdateButtonAppearance(index);
        }

        private bool CheckActiveChildren(int index)
        {
            if (ManagerSkillTree.branchSwordShield.SkillNodes[index].children == null)
            {
                return false;
            }
            foreach (SkillNode node in ManagerSkillTree.branchSwordShield.SkillNodes[index].children)
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
            bool isActive = ManagerSkillTree.branchSwordShield.SkillNodes[index]._effect.isActive;
            Color targetColor = isActive ? new Color(255f, 204f, 0f) : Color.white;

            // Update button appearance
            skillTreeButtons[index].transform.Find("Border").GetComponent<Image>().color = targetColor;

            // Check and update connections
            foreach (NodeConnection connection in connections)
            {
                bool bothNodesActive = connection.startNode == skillTreeButtons[index] || 
                                       connection.endNode == skillTreeButtons[index];

                if (bothNodesActive)
                {
                    // If both start and end nodes are active, color the line yellow
                    bool startNodeActive = ManagerSkillTree.branchSwordShield
                        .SkillNodes[Array.IndexOf(skillTreeButtons, connection.startNode)]
                        ._effect.isActive;
                    bool endNodeActive = ManagerSkillTree.branchSwordShield
                        .SkillNodes[Array.IndexOf(skillTreeButtons, connection.endNode)]
                        ._effect.isActive;

                    connection.lineImage.color = (startNodeActive && endNodeActive) 
                        ? new Color(255f, 204f, 0f) 
                        : Color.white;
                }
            } 
        }

        private void SetupConnections()
        {
            connectionsChildTransforms = connectionsParentTransform.Cast<Transform>().ToArray();
        }
        
        public void JuggernautRepeatingInvoke()
        {
            SkillTreeEffect effect = ManagerSkillTree.branchSwordShield.GetNodeByName("Juggernaut")._effect;
            if (effect is JuggernautEffect juggernautEffect)
            {
                // Start repeating update for the Juggernaut effect
                InvokeRepeating(nameof(UpdateJuggernautEffect), 0f, 1f);
            }
        }

        private void UpdateJuggernautEffect()
        {
            SkillTreeEffect effect = ManagerSkillTree.branchSwordShield.GetNodeByName("Juggernaut")._effect;

            if (effect is JuggernautEffect juggernautEffect)
            {
                if (Inventory.Instance.EquippedArmour == null)
                {
                    juggernautEffect.UpdateDamageReduction(this); // Ensure UpdateDamageReduction is parameterless
                }
                else
                {
                    CancelInvoke(nameof(UpdateJuggernautEffect));
                }
            }
        }
        
        public void PrintDebugConnections()
        {
            if (connectionsChildTransforms.Length == 0)
            {
                SetupConnections();
            }
            foreach (Transform child in connectionsChildTransforms)
            {
                Debug.Log(child.name);
            }
        }
    }
}

[Serializable]
public class NodeConnection
{
    public Button startNode;
    public Button endNode;
    public Image lineImage;
}
