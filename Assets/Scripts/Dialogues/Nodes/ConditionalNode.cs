using dialogues;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace dialogues.node
{
    [CreateAssetMenu(menuName = "node/condition")]
    public class ConditionalNode : TreeNode
    {
        [SerializeField] List<ConditionContainer> conditionContainers = new List<ConditionContainer>();

        public override bool AddChild(TreeNode newChild)
        {
            if (directChildren == null) directChildren = new List<TreeNode>();
            if (DirectChildren.Count >= 2) return false;

            directChildren.Add(newChild);
            return true;
        }

        public override List<TreeNode> GetNextNode()
        {
            List<TreeNode> res = new List<TreeNode>();
            if (PlayAllConditions())
            {
                res.Add(base.GetNextNode()[0]);
            }
            else
            {
                res.Add(base.GetNextNode()[1]);
            }
            return res;
        }

        public virtual bool PlayAllConditions()
        {
            bool isTrue = false;
            for (int i = 0; i < conditionContainers.Count; i++)
            {
                isTrue = conditionContainers[i].ActivateSelectedMethod();
                if (isTrue == false)
                {
                    return false;
                }
            }

            if (isTrue == true)
            {
                return true;
            }

            return false;
        }
    }
}

