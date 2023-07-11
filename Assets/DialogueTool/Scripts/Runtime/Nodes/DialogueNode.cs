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
        [SerializeField] DialogueSpeakerType dialogueSpeakerType;

        public string SpeakerName => speakerName;
        public Sprite SpeakerIcon => speakerIcone;
        public string Dialogue => dialogue;

        public override NodeData GetData()
        {
            NodeData nodeData = base.GetData();
            DialogueData dialogueData = new DialogueData(nodeData);
            dialogueData.SpeakerIcone = speakerIcone;
            dialogueData.Dialogue = dialogue;
            dialogueData.SpeakerName = speakerName;
            dialogueData.DialogueSpeakerType = dialogueSpeakerType;
            return dialogueData;
        }

        public override void SetUpData(NodeData nodeData)
        {
            base.SetUpData(nodeData);
            DialogueData dialogueData = nodeData as DialogueData;
            this.speakerName = dialogueData.SpeakerName;
            this.dialogue = dialogueData.Dialogue;
            this.speakerIcone = dialogueData.SpeakerIcone;
        }

        public string GetDialogue()
        {
            return dialogue;    
        }
    }

    public class DialogueData : NodeData
    {
        public DialogueData() { }

        public DialogueData(NodeData nodeData): base(nodeData) { }

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
        [SerializeField] DialogueSpeakerType dialogueSpeakerType;

        public string SpeakerName { get => speakerName; set => speakerName = value; }
        public Sprite SpeakerIcone { get => speakerIcone; set => speakerIcone = value; }
        public string Dialogue { get => dialogue; set => dialogue = value; }
        public DialogueSpeakerType DialogueSpeakerType { get => dialogueSpeakerType; set => dialogueSpeakerType = value; }
    }

    public enum DialogueSpeakerType
    {
        NPC, Player
    }
}
