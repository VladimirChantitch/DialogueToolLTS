using dialogues.node;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Plastic.Antlr3.Runtime.Tree;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class GraphViewChangedHandler
{
    public event EventHandler<NodeParentingArgs> OnNodeParented;
    public event EventHandler<NodeParentingArgs> OnNodeUnParented;
    public event EventHandler<NodeMovedArgs> OnNodeMoved;
    public event EventHandler<NodeData> OnNodeDeleted;

    internal GraphViewChange HandleGraphViewChanged(GraphViewChange graphViewChange)
    {
        if (graphViewChange.elementsToRemove != null)
        {
            HandleRemoveElements(graphViewChange.elementsToRemove);
        }

        if (graphViewChange.edgesToCreate != null)
        {
            HandleCreateEdge(graphViewChange.edgesToCreate);
        }

        if (graphViewChange.movedElements != null)
        {
            HandleMovedElements(graphViewChange.movedElements);
        }

        return graphViewChange;
    }

    private void HandleRemoveElements(List<GraphElement> elementsToRemove)
    {
        List<Edge> edge_remove = new List<Edge>();
        elementsToRemove.ForEach(elem =>
        {
            if (elem != null)
            {
                if ((elem as DialogueNodeView)?.nodeData is not RootData)
                {
                    edge_remove.AddRange(HandleRemoveElement(elem));
                }
            }
        });
        if (edge_remove.Count > 0)
        {
            edge_remove.ForEach(edge =>
            {
                edge.input.DisconnectAll();
                edge.output.DisconnectAll();
                edge.Clear();
            });
        }
    }

    private List<Edge> HandleRemoveElement(GraphElement elem)
    {
        List<Edge> edge_remove = new List<Edge>();

        if (elem is DialogueNodeView dialogueNodeView)
        {
            DialogueNodeView parentView = dialogueNodeView.inPort.node as DialogueNodeView;
            OnNodeUnParented?.Invoke(this, new NodeParentingArgs() { parentNode = parentView.nodeData, childNode = dialogueNodeView.nodeData, outPortIndex = dialogueNodeView.inPort.portTypeInnerClass.portParentIndex });
            edge_remove.AddRange(dialogueNodeView.inPort.connections.ToList());

            dialogueNodeView.outPorts.ForEach(op =>
            {
                DialogueNodeView childView = op.node as DialogueNodeView;
                OnNodeUnParented?.Invoke(this, new NodeParentingArgs() { parentNode = dialogueNodeView.nodeData, childNode = childView.nodeData, outPortIndex = op.portTypeInnerClass.portIndex });
                edge_remove.AddRange(op.connections.ToList());
            });

            OnNodeDeleted?.Invoke(this, dialogueNodeView.nodeData);
        }
        else if (elem is Edge e)
        {
            DialogueNodeView parentView = e.output.node as DialogueNodeView;
            DialogueNodeView childView = e.input.node as DialogueNodeView;

            OnNodeUnParented?.Invoke(this, new NodeParentingArgs() { parentNode = parentView.nodeData, childNode = childView.nodeData, outPortIndex = (e.output as PortView).portTypeInnerClass.portIndex });
        }

        return edge_remove;
    }

    private void HandleCreateEdge(List<Edge> edgesToCreate)
    {
        edgesToCreate.ForEach(e =>
        {
            PortView parentPort = e.output as PortView;
            PortView childPort = e.input as PortView;

            childPort.portTypeInnerClass.portParentIndex = parentPort.portTypeInnerClass.portIndex;

            DialogueNodeView parentView = parentPort.node as DialogueNodeView;
            DialogueNodeView childView = childPort.node as DialogueNodeView;

            OnNodeParented?.Invoke(this, new NodeParentingArgs() { parentNode = parentView.nodeData, childNode = childView.nodeData, outPortIndex = (e.output as PortView).portTypeInnerClass.portIndex});
        });
    }

    private void HandleMovedElements(List<GraphElement> movedElements)
    {
        movedElements.ForEach(me =>
        {
            if (me is DialogueNodeView dialogueNodeView)
            {
                OnNodeMoved?.Invoke(this, new NodeMovedArgs()
                {
                    nodeData = dialogueNodeView.nodeData,
                    newPosition = dialogueNodeView.GetPosition()
                });
            }
        });
    }
}

public class NodeParentingArgs
{
    public NodeData parentNode;
    public NodeData childNode;
    public int outPortIndex = 0;
}

public class NodeMovedArgs
{
    public NodeData nodeData;
    public Rect newPosition;
}
