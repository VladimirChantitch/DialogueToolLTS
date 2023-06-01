using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Condition/DialogueBaseCondition")]
public class DialogueConditionsBaseClass : ScriptableObject, IConditionable
{
    public virtual bool PlayCondition()
    {
        Debug.Log("Condition");
        return true;
    }
}
