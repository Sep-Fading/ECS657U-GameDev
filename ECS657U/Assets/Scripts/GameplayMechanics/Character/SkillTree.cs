using System;
using System.Linq;
using GameplayMechanics.Effects;
using Unity.VisualScripting;
using UnityEngine;

namespace GameplayMechanics.Character
{
    public class SkillTree
    {
        private SkillTreeBranch _branchSwordShield;

        SkillTree()
        {
            /* --- Initialise effects (Sword & Shield) --- */

            /* --- Start Node --- */
            SkillTreeEffect swordShieldStartNodeEffect = new SwordShieldStartEffect();

            /* --- Tier 1 Nodes --- */
            // Increased Health
            SkillTreeEffect increasedHealthEffect = new IncreasedHPEffect();
            SkillTreeEffect increasedHealthEffect2 = new IncreasedHPEffect();

            // Increased Melee Damage
            SkillTreeEffect increasedMeleeDamageEffect = new IncreasedMeleeDamageEffect();
            SkillTreeEffect increasedMeleeDamageEffect2 = new IncreasedMeleeDamageEffect();

            // Added Block Effectiveness
            SkillTreeEffect addedBlockEffect = new AddedBlockEffect();
            SkillTreeEffect addedBlockEffect2 = new AddedBlockEffect();

            /* --- Notables --- */
            // TODO

            /* --- Masteries ---*/
            // Armour Mastery
            SkillTreeEffect armourMasteryEffect = new ArmourMasteryEffect();

            // Versatility Mastery
            SkillTreeEffect versatilityMasteryEffect = new VersatilityMasteryEffect();

            /* --- Initialise the skill nodes --- */

            /* --- Start Node --- */
            SkillNode swordShieldStartNode = new SkillNode("SS_Start", swordShieldStartNodeEffect.name,
                swordShieldStartNodeEffect.description, swordShieldStartNodeEffect);

            /* --- Tier 1 Nodes --- */
            SkillNode increasedHealthNode1 = new SkillNode("health_1", increasedHealthEffect.name,
                increasedHealthEffect.description, increasedHealthEffect);
            SkillNode increasedHealthNode2 = new SkillNode("health_2", increasedHealthEffect2.name,
                increasedHealthEffect2.description, increasedHealthEffect2);

            SkillNode increasedMeleeDamageNode1 = new SkillNode("meleeDamage_1", increasedMeleeDamageEffect.name,
                increasedMeleeDamageEffect.description, increasedMeleeDamageEffect);
            SkillNode increasedMeleeDamageNode2 = new SkillNode("meleeDamage_2", increasedMeleeDamageEffect2.name,
                increasedMeleeDamageEffect2.description, increasedMeleeDamageEffect2);

            SkillNode addedBlockEffectNode1 = new SkillNode("blockEffect_1", addedBlockEffect.name,
                addedBlockEffect.description, addedBlockEffect);
            SkillNode addedBlockEffectNode2 = new SkillNode("blockEffect_2", addedBlockEffect2.name,
                addedBlockEffect2.description, addedBlockEffect2);

            /* --- Masteries --- */
            SkillNode armourMasteryNode = new SkillNode("amourMastery", armourMasteryEffect.name,
                armourMasteryEffect.description, armourMasteryEffect);
            SkillNode versatilityMasteryNode = new SkillNode("versMastery", versatilityMasteryEffect.name,
                versatilityMasteryEffect.description, versatilityMasteryEffect);

            /* --- Load the nodes into an array --- */
            SkillNode[] _skillNodes = new[]
            {
                swordShieldStartNode,
                increasedHealthNode1, increasedHealthNode2,
                increasedMeleeDamageNode1, increasedMeleeDamageNode2,
                addedBlockEffectNode1, addedBlockEffectNode2,
                armourMasteryNode, versatilityMasteryNode
            };

        /* --- Construct a SkillTreeBranch ---*/
        _branchSwordShield = new SkillTreeBranch("Sword and Shield Branch", _skillNodes);
    }
}


/*------------------------------------------------------------------------------------------
 Represents a branch in the skill tree.
 A branch is a 1/3 sector of the skill tree, categorised by weapon class.
 Abstract class to be used by the tree above.
 ------------------------------------------------------------------------------------------*/
    public class SkillTreeBranch
    {
        private string _name;
        private SkillNode[] _skillNodes;
        
        /*------------------
         Class Constructor
         ------------------*/
        public SkillTreeBranch(string name, SkillNode[] skillNodes)
        {
            this._name = name;
            this._skillNodes = skillNodes;
            
            /* --- Create the relationships in the tree --- */
            this._skillNodes[0].parent = null;
            this._skillNodes[0].children = new[]
            {
                _skillNodes[1],
                _skillNodes[3],
                _skillNodes[5]
            };
            foreach (SkillNode skillNode in this._skillNodes[0].children)
            {
                skillNode.parent = this._skillNodes[0];
            }
            
            this._skillNodes[1].children = new[] { this._skillNodes[2] };
            foreach (SkillNode skillNode in this._skillNodes[1].children)
            {
                skillNode.parent = this._skillNodes[1];
            }
            
            this._skillNodes[2].children = new[] { this._skillNodes[7] };
            foreach (SkillNode skillNode in this._skillNodes[2].children)
            {
                skillNode.parent = this._skillNodes[2];
            }
            
            this._skillNodes[3].children = new[] { this._skillNodes[4] };
            foreach (SkillNode skillNode in this._skillNodes[3].children)
            {
                skillNode.parent = this._skillNodes[3];
            }
        }
        
        /*------------------
         Returns a string that visualises a skill branch.
         Used for debugging purposes.
         ------------------*/
        public string GetSkillBranch()
        {
            return "";
        }
        
        /*------------------
         Returns a string that visualises an array of skill nodes.
         Used by GetSkillBranch() to visualise skill branches,
         for debugging purposes.
         -----------------*/
        private string SkillBranchStringify(SkillNode[] nodes)
        {
            return "";
        }
    }
    
    /*------------------------------------------------------------------------------------------
     Represents a node in the skill tree,
     used to build out a data structure that holds the passive skill tree in it.
     ------------------------------------------------------------------------------------------*/
    public class SkillNode
    {
        public SkillNode parent;
        public SkillNode[] children;
        public string id;
        public string name;
        private string _description;
        private SkillTreeEffect _effect;
        
        /*------------------
         Class Constructor
         ------------------*/
        public SkillNode(string id, string name, string description, SkillTreeEffect effect)
        {
            this.id = id;
            this.name = name;
            this._description = description;
            this._effect = effect;
            this.parent = null;
            this.children = null;
        }
        
        /*------------------
        Getters and Setters for Parent and Children
        GetParent returns SkillNode that is the parent of this instance.
        GetChildren returns an array containing all children of the node.
        GetChild returns a specific child by id, name or index. **Name only returns the first child.**
         ------------------*/
        public SkillNode GetParent { get => parent; set => parent = value; }
        public SkillNode[] GetChildren { get => children; set => children = value; }
        
        public SkillNode GetChild(string name, int? index = null, string id = null)
        {
            if (index != null)
            {
                return children[index.Value];
            }

            if (id != null)
            {
                foreach (SkillNode node in children)
                {
                    if (node.id == id)
                    {
                        return node;
                    }
                }
            }
            
            foreach (SkillNode node in children)
            {
                if (node.name == name)
                {
                    return node;
                }
            }
            
            Debug.Log("Node not found!");
            return null;
        }
    }
    
    
    /*------------------------------------------------------------------------------------------
     Extends the IEffect interface to create an effects class.
     The class is still implemented in an abstract manner, to
     be used to create different effects.
     ------------------------------------------------------------------------------------------*/
    public class SkillTreeEffect : IEffect
    {
        public EffectType effectType { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public float duration { get; set; }
        public GameObject target { get; set; }
        
        public void Apply()
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }
    }
}