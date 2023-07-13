using dialogues.node;
using dialogues.data;
using System;
using UnityEditor;
using UnityEngine;

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
            case DialogueNodeData dialogueData:
                node = ScriptableObject.CreateInstance<DialogueNode>();
                node.SetUpData(dialogueData);
                break;
            case ConditionNodeData conditionalData:
                node = ScriptableObject.CreateInstance<ConditionNode>();
                node.SetUpData(conditionalData);
                break;
            case EndNodeData endData:
                node = ScriptableObject.CreateInstance<EndNode>();
                node.SetUpData(endData);
                break;
            case RootNodeData rootData:
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
