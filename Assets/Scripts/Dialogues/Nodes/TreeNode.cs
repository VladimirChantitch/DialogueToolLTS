using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Utils;

namespace dialogues
{
    public abstract class TreeNode : ScriptableObject
    {
        [HideInInspector] public bool started = false;
        [HideInInspector] public string guid;
        [HideInInspector] public Vector2 position;

        [SerializeField] private List<TreeNode> directChildren = new List<TreeNode> ();
        public List<TreeNode> DirectChildren => directChildren;
        [SerializeField] private List<TreeNode> directParents = new List<TreeNode> ();
        public List<TreeNode> DirectParents => directParents;   

        [SerializeField] List<EventContainer> eventContainers = new List<EventContainer>();

        public event EventHandler OnNodeDelete;

        public virtual TreeNode Clone()
        {
            return Instantiate(this);
        }

        public virtual NodeData GetData()
        {
            NodeData nodeData = new NodeData()
            {
                Position = position,
                OldGuid = guid
            };
            return nodeData;
        }

        public virtual void SetUpData(NodeData nodeData)
        {
            position = nodeData.Position;
        }

        public void AddChild(TreeNode newChild)
        {
            if (!directChildren.Contains(newChild))
            {
                directChildren.Add(newChild);
            }
        }

        public void RemoveChild(TreeNode removedChild)
        {
            directChildren.Remove(removedChild);
        }

        public void AddParent(TreeNode newParent)
        {
            if (!directParents.Contains(newParent))
            {
                directParents.Add(newParent);
            }
        }

        public void RemoveParent(TreeNode removedParent)
        {
            directParents.Remove(removedParent);
        }

        public void Delete()
        {
            OnNodeDelete?.Invoke(this, EventArgs.Empty);
        }

        public virtual List<TreeNode> GetNextNode()
        {
            PlayAllEvents();
            return directChildren;
        }

        public virtual void PlayAllEvents()
        {
            for (int i = 0; i < eventContainers.Count; i++)
            {
                eventContainers[i].ActivateSelectedMethod();
            }
        }
    }

    [Serializable]
    public class NodeData
    {
        public NodeData() { }
        public NodeData(NodeData nodeData)
        {
            this.position = nodeData.Position;
            this.oldGuid = nodeData.OldGuid;
        }

        [SerializeField] Vector2 position;
        [SerializeField] string oldGuid;

        public Vector2 Position { get => position; set => position = value; }
        public string OldGuid  {get => oldGuid; set => oldGuid = value; }
    }
}

