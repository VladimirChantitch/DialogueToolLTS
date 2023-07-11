using dialogues.node;
using System;
using System.Collections.Generic;

namespace dialogues.editor.treeHandler
{
    public interface ITreeHandler
    {
        event EventHandler<TreeNode> OnChildAdded;
        event EventHandler<TreeNode> OnChildRemoved;
        event EventHandler<List<TreeNode>> OnNodeModelLoaded;

        void AddAssetToRootNode(TreeNode node);
        bool AddOrUpdateChild(NodeData parent, NodeData child);
        (bool, NodeData) CheckForRootNode();
        void CreateAssetInDataBase(TreeNode node);
        NodeData CreateNode(Type type);
        NodeData CreateNodeCopyFromData(NodeData data);
        TreeNode CreateNodeFromData(NodeData data);
        bool DeleteNodeFromData(NodeData data);
        List<NodeData> GetChildren(NodeData parent);
        bool RemoveChild(NodeData parent, NodeData child);
        void UpdateNode(NodeData data);
        void UseAnotherRoot(RootNode newRoot);
    }
}