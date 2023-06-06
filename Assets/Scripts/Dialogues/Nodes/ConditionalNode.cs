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
            ConditionalData conditionalData = new ConditionalData(nodeData);
            conditionalData.ConditionContainers.AddRange(conditionContainers);
            return conditionalData;
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

    public class ConditionalData : NodeData
    {
        public ConditionalData() { }

        public ConditionalData(NodeData nodeData)
        {
            this.position = nodeData.Position;
            this.guid = nodeData.Guid;
            this.eventContainers = nodeData.EventContainers;
        }

        public override NodeData Clone(NodeData nodeData)
        {
            ConditionalData Original = (ConditionalData)nodeData;
            ConditionalData conditionalData = new ConditionalData();

            conditionalData.position = Original.Position;
            conditionalData.guid = Original.Guid;
            conditionalData.eventContainers = Original.EventContainers;
            conditionalData.conditionContainers = Original.conditionContainers;

            return conditionalData;
        }

        [SerializeField] protected List<ConditionContainer> conditionContainers = new List<ConditionContainer>();
        public List<ConditionContainer> ConditionContainers { get => conditionContainers; set => conditionContainers = value; }
    }
}

