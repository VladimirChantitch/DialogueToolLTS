using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Utils
{
    [Serializable]
    public class ConditionContainer
    {
        [SerializeReference] DialogueConditionsBaseClass conditionableObject;
        public event Action selectedMethod;

        public ConditionContainer() { }

        public ConditionContainer(DialogueConditionsBaseClass conditionableObject)
        {
            this.conditionableObject = conditionableObject;
        }

        public IConditionable ConditionableObject
        {
            get { return conditionableObject as IConditionable; }
        }

        public void SetSelectedMethod(Action selectedMethod)
        {
            this.selectedMethod = null;
            this.selectedMethod = selectedMethod;
        }

        public bool ActivateSelectedMethod()
        {
            if (ConditionableObject == null)
            {
                Debug.Log("Please Implement the ICondition Interface or use the Dialogue Events Base Class");
                return false;
            }
            return ConditionableObject.PlayCondition();
        }
    }
}