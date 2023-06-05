using dialogues;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace dialogues.node
{
    [CreateAssetMenu(menuName = "node/root")]
    public class RootNode : TreeNode
    {
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
    }
}

