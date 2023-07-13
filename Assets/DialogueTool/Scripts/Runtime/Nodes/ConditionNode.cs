using dialogues.data;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;
using Utils;

namespace dialogues.node
{
    [CreateAssetMenu(menuName = "node/condition")]
    public class ConditionNode : TreeNode
    {
        [SerializeField] List<ConditionContainer> conditionContainers = new List<ConditionContainer>();
        public List<ConditionContainer> ConditionContainers => conditionContainers;

        public override bool AddChild(TreeNode newChild)
        {
            if (directChildren == null) directChildren = new List<TreeNode>();
            if (DirectChildren.Count >= 2) return false;

            directChildren.Add(newChild);
            return true;
        }

        public override List<TreeNode> GetNextNodes()
        {
            List<TreeNode> res = new List<TreeNode>();
            if (PlayAllConditions())
            {
                res.Add(base.GetNextNodes()[1]);
            }
            else
            {
                res.Add(base.GetNextNodes()[0]);
            }
            return res;
        }

        public override NodeData GetData()
        {
            NodeData nodeData = base.GetData();
            ConditionNodeData conditionalData = new ConditionNodeData(nodeData);
            conditionalData.ConditionContainers.AddRange(conditionContainers);
            if (directChildren.Count < 1)
            {
                directChildren.Add(null);
                directChildren.Add(null);
            }
            else if (directChildren.Count < 2)
            {
                directChildren.Add(null);
            }
            conditionalData.trueChild = directChildren[0]?.GetData();
            conditionalData.falseChild = directChildren[1]?.GetData();   
            return conditionalData;
        }

        public override void SetUpData(NodeData nodeData)
        {
            base.SetUpData(nodeData);
            ConditionNodeData conditionData = nodeData as ConditionNodeData;
            conditionContainers.Clear();
            this.conditionContainers.AddRange(conditionData.ConditionContainers);
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

