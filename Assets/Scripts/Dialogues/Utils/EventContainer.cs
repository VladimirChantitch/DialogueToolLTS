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
    public class EventContainer {
        [SerializeReference] ScriptableObject eventableObject;
        public event Action selectedMethod;

        public IEventable EventableObject
        {
            get { return eventableObject as IEventable; }
        }

        public void SetSelectedMethod(Action selectedMethod)
        {
            this.selectedMethod = null;
            this.selectedMethod = selectedMethod;
        }

        public void ActivateSelectedMethod()
        {
            if (EventableObject == null)
            {
                Debug.Log("Please Implement the IEventable Interface or use the Dialogue Events Base Class");
                return;
            }
            EventableObject.PlayEvent();
        }
    }
}

