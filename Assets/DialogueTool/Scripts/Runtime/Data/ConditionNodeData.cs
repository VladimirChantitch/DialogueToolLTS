using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace dialogues.data
{
    public class ConditionNodeData : NodeData
    {
        public ConditionNodeData() { }

        public ConditionNodeData(NodeData nodeData) : base(nodeData) { }

        public override NodeData Clone(NodeData nodeData)
        {
            ConditionNodeData Original = (ConditionNodeData)nodeData;
            ConditionNodeData conditionalData = new ConditionNodeData();

            conditionalData.position = Original.Position;
            conditionalData.guid = Original.Guid;
            conditionalData.eventContainers = Original.EventContainers;
            conditionalData.conditionContainers = Original.conditionContainers;

            return conditionalData;
        }

        [SerializeField] protected List<ConditionContainer> conditionContainers = new List<ConditionContainer>();
        public List<ConditionContainer> ConditionContainers { get => conditionContainers; set => conditionContainers = value; }

        public NodeData trueChild;
        public NodeData falseChild;

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

