using System;
using System.Collections.Generic;
using System.Linq;

namespace LuccaDevises.dataLayer
{
    public class TreeNode
    {
        public ExchangeRate Value { get; set; }
        public string UniqueBranchId {
            get {
                if (IsRoot())
                    return Value.LeftCurrency;
                else
                    return Value.LeftCurrency != Parent.UniqueBranchId ? Value.LeftCurrency : Value.RightCurrency;
            }
        }
        public TreeNode Parent { get; set; }
        public List<TreeNode> Children { get; set; }
        public TreeNode(ExchangeRate value)
        {
            this.Value = value;
            Children = new List<TreeNode>();
        }


        public void AddChild(ExchangeRate child)
        {
            var childNode = new TreeNode(child);
            childNode.Parent = this;
            this.Children.Add(childNode);
        }

        public List<string> GetBranchIds()
        {
            var branchIds = new List<string>() { this.UniqueBranchId };
            if (IsRoot())
                return branchIds;
            return branchIds.Concat(this.Parent.GetBranchIds()).ToList();
        }

        public int GetIndex()
        {
            if (IsRoot())
                return 0;
            return this.Parent.GetIndex() + 1;
        }

        public TreeNode GetRoot()
        {
            if (IsRoot())
                return this;
            return this.Parent.GetRoot();
        }

        public bool IsRoot()
        {
            if (Parent != null)
                return false;
            return true;
        }

        public List<TreeNode> GetNodesFromLevel(int level)
        {
            if (level == GetIndex())
            {
                return new List<TreeNode>() { this };
            }
            return Children.SelectMany(c => c.GetNodesFromLevel(level)).ToList();
        }

        
    }
}
