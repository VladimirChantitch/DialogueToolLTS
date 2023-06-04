using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEditor;
using System;

namespace dialogues
{
    [CreateAssetMenu()]
    public class DialogueTree : ScriptableObject
    {
        [SerializeField] private string Name;
        public TreeNode RootNode;
        public List<TreeNode> nodes = new List<TreeNode>();

        public event EventHandler<TreeNode> OnChildAdded;
        public event EventHandler<TreeNode> OnChildRemoved;

        public TreeNode CreateNode(System.Type type)
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

        public TreeNode CreateNode(System.Type type, NodeData nodeData)
        {
            TreeNode node = ScriptableObject.CreateInstance(type) as TreeNode;
            node.name = type.Name;
            node.guid = GUID.Generate().ToString();
            node.SetUpData(nodeData);

            Undo.RecordObject(this, "Dialogue Tree (CreateNode)");
            nodes.Add(node);

            if (!Application.isPlaying) AssetDatabase.AddObjectToAsset(node, this);

            Undo.RegisterCreatedObjectUndo(node, "Behaviour Tree (CreateNode)");

            AssetDatabase.SaveAssets();

            return node;
        }

        public void DeleteNode(TreeNode node)
        {
            node.Delete();
        }

        public void AddChild(TreeNode parent, TreeNode child)
        {
            parent.AddChild(child);
        }

        public void RemoveChild(TreeNode parent, TreeNode child)
        {
            parent.RemoveChild(child);
        }

        public List<TreeNode> GetChildren(TreeNode parent)
        {
            if (parent.DirectChildren != null)
            {
                return parent.DirectChildren;
            }
            return new List<TreeNode>();
        }

        public void Traverse(TreeNode node, System.Action<TreeNode> visiter)
        {
            if (node)
            {
                visiter.Invoke(node);
                var children = GetChildren(node);
                children.ForEach((n) => Traverse(n, visiter));
            }
        }

        public DialogueTree Clone()
        {
            DialogueTree tree = Instantiate(this);
            tree.RootNode = tree.RootNode.Clone();
            tree.nodes = new List<TreeNode>();
            Traverse(tree.RootNode, (n) =>
            {
                tree.nodes.Add(n);
            });
            return tree;
        }
    }
}

