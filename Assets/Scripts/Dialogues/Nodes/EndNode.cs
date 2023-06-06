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
    }

    [Serializable]
    public class EndData : NodeData { }
}
