
using dialogues.node;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace dialogues.data
{
    public class DialogueNodeData : NodeData
    {
        public DialogueNodeData() { }

        public DialogueNodeData(NodeData nodeData) : base(nodeData) { }

        public override NodeData Clone(NodeData nodeData)
        {
            DialogueNodeData Original = (DialogueNodeData)nodeData;
            DialogueNodeData dialogueData = new DialogueNodeData();

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
        [SerializeField] CharacterType dialogueSpeakerType;

        public string SpeakerName { get => speakerName; set => speakerName = value; }
        public Sprite SpeakerIcone { get => speakerIcone; set => speakerIcone = value; }
        public string Dialogue { get => dialogue; set => dialogue = value; }
        public CharacterType DialogueSpeakerType { get => dialogueSpeakerType; set => dialogueSpeakerType = value; }
    }
}

