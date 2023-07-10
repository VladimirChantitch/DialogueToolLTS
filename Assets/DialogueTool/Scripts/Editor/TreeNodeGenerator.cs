using dialogues.node;
using log4net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.AccessControl;
using UnityEditor;
using UnityEngine;
using static Codice.Client.Common.Connection.AskCredentialsToUser;
using static UnityEditor.Progress;

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
                break;
            case ConditionalData conditionalData:
                node = ScriptableObject.CreateInstance<ConditionalNode>();
                node.SetUpData(conditionalData);
                break;
            case EndData endData:
                node = ScriptableObject.CreateInstance<EndNode>();
                node.SetUpData(endData);
                break;
            case RootData rootData:
                node = ScriptableObject.CreateInstance<RootNode>();
                node.SetUpData(rootData);
                break;
        }

        return node;
    }

    public TreeNode GenerateNodeCopyFromData(NodeData data)
    {
        TreeNode node = GenerateNodeFromData(data);

        node.name = node.GetType().Name;
        node.guid = GUID.Generate().ToString();

        return node;
    }
}
