using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            return new EndData(base.GetData());
        }
    }

    [Serializable]
    public class EndData : NodeData 
    {
        public EndData(NodeData nodeData) : base(nodeData) { }
    }
}
