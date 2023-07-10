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
        public RootNode rootNode;
        public List<TreeNode> nodes = new List<TreeNode>(); 

        public event EventHandler<TreeNode> OnChildAdded;
        public event EventHandler<TreeNode> OnChildRemoved;
        public event EventHandler<List<TreeNode>> OnNodeModelLoaded;

        public TreeHandler()
        {
            nodeGenerator = new TreeNodeGenerator();
        }

        public TreeHandler(RootNode currentRootNode)
        {
            rootNode = currentRootNode;
            nodeGenerator = new TreeNodeGenerator();
        }


        public void UseAnotherRoot(RootNode newRoot)
        {
            rootNode = newRoot;
            LoadNewTree();
        }

        private void LoadNewTree()
        {
            nodes.Clear();
            nodes.AddRange(rootNode.GetNodeModel());
            OnNodeModelLoaded?.Invoke(this, nodes);
        }

        public bool DeleteNode(NodeData data)
        {
            if (data is RootData) return false;
            try
            {
                TreeNode node = LookForNode(data);
                string path = AssetDatabase.GetAssetPath(node.GetInstanceID());
                if (node is RootNode)
                {
                    AssetDatabase.DeleteAsset(path);
                }
                else
                {
                    rootNode.nodesModel.Remove(node);
                    AssetDatabase.RemoveObjectFromAsset(node);
                }

                AssetDatabase.SaveAssets();

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
            List<NodeData> datas = parentNode.GetChildrenData();   
            
            return datas;
        }

        private TreeNode LookForNode(NodeData data)
        {
            if (!nodes.Contains(rootNode)) nodes.Add(rootNode);
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
            node.SetUpData(data);
        }

        public TreeNode CreateNodeFromData(NodeData data)
        {
            TreeNode node = nodeGenerator.GenerateNodeFromData(data);
            rootNode.AddNodeToModel(node);

            AddAssetToRootNode(node);
            return node;
        }

        public NodeData CreateNodeCopyFromData(NodeData data)
        {
            AssetDatabase.SaveAssets();
            TreeNode node = nodeGenerator.GenerateNodeCopyFromData(data);
            nodes.Add(node);
            rootNode.AddNodeToModel(node);

            AddAssetToRootNode(node);
            return node.GetData();
        }

        public NodeData CreateNode(Type type)
        {
            AssetDatabase.SaveAssets();
            TreeNode node = nodeGenerator.GenerateNode(type);

            if (node is RootNode)
            {
                return null;
            }

            nodes.Add(node);
            rootNode.AddNodeToModel(node);

            AddAssetToRootNode(node);
            return node.GetData();
        }

        public (bool,NodeData) CheckForRootNode()
        {
            if (rootNode == null)
            {
                rootNode = (RootNode)nodeGenerator.GenerateNode(typeof(RootNode));
                nodes.Add(rootNode);
                rootNode.AddNodeToModel(rootNode);
                CreateAssetInDataBase(rootNode);
                return (false, rootNode.GetData());
            }

            return (true,rootNode.GetData());
        }

        public void CreateAssetInDataBase(TreeNode node)
        {
            string path = "Assets/";
            path += node.name;
            path += ".asset";
            AssetDatabase.CreateAsset(node, path);
            AssetDatabase.SaveAssets();
        }

        public void AddAssetToRootNode(TreeNode node)
        {
            AssetDatabase.AddObjectToAsset(node, rootNode);
            AssetDatabase.SaveAssets();
        }
    }
}

