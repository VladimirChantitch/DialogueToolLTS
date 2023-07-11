using dialogues;
using dialogues.eventSystem;
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
            ConditionData conditionalData = new ConditionData(nodeData);
            conditionalData.ConditionContainers.AddRange(conditionContainers);
            return conditionalData;
        }

        public override void SetUpData(NodeData nodeData)
        {
            base.SetUpData(nodeData);
            ConditionData conditionalData = nodeData as ConditionData;
            conditionContainers.Clear();
            this.conditionContainers.AddRange(conditionalData.ConditionContainers);
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

    public class ConditionData : NodeData
    {
        public ConditionData() { }

        public ConditionData(NodeData nodeData) : base(nodeData){}

        public override NodeData Clone(NodeData nodeData)
        {
            ConditionData Original = (ConditionData)nodeData;
            ConditionData conditionalData = new ConditionData();

            conditionalData.position = Original.Position;
            conditionalData.guid = Original.Guid;
            conditionalData.eventContainers = Original.EventContainers;
            conditionalData.conditionContainers = Original.conditionContainers;

            return conditionalData;
        }

        [SerializeField] protected List<ConditionContainer> conditionContainers = new List<ConditionContainer>();
        public List<ConditionContainer> ConditionContainers { get => conditionContainers; set => conditionContainers = value; }

        public void InsertConditionAtIndex(DialogueConditionsBaseClass conditionsBaseClass, int index)
        {
            conditionContainers.Insert(index, new ConditionContainer(conditionsBaseClass));        
        }

        public void RemoveConditionAtIndex(int index)
        {
            if (index >= 0 && index < conditionContainers.Count)
            {
                conditionContainers.RemoveAt(index);
            }
        }

        public void UpdateConditionsBasedOnFields(List<DialogueConditionsBaseClass> dialogueConditionsBaseClasses)
        {
            ConditionContainers.Clear();
            dialogueConditionsBaseClasses.ForEach((dc) =>
            {
                ConditionContainers.Add(new ConditionContainer(dc));
            });
        }
    }
}

