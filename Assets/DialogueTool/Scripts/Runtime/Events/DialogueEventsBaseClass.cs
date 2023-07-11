using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace dialogues.eventSystem
{
    [CreateAssetMenu(menuName = "Events/BaseDialogueEvent")]
    public class DialogueEventsBaseClass : ScriptableObject, IEventable
    {
        public virtual void PlayEvent()
        {
            Debug.Log("General Kenobi?");
        }
    }
}

