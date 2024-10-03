using System;
using GameplayMechanics.Effects;
using Unity.VisualScripting;
using UnityEngine;

namespace GameplayMechanics.Character
{
    public class SkillTree
    {
        
    }
    

    /*------------------------------------------------------------------------------------------
     Represents a branch in the skill tree.
     A branch is a 1/3 sector of the skill tree, categorised by weapon class.
     ------------------------------------------------------------------------------------------*/
    public class SkillTreeBranch
    {
        private string _name;
        private SkillNode[] _tier1Nodes;
        private SkillNode[] _NotableNodes;
        private SkillNode[] _tier2Nodes;
        private SkillNode[] _masteryNodes;
        
        /*------------------
         Class Constructor
         ------------------*/
        SkillTreeBranch(string name, SkillNode[] tier1, SkillNode[] notables, SkillNode[] tier2,
            SkillNode[] masteries)
        {
            this._name = name;
            this._tier1Nodes = tier1;
            this._NotableNodes = notables;
            this._tier2Nodes = tier2;
            this._masteryNodes = masteries;
        }
        
        /*------------------
         Returns a string that visualises a skill branch.
         Used for debugging purposes.
         ------------------*/
        public string GetSkillBranch()
        {
            string t1nodes = SkillBranchStringify(this._tier1Nodes);
            string t2nodes = SkillBranchStringify(this._tier2Nodes);
            string masterynodes = SkillBranchStringify(this._masteryNodes);
            string notables = SkillBranchStringify(this._NotableNodes);

            return this._name + " Skill branch \n" + t1nodes + t2nodes + masterynodes + notables;
        }
        
        /*------------------
         Returns a string that visualises an array of skill nodes.
         Used by GetSkillBranch() to visualise skill branches,
         for debugging purposes.
         -----------------*/
        private string SkillBranchStringify(SkillNode[] nodes)
        {
            string result = "[";
            foreach (SkillNode node in nodes)
            {
                result += node.id + " - " + node.name + ", ";
            }
            result += "]";
            return result;
        }
    }
    
    /*------------------------------------------------------------------------------------------
     Represents a node in the skill tree,
     used to build out a data structure that holds the passive skill tree in it.
     ------------------------------------------------------------------------------------------*/
    public class SkillNode
    {
        private SkillNode _parent;
        private SkillNode[] _children;
        public string id;
        public string name;
        private string _description;
        private SkillTreeEffect _effect;
        
        /*------------------
         Class Constructor
         ------------------*/
        SkillNode(string id, string name, string description, SkillTreeEffect effect)
        {
            this.id = id;
            this.name = name;
            this._description = description;
            this._effect = effect;
            this._parent = null;
            this._children = null;
        }
        
        /*------------------
        Getters and Setters for Parent and Children
        GetParent returns SkillNode that is the parent of this instance.
        GetChildren returns an array containing all children of the node.
        GetChild returns a specific child by id, name or index. **Name only returns the first child.**
         ------------------*/
        public SkillNode GetParent { get => _parent; set => _parent = value; }
        public SkillNode[] GetChildren { get => _children; set => _children = value; }
        
        public SkillNode GetChild(string name, int? index = null, string id = null)
        {
            if (index != null)
            {
                return _children[index.Value];
            }

            if (id != null)
            {
                foreach (SkillNode node in _children)
                {
                    if (node.id == id)
                    {
                        return node;
                    }
                }
            }
            
            foreach (SkillNode node in _children)
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