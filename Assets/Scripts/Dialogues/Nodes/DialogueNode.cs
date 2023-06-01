using dialogues;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Utils;

[CreateAssetMenu(menuName = "Dialogue")]
public class DialogueNode : TreeNode
{
    [SerializeField] string speakerName;
    [SerializeField] Sprite speakerIcone;
    [SerializeField] string Dialogue;
}
