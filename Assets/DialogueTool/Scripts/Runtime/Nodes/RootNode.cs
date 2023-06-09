using dialogues.data;
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
            return nodesModel;
        }

        public void AddNodeToModel(TreeNode node)
        {
            nodesModel.Add(node);
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
            return new RootNodeData(base.GetData());
        }
    }
}

