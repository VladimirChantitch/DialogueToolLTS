using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEditor;
using System;
using dialogues.node;

namespace dialogues.editor
{
    public class TreeHandler
    {
        [SerializeField] private string Name;
        TreeNodeGenerator nodeGenerator = null;
        public TreeNode RootNode;
        public List<TreeNode> nodes = new List<TreeNode>();

        public event EventHandler<TreeNode> OnChildAdded;
        public event EventHandler<TreeNode> OnChildRemoved;

        public TreeHandler()
        {
            nodeGenerator = new TreeNodeGenerator();
        }

        public void UseAnotherRoot(RootNode newRoot)
        {
            SavePreviousTree();
            RootNode = newRoot;
            LoadNewTree();
        }

        private void SavePreviousTree()
        {
            if (RootNode != null)
            {
                // Save previous tree in its current state
            }
        }

        private void LoadNewTree()
        {
            throw new NotImplementedException();
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

        public bool DeleteNode(NodeData data)
        {
            try
            {
                TreeNode node = LookForNode(data);
                string path = AssetDatabase.GetAssetPath(node.GetInstanceID());
                AssetDatabase.DeleteAsset(path);
                return true;
            }
            catch (Exception ex)
            {
                Debug.Log($"<color=orange> {ex} </color>");
                return false;
            }
        }

        public bool AddOrUpdateChild(NodeData parent, NodeData child)
        {
            try
            {
                TreeNode parentNode = LookForNode(parent);
                TreeNode childNode = LookForNode(child);

                if (parentNode.AddChild(childNode))
                {
                    return true;
                }
                return false;
            }
            catch(Exception ex)
            {
                Debug.Log($"<color=orange> {ex} </color>");
                return false;
            }

        }

        public bool RemoveChild(NodeData parent, NodeData child)
        {
            try
            {
                TreeNode parentNode = LookForNode(parent);
                TreeNode childNode = LookForNode(child);

                parentNode.RemoveChild(childNode);

                return true;
            }
            catch(Exception ex )
            {
                Debug.Log($"<color=orange> {ex} </color>");
                return false;
            }

        }

        public List<NodeData> GetChildren(NodeData parent)
        {
            TreeNode parentNode = LookForNode(parent);
            List<NodeData> datas = new List<NodeData>();    
            //if (parent.DirectChildren != null)
            //{
            //    return parent.DirectChildren;
            //}
            return datas;
        }

        private TreeNode LookForNode(NodeData data)
        {
            TreeNode node = nodes.Find(n => n.guid == data.Guid);
            if (node == null)
            {
                node = CreateNodeFromData(data);
            }
            else
            {
                UpdateNodeFromData(data, node);
            }
            return node;
        }

        public void UpdateNode(NodeData data)
        {
            LookForNode(data);
        }

        private void UpdateNodeFromData(NodeData data, TreeNode node)
        {
            switch (node)
            {
                case ConditionalNode conditionalNode:

                    break;

                case DialogueNode dialogueNode:

                    break;

                case EndNode endNode:

                    break;

                case RootNode rootNode:

                    break;
            }
        }

        private TreeNode CreateNodeFromData(NodeData data)
        {
            return nodeGenerator.GenerateNodeFromData(data);
        }

        private NodeData CreateNode(Type type)
        {
            return nodeGenerator.GenerateNode(type);
        }
    }
}

//public void Traverse(TreeNode node, System.Action<TreeNode> visiter)
//{
//    if (node)
//    {
//        visiter.Invoke(node);
//        var children = GetChildren(node);
//        children.ForEach((n) => Traverse(n, visiter));
//    }
//}

//public DialogueSystemTree Clone()
//{
//    DialogueSystemTree tree = Instantiate(this);
//    tree.RootNode = tree.RootNode.Clone();
//    tree.nodes = new List<TreeNode>();
//    Traverse(tree.RootNode, (n) =>
//    {
//        tree.nodes.Add(n);
//    });
//    return tree;
//}

