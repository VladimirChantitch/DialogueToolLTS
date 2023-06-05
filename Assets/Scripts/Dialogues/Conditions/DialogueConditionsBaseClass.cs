using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Condition/DialogueBaseCondition")]
public class DialogueConditionsBaseClass : ScriptableObject, IConditionable
{
    [SerializeField] bool isIt;
    public virtual bool PlayCondition()
    {
        return isIt;
    }
}
