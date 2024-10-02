using System;
using GameplayMechanics.Effects;
using UnityEngine;

namespace GameplayMechanics.Character
{
    public class SkillTree
    {
        
    }

    public class SkillTreeBranch
    {
        private string _name;
        private SkillNode[] _tier1Nodes;
        private SkillNode[] _NotableNodes;
        private SkillNode[] _tier2Nodes;
        private SkillNode[] _masteryNodes;

        SkillTreeBranch(string name, SkillNode[] tier1, SkillNode[] notables, SkillNode[] tier2,
            SkillNode[] masteries)
        {
            this._name = name;
            this._tier1Nodes = tier1;
            this._NotableNodes = notables;
            this._tier2Nodes = tier2;
            this._masteryNodes = masteries;
        }

        public string GetSkillBranch()
        {
            string t1nodes = SkillBranchStringify(this._tier1Nodes);
            string t2nodes = SkillBranchStringify(this._tier2Nodes);
            string masterynodes = SkillBranchStringify(this._masteryNodes);
            string notables = SkillBranchStringify(this._NotableNodes);

            return this._name + " Skill branch \n" + t1nodes + t2nodes + masterynodes + notables;
        }

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


    public class SkillNode
    {
        private SkillNode _parent;
        private SkillNode[] _children;
        public string id;
        public string name;
        private string _description;
        private SkillTreeEffect _effect;

        SkillNode(string id, string name, string description, SkillTreeEffect effect)
        {
            this.id = id;
            this.name = name;
            this._description = description;
            this._effect = effect;
            this._parent = null;
            this._children = null;
        }
        
        // Getters and Setters for Parent and Children
        // GetChildren will return an array containing all children of the node.
        public SkillNode GetParent { get => _parent; set => _parent = value; }
        public SkillNode[] GetChildren { get => _children; set => _children = value; }
        
        // Search function to get a specific child.
        public SkillNode GetChild(string name, int? index = null, string id = null)
        {
            if (index != null)
            {
                // Return the child at that index
                return _children[index.Value];
            }

            if (id != null)
            {
                // Return the child with that id
                foreach (SkillNode node in _children)
                {
                    if (node.id == id)
                    {
                        return node;
                    }
                }
            }
            
            // Otherwise, search the children by name and return the correct child.
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

    public class SkillTreeEffect : IEffect
    {
        private SkillTreeEffect(EffectType effectType, string name, string description,
            float duration)
        {
            EffectType = effectType;
            Name = name;
            Description = description;
            Duration = duration;
        }

        public EffectType EffectType { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float Duration { get; set; }

        public GameObject target { get; set; } // Temp

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