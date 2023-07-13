using UnityEngine;
using dialogues.data;

namespace dialogues.node
{
    [CreateAssetMenu(menuName = "node/end")]
    public class EndNode : TreeNode
    {
        public override bool AddChild(TreeNode newChild)
        {
            return false;
        }

        public override NodeData GetData()
        {
            return new EndNodeData(base.GetData());
        }
    }
}
