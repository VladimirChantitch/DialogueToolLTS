using dialogues.node;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static Codice.Client.Common.Connection.AskCredentialsToUser;

public class TreeNodeGenerator
{
    internal TreeNode GenerateNode(Type type)
    {
        TreeNode node = ScriptableObject.CreateInstance(type) as TreeNode;
        node.name = type.Name;
        node.guid = GUID.Generate().ToString();
        return node;
    }

    public TreeNode GenerateNodeFromData(NodeData data)
    {
        TreeNode node = null;
        switch (data)
        {
            case DialogueData dialogueData:
                node = ScriptableObject.CreateInstance<DialogueNode>();
                node.SetUpData(dialogueData);
                return node;
            case ConditionalData conditionalData:
                node = ScriptableObject.CreateInstance<ConditionalNode>();
                node.SetUpData(conditionalData);
                return node;
            case EndData endData:
                node = ScriptableObject.CreateInstance<EndNode>();
                node.SetUpData(endData);
                return node;
            case RootData rootData:
                node = ScriptableObject.CreateInstance<RootNode>();
                node.SetUpData(rootData);
                return node;
            default: 
                return node;
        }
    }

    public TreeNode GenerateNodeCopyFromData(NodeData data)
    {
        TreeNode node = GenerateNodeFromData(data);

        node.name = node.GetType().Name;
        node.guid = GUID.Generate().ToString();

        return node;
    }


}
