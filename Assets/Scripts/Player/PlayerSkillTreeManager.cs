using System;
using System.Collections.Generic;
using System.Linq;
using GameplayMechanics.Character;
using GameplayMechanics.Effects;
using InventoryScripts;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
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

        [SerializeField] private TextMeshProUGUI tooltipTitle;
        [SerializeField] private TextMeshProUGUI tooltipDesc;
        void Start()
        {
            Instance = this;
            ManagerSkillTree = new SkillTree();
            _skillTreeButtonsStatus = new bool[skillTreeButtons.Length];
            
            tooltipTitle = GameObject.Find("-- ToolTip").transform.Find("Title").GetComponent<TextMeshProUGUI>();
            tooltipDesc = GameObject.Find("-- ToolTip").transform.Find("Description").GetComponent<TextMeshProUGUI>();

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

                AddHoverEvents(skillTreeButtons[i], index);
            }
            
            // Cheats
            XpManager.SetCurrentSkillPoints(SkillCheats);
        }

        private void AddHoverEvents(Button button, int index)
        {
            EventTrigger trigger = button.gameObject.AddComponent<EventTrigger>();
            
            // Entry
            EventTrigger.Entry entryEnter = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerEnter
            };
            
            entryEnter.callback.AddListener((data) => OnButtonHoverStart(index));
            trigger.triggers.Add(entryEnter);
            
            // Exit
            EventTrigger.Entry entryExit = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerExit
            };
            entryExit.callback.AddListener((data) => OnButtonHoverEnd());
            trigger.triggers.Add(entryExit);
        }

        private void OnButtonHoverEnd()
        {
            tooltipTitle.text = "";
            tooltipDesc.text = "";
            tooltipTitle.gameObject.SetActive(false);
            tooltipDesc.gameObject.SetActive(false);
        }

        private void OnButtonHoverStart(int index)
        {
            SkillNode node = ManagerSkillTree.branchSwordShield.SkillNodes[index];
            tooltipTitle.text = node.name;
            tooltipDesc.text = node._effect.description;
            tooltipDesc.gameObject.SetActive(true);
            tooltipTitle.gameObject.SetActive(true);
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
    SkillNode currentNode = ManagerSkillTree.branchSwordShield.SkillNodes[index];
    
    // If the current node has no children, return false
    if (currentNode.children == null || currentNode.children.Count == 0)
    {
        return false;
    }

    // Iterate over each child of the current node
    foreach (SkillNode childNode in currentNode.children)
    {
        // Check if the child node is active
        if (childNode._effect.isActive)
        {
            bool otherParentActive = false;

            // Check if any other parent of the child node is active
            foreach (SkillNode parentNode in childNode.parent)
            {
                if (parentNode != currentNode && parentNode._effect.isActive)
                {
                    otherParentActive = true;
                    break; // Stop checking other parents if one is active
                }
            }

            // If no other parents are active, this child depends solely on the current node
            if (!otherParentActive)
            {
                return true; // Return true if the child has no other active parent
            }
        }
    }

    return false; // Return false if all active children have other active parents
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
                if (Inventory.Instance.EquippedArmour != null)
                {
                    juggernautEffect.UpdateDamageReduction(this);
                }
                else
                {
                    Debug.Log("No Juggernaut");
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
