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
        [SerializeField] string dialogue;

        public string GetDialogue()
        {
            return dialogue;    
        }
    }

    public class DialogueData : NodeData
    {
        public override NodeData Clone(NodeData nodeData)
        {
            DialogueData Original = (DialogueData)nodeData;
            DialogueData dialogueData = new DialogueData();

            dialogueData.position = Original.Position;
            dialogueData.guid = Original.Guid;
            dialogueData.eventContainers = Original.EventContainers;
            dialogueData.speakerIcone = Original.SpeakerIcone;
            dialogueData.speakerName = Original.SpeakerName;
            dialogueData.dialogue = Original.Dialogue;

            return dialogueData;
        }

        [SerializeField] string speakerName;
        [SerializeField] Sprite speakerIcone;
        [SerializeField] string dialogue;

        public string SpeakerName { get => speakerName; set => speakerName = value; }
        public Sprite SpeakerIcone { get => speakerIcone; set => speakerIcone = value; }
        public string Dialogue { get => dialogue; set => dialogue = value; }
    }
}
