using UnityEngine;
using dialogues.data;

namespace dialogues.node
{
    [CreateAssetMenu(menuName = "node/dialogue")]
    public class DialogueNode : TreeNode
    {
        [SerializeField] string dialogue;
        [SerializeField] public ScriptableObject character;

        public string SpeakerName => iCharacter?.CharacterName;
        public Sprite SpeakerIcon => iCharacter?.CharacterIcon;
        public CharacterType CharacterType_ { 
            get {
                if (iCharacter != null) return iCharacter.CharacterType_;
                else return default(CharacterType);

            }
            set
            {

            }
        }
        public string Dialogue => dialogue;
        private ICharacter iCharacter { get => character as ICharacter; }

        public override NodeData GetData()
        {
            NodeData nodeData = base.GetData();
            DialogueNodeData dialogueData = new DialogueNodeData(nodeData);
            dialogueData.SpeakerIcone = SpeakerIcon;
            dialogueData.Dialogue = dialogue;
            dialogueData.SpeakerName = SpeakerName;
            dialogueData.DialogueSpeakerType = CharacterType_;
            return dialogueData;
        }

        public override void SetUpData(NodeData nodeData)
        {
            base.SetUpData(nodeData);
            DialogueNodeData dialogueData = nodeData as DialogueNodeData;
            //this.SpeakerName = dialogueData.SpeakerName;
            this.dialogue = dialogueData.Dialogue;
            //this.SpeakerIcon = dialogueData.SpeakerIcone;
        }

        public string GetDialogue()
        {
            return dialogue;    
        }
    }

    public enum CharacterType
    {
        None, NPC, Player,
    }
}
