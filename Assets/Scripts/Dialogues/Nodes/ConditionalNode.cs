using dialogues;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class ConditionalNode : TreeNode
{
    [SerializeField] List<ConditionContainer> conditionContainers = new List<ConditionContainer>();

    public override List<TreeNode> GetNextNode()
    {
        return base.GetNextNode();
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
