using dialogues;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace dialogues.node
{
    [CreateAssetMenu(menuName = "node/root")]
    public class RootNode : TreeNode
    {
        public List<TreeNode> nodesModel = new List<TreeNode>();

        public List<TreeNode> GetNodeModel()
        {
            UpdateNodeModel();
            return nodesModel;
        }

        public void AddNodeToModel(TreeNode node)
        {
            nodesModel.Add(node);
        }

        private void UpdateNodeModel()
        {
            nodesModel = GetChildrenNodeModel();
        }

        public override bool AddChild(TreeNode newChild)
        {
            if (directChildren == null) directChildren = new List<TreeNode>();
            if (DirectChildren.Count >= 1)
            {
                directChildren[0] = newChild;
            }
            else
            {
                directChildren.Add(newChild);
            }

            return true;
        }

        public override NodeData GetData()
        {
            return new RootData(base.GetData());
        }
    }

    [Serializable]
    public class RootData : NodeData
    {
        public RootData(NodeData nodeData) : base(nodeData)
        {

        }
    }
}

