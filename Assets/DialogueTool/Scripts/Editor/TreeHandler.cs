using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEditor;
using System;
using dialogues.node;
using System.Linq;

namespace dialogues.editor.treeHandler
{
    public class TreeHandler : ITreeHandler
    {
        [SerializeField] private string Name;
        TreeNodeGenerator nodeGenerator = null;
        public RootNode rootNode;
        public RootNode RootNode => rootNode;
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
            LoadNewTree();
        }


        public void UseAnotherRoot(RootNode newRoot)
        {
            rootNode = newRoot;
            LoadNewTree();
        }

        private void LoadNewTree()
        {
            nodes.Clear();
            nodes.AddRange(rootNode.nodesModel);
            OnNodeModelLoaded?.Invoke(this, nodes);
        }

        public bool DeleteNodeFromData(NodeData data)
        {
            if (data is RootData) return false;
            try
            {
                if (data == null) throw new Exception("null data");
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

        public bool CreateOrUpdateChild(NodeData parent, NodeData child)
        {
            try
            {
                if (child == null) throw new Exception("child is null");
                if (parent == null) throw new Exception("parent is null");
                TreeNode parentNode = LookForNode(parent);
                TreeNode childNode = LookForNode(child);

                if (parentNode.AddChild(childNode))
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
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
            catch (Exception ex)
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
            if (data == null) return null;
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

            if (node is ConditionNode conditionNode)
            {
                conditionNode.DirectChildren.Clear();
                TreeNode trueNode = null;
                TreeNode falseNode = null;
                ConditionData conditionData = (data as ConditionData);
                if (conditionData.trueChild != null) trueNode = rootNode.nodesModel.Find(n => n.guid == conditionData.trueChild.Guid);
                if (conditionData.falseChild != null) falseNode = rootNode.nodesModel.Find(n => n.guid == conditionData.falseChild.Guid);
                conditionNode.DirectChildren.Add(trueNode);
                conditionNode.DirectChildren.Add(falseNode);
            }
        }

        public TreeNode CreateNodeFromData(NodeData data)
        {
            if (data == null) return null;

            TreeNode node = nodeGenerator.GenerateNodeFromData(data);

            if (node is RootNode && rootNode == null)
            {
                rootNode = node as RootNode;
                CreateAssetInDataBase(node);
            }
            else
            {
                if (rootNode.nodesModel.Find(n => n.guid == data.Guid) != null)
                {
                    return null;
                }
            }
            if (node != null)
            {
                nodes.Add(node);
                this.rootNode.AddNodeToModel(node);
                CreateAssetToRootNode(node);
            }

            return node;
        }

        public NodeData CreateNodeCopyFromData(NodeData data)
        {
            AssetDatabase.SaveAssets();
            TreeNode node = nodeGenerator.GenerateNodeCopyFromData(data);
            nodes.Add(node);
            rootNode.AddNodeToModel(node);

            CreateAssetToRootNode(node);
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

            CreateAssetToRootNode(node);
            return node.GetData();
        }

        public (bool, NodeData) CheckForRootNode()
        {
            if (rootNode == null)
            {
                rootNode = (RootNode)nodeGenerator.GenerateNode(typeof(RootNode));
                nodes.Add(rootNode);
                CreateAssetInDataBase(rootNode);
                rootNode.AddNodeToModel(rootNode);
                return (false, rootNode.GetData());
            }

            return (true, rootNode.GetData());
        }

        public void CreateAssetInDataBase(TreeNode node)
        {
            string path = "Assets/";
            path += node.GetType().ToString().Split(".").Last();
            path += ".asset";
            AssetDatabase.CreateAsset(node, path);
            AssetDatabase.SaveAssets();
        }

        public void CreateAssetToRootNode(TreeNode node)
        {
            if (node == rootNode) return;

            AssetDatabase.AddObjectToAsset(node, rootNode);
            AssetDatabase.SaveAssets();
        }
    }
}

