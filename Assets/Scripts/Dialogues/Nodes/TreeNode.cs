using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Utils;

namespace dialogues.node
{
    public abstract class TreeNode : ScriptableObject
    {
        [HideInInspector] public string guid;
        [HideInInspector] public Vector2 position;

        [SerializeField] protected List<TreeNode> directChildren = new List<TreeNode> ();
        public List<TreeNode> DirectChildren => directChildren;
        [SerializeField] private List<TreeNode> directParents = new List<TreeNode> ();
        public List<TreeNode> DirectParents => directParents;   

        [SerializeField] List<EventContainer> eventContainers = new List<EventContainer>();

        public virtual TreeNode Clone()
        {
            return Instantiate(this);
        }

        public virtual NodeData GetData()
        {
            NodeData nodeData = new NodeData()
            {
                Position = position,
                Guid = guid,
                EventContainers = eventContainers          
            };
            return nodeData;
        }

        public virtual void SetUpData(NodeData nodeData)
        {
            position = nodeData.Position;
            guid = nodeData.Guid;
            eventContainers.AddRange(nodeData.EventContainers);     
        }

        public virtual bool AddChild(TreeNode newChild)
        {
            if (!directChildren.Contains(newChild))
            {
                directChildren.Add(newChild);
                newChild.AddParent(this);
                return true;
            }
            return false;
        }

        public void RemoveChild(TreeNode removedChild)
        {
            if (directChildren.Contains(removedChild))
            {
                directChildren.Remove(removedChild);
                removedChild.RemoveParent(this);
            }
        }

        public virtual bool AddParent(TreeNode newParent)
        {
            if (!directParents.Contains(newParent))
            {
                directParents.Add(newParent);
                newParent.AddChild(this);
                return true;
            }
            return false;
        }

        public void RemoveParent(TreeNode removedParent)
        {
            if (directParents.Contains(removedParent))
            {
                directParents.Remove(removedParent);
                removedParent.RemoveChild(this);
            }
        }

        public virtual List<TreeNode> GetNextNodes()
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

        public virtual List<TreeNode> GetChildrenNodeModel()
        {
            List<TreeNode> children = new List<TreeNode>();
            DirectChildren.ForEach(dc =>
            {
                children.AddRange(dc.GetChildrenNodeModel());
            });
            return children;
        }

        public List<NodeData> GetChildrenData()
        {
            List<NodeData> datas = new List<NodeData>();
            DirectChildren.ForEach(dc =>
            {
                datas.Add(dc.GetData());
            });
            return datas;
        }
    }

    [Serializable]
    public class NodeData
    {
        public NodeData() { }

        public NodeData(NodeData nodeData)
        {
            this.position = nodeData.Position;
            this.guid = nodeData.Guid;
            this.eventContainers = nodeData.EventContainers;
        }

        public virtual NodeData Clone(NodeData nodeData)
        {
            NodeData node = new NodeData();
            node.position = nodeData.Position;
            node.guid = nodeData.Guid;
            node.eventContainers = nodeData.EventContainers;

            return node;
        }

        [SerializeField] protected Vector2 position;
        [SerializeField] protected string guid;
        [SerializeField] protected List<EventContainer> eventContainers = new List<EventContainer>();

        public Vector2 Position { get => position; set => position = value; }
        public string Guid { get => guid; set => guid = value; }
        public List<EventContainer> EventContainers { get => eventContainers; set => eventContainers = value; }

        public void SetPosition(Rect position)
        {
            Vector2 updatedPosition = new Vector2(position.x, position.y);
            Position = updatedPosition;
        }
    }
}

