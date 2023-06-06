using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class GraphViewChangedHandler
{
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
            edge_remove.AddRange(HandleRemoveElement(elem));
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
        //if (elem is NodeView nodeView)
        //{
        //    if (!(nodeView.node is RootNode))
        //    {
        //        nodeView.output.ForEach(op =>
        //        {
        //            edge_remove = op.connections.ToList();
        //        });

        //        tree.DeleteNode(nodeView.node);
        //    }
        //}

        //if (elem is Edge edge)
        //{
        //    edge.layer = 10;
        //    NodeView parentView = edge.output.node as NodeView;
        //    NodeView childView = edge.input.node as NodeView;
        //    if (parentView.node is ConditionalNode)
        //    {
        //        if (edge.output.name == "outTrue")
        //        {
        //            tree.RemoveChild(parentView.node, childView.node, true);
        //        }
        //        else
        //        {
        //            tree.RemoveChild(parentView.node, childView.node, false);
        //        }
        //    }
        //    else
        //    {
        //        tree.RemoveChild(parentView.node, childView.node);
        //    }
        //}
        return edge_remove;
    }

    private void HandleCreateEdge(List<Edge> edgesToCreate)
    {
        throw new NotImplementedException();
    }

    private void HandleMovedElements(List<GraphElement> movedElements)
    {

    }
}
