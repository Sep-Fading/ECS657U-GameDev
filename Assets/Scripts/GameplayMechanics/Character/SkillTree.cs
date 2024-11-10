using System;
using System.Collections.Generic;
using System.Linq;
using GameplayMechanics.Effects;
using Unity.VisualScripting;
using UnityEngine;

namespace GameplayMechanics.Character
{
    public class SkillTree
    {
        public SkillTreeBranch branchSwordShield;

        public SkillTree()
        {
            /* --- Initialise effects (Sword & Shield) --- */

            /* --- Start Node --- */
            SkillTreeEffect swordShieldStartNodeEffect = new SwordShieldStartEffect();

            /* --- Tier 1 Nodes --- */
            // Increased Health
            SkillTreeEffect increasedHealthEffect = new IncreasedHpEffect();
            SkillTreeEffect increasedHealthEffect2 = new IncreasedHpEffect();

            // Increased Melee Damage
            SkillTreeEffect increasedMeleeDamageEffect = new IncreasedMeleeDamageEffect();
            SkillTreeEffect increasedMeleeDamageEffect2 = new IncreasedMeleeDamageEffect();

            // Added Block Effectiveness
            SkillTreeEffect addedBlockEffect = new AddedBlockEffect();
            SkillTreeEffect addedBlockEffect2 = new AddedBlockEffect();

            /* --- Masteries ---*/
            // Armour Mastery
            SkillTreeEffect armourMasteryEffect = new ArmourMasteryEffect();

            // Versatility Mastery
            SkillTreeEffect versatilityMasteryEffect = new VersatilityMasteryEffect(); 
            
            /* --- Tier 2 Nodes ---*/
            // Increased Health
            SkillTreeEffect increasedHealthEffect3 = new IncreasedHpEffect();
            SkillTreeEffect increasedHealthEffect4 = new IncreasedHpEffect();

            // Increased Melee Damage
            SkillTreeEffect increasedMeleeDamageEffect3 = new IncreasedMeleeDamageEffect();
            SkillTreeEffect increasedMeleeDamageEffect4 = new IncreasedMeleeDamageEffect();

            // Added Block Effectiveness
            SkillTreeEffect addedBlockEffect3 = new AddedBlockEffect();
            SkillTreeEffect addedBlockEffect4 = new AddedBlockEffect(); 
            
            /* --- Ascendencies ---*/
            // -- Gladiator --
            SkillTreeEffect slayer = new SlayerEffect();
            // -- Champion -- 
            SkillTreeEffect juggernaut = new JuggernautEffect();
            
            

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
            
            /* -- Tier 2 Nodes --*/
            SkillNode increasedHealthNode3 = new SkillNode("health_3", increasedHealthEffect3.name,
                increasedHealthEffect3.description, increasedHealthEffect3);
            SkillNode increasedHealthNode4 = new SkillNode("health_4", increasedHealthEffect4.name,
                increasedHealthEffect4.description, increasedHealthEffect4);

            SkillNode increasedMeleeDamageNode3 = new SkillNode("meleeDamage_3", increasedMeleeDamageEffect3.name,
                increasedMeleeDamageEffect3.description, increasedMeleeDamageEffect3);
            SkillNode increasedMeleeDamageNode4 = new SkillNode("meleeDamage_4", increasedMeleeDamageEffect4.name,
                increasedMeleeDamageEffect4.description, increasedMeleeDamageEffect4);

            SkillNode addedBlockEffectNode3 = new SkillNode("blockEffect_3", addedBlockEffect3.name,
                addedBlockEffect3.description, addedBlockEffect3);
            SkillNode addedBlockEffectNode4 = new SkillNode("blockEffect_4", addedBlockEffect4.name,
                addedBlockEffect4.description, addedBlockEffect4); 
            
            /* --- Ascendencies --*/
            SkillNode juggernautNode = new SkillNode("juggernautAsc", juggernaut.name,
                juggernaut.description, juggernaut);
            SkillNode slayerNode = new SkillNode("slayerAsc", slayer.name,
                slayer.description, slayer);
            
            /* --- Load the nodes into an array --- */
            SkillNode[] _skillNodes = new[]
            {
                swordShieldStartNode,
                increasedHealthNode1, increasedMeleeDamageNode1, addedBlockEffectNode1,
                increasedHealthNode2, increasedMeleeDamageNode2, addedBlockEffectNode2,
                armourMasteryNode, versatilityMasteryNode,
                increasedHealthNode3, increasedMeleeDamageNode3, addedBlockEffectNode3,
                increasedHealthNode4, increasedMeleeDamageNode4, addedBlockEffectNode4,
                juggernautNode, slayerNode
            };

        /* --- Construct a SkillTreeBranch ---*/
        branchSwordShield = new SkillTreeBranch("Sword and Shield Branch", _skillNodes);
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
        public SkillNode[] SkillNodes;
        
        /*------------------
         Class Constructor
         ------------------*/
        public SkillTreeBranch(string name, SkillNode[] skillNodes)
        {
            this._name = name;
            this.SkillNodes = skillNodes;
            
            /* --- Create the relationships in the tree --- */
            // Start Node
            skillNodes[0].children.Add(skillNodes[1]);
            skillNodes[0].children.Add(skillNodes[2]);
            skillNodes[0].children.Add(skillNodes[3]);
            
            // Tier 1
            // HP
            skillNodes[1].parent.Add(skillNodes[0]);
            skillNodes[1].children.Add(skillNodes[4]);
            skillNodes[4].parent.Add(skillNodes[1]);
            skillNodes[4].children.Add(skillNodes[7]);
            // DPS
            skillNodes[2].parent.Add(skillNodes[0]);
            skillNodes[2].children.Add(skillNodes[5]);
            skillNodes[5].parent.Add(skillNodes[2]);
            skillNodes[5].children.Add(skillNodes[7]);
            skillNodes[5].children.Add(skillNodes[8]);
            // BE
            skillNodes[3].parent.Add(skillNodes[0]);
            skillNodes[3].children.Add(skillNodes[6]);
            skillNodes[6].parent.Add(skillNodes[3]);
            skillNodes[6].children.Add(skillNodes[8]);
            
            // Masteries
            // Armour 
            skillNodes[7].parent.Add(skillNodes[4]);
            skillNodes[7].parent.Add(skillNodes[5]);
            skillNodes[7].children.Add(skillNodes[9]);
            skillNodes[7].children.Add(skillNodes[10]);
            // Versatile Combatant
            skillNodes[8].parent.Add(skillNodes[5]);
            skillNodes[8].parent.Add(skillNodes[6]); 
            skillNodes[8].children.Add(skillNodes[10]); 
            skillNodes[8].children.Add(skillNodes[11]);
            
            // Tier 2
            // HP
            skillNodes[9].parent.Add(skillNodes[7]);
            skillNodes[9].children.Add(skillNodes[12]);
            skillNodes[12].parent.Add(skillNodes[9]);
            skillNodes[12].children.Add(skillNodes[15]);
            // DPS
            skillNodes[10].parent.Add(skillNodes[7]);
            skillNodes[10].parent.Add(skillNodes[8]);
            skillNodes[10].children.Add(skillNodes[13]);
            skillNodes[13].parent.Add(skillNodes[10]);
            skillNodes[13].children.Add(skillNodes[15]);
            skillNodes[13].children.Add(skillNodes[16]);
            // BE
            skillNodes[11].parent.Add(skillNodes[8]);
            skillNodes[11].children.Add(skillNodes[14]);
            skillNodes[14].parent.Add(skillNodes[11]);
            skillNodes[14].children.Add(skillNodes[16]);
            
            // Ascendencies
            // Juggernaut
            skillNodes[15].parent.Add(skillNodes[12]);
            skillNodes[15].parent.Add(skillNodes[13]);
            
            // Slayer
            skillNodes[16].parent.Add(skillNodes[13]);
            skillNodes[16].parent.Add(skillNodes[14]);
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

        public SkillNode GetNodeByName(string name)
        {
            foreach (SkillNode node in SkillNodes)
            {
                if (node.name == name)
                {
                    return node;
                }
            }

            return null;
        }
    }
    
    /*------------------------------------------------------------------------------------------
     Represents a node in the skill tree,
     used to build out a data structure that holds the passive skill tree in it.
     ------------------------------------------------------------------------------------------*/
    public class SkillNode
    {
        public List<SkillNode> parent;
        public List<SkillNode> children;
        public string id;
        public string name;
        private string _description;
        public SkillTreeEffect _effect;
        
        /*------------------
         Class Constructor
         ------------------*/
        public SkillNode(string id, string name, string description, SkillTreeEffect effect)
        {
            this.id = id;
            this.name = name;
            this._description = description;
            this._effect = effect;
            this.parent = new List<SkillNode>();
            this.children = new List<SkillNode>();
        }
        
        /*------------------
        Getters and Setters for Parent and Children
        GetParent returns SkillNode that is the parent of this instance.
        GetChildren returns an array containing all children of the node.
        GetChild returns a specific child by id, name or index. **Name only returns the first child.**
         ------------------*/
        public List<SkillNode> GetParent { get => parent; set => parent = value; }
        public List<SkillNode> GetChildren { get => children; set => children = value; }
        
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
        public bool isActive { get; set; }
        
        public virtual void Apply()
        {
            throw new NotImplementedException();
        }

        public virtual void Clear()
        {
            throw new NotImplementedException();
        }

        public virtual void turnOff()
        {
            throw new NotImplementedException();
        }
    }
}