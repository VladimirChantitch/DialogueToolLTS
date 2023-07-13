using dialogues.eventSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace dialogues.data
{
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

        public void InsertEventAtIndex(DialogueEventsBaseClass eventsBaseClass, int index)
        {
            eventContainers.Insert(index, new EventContainer(eventsBaseClass));
        }

        public void RemoveEventAtIndex(int index)
        {
            if (index >= 0 && index < eventContainers.Count)
            {
                eventContainers.RemoveAt(index);
            }
        }

        public void UpdateEventsBasedOnFields(List<DialogueEventsBaseClass> dialogueEventsBaseClasses)
        {
            EventContainers.Clear();
            dialogueEventsBaseClasses.ForEach((de) =>
            {
                EventContainers.Add(new EventContainer(de));
            });
        }
    }
}
