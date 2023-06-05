using dialogues;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Utils;

namespace dialogues.node
{
    [CreateAssetMenu(menuName = "node/dialogue")]
    public class DialogueNode : TreeNode
    {
        [SerializeField] string speakerName;
        [SerializeField] Sprite speakerIcone;
        [SerializeField] string Dialogue;

        public string GetDialogue()
        {
            return Dialogue;    
        }
    }
}
