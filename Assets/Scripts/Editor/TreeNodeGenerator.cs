using dialogues.node;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TreeNodeGenerator
{
    public TreeNode GenerateNodeFromData(NodeData data)
    {
        switch (data)
        {
            default:
                return null;
        }
    }

    internal NodeData GenerateNode(Type type)
    {
        TreeNode node = ScriptableObject.CreateInstance(type) as TreeNode;
        node.name = type.Name;
        node.guid = GUID.Generate().ToString();

        Undo.RecordObject(this, "Behaviour Tree (CreateNode)");
        nodes.Add(node);

        if (!Application.isPlaying) AssetDatabase.AddObjectToAsset(node, this);

        Undo.RegisterCreatedObjectUndo(node, "Behaviour Tree (CreateNode)");

        AssetDatabase.SaveAssets();

        return node;
    }
}
