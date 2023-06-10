using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Experimental.GraphView.Port;

public class PortView : Port
{
    protected PortView(Orientation portOrientation, Direction portDirection, Capacity portCapacity, Type type, PortType portType) : base(portOrientation, portDirection, portCapacity, type)
    {
        this.portTypeInnerClass = portType;
    }

    public PortType portTypeInnerClass;
    public event EventHandler OnConnect;
    public event EventHandler OnDisconnect;

    public static PortView Create<TEdge>(Orientation orientation, Direction direction, Capacity capacity, Type type, PortType portType) where TEdge : Edge, new()
    {
        DefaultEdgeConnectorListener listener = new DefaultEdgeConnectorListener();
        PortView port = new PortView(orientation, direction, capacity, type, portType)
        {
            m_EdgeConnector = new EdgeConnector<TEdge>(listener)
        };
        port.AddManipulator(port.m_EdgeConnector);
        return port;
    }

    public override void Connect(Edge edge)
    {
        base.Connect(edge);
        OnConnect?.Invoke(this, null);
    }

    public override void Disconnect(Edge edge)
    {
        base.Disconnect(edge);
        OnDisconnect?.Invoke(this, null);
    }
    


    private class DefaultEdgeConnectorListener : IEdgeConnectorListener
    {
        private GraphViewChange m_GraphViewChange;
        private List<Edge> m_EdgesToCreate;
        private List<GraphElement> m_EdgesToDelete;

        public DefaultEdgeConnectorListener()
        {
            this.m_EdgesToCreate = new List<Edge>();
            this.m_EdgesToDelete = new List<GraphElement>();
            this.m_GraphViewChange.edgesToCreate = this.m_EdgesToCreate;
        }

        public void OnDropOutsidePort(Edge edge, Vector2 position)
        {
        }

        public void OnDrop(UnityEditor.Experimental.GraphView.GraphView graphView, Edge edge)
        {
            this.m_EdgesToCreate.Clear();
            this.m_EdgesToCreate.Add(edge);
            this.m_EdgesToDelete.Clear();
            if (edge.input.capacity == Port.Capacity.Single)
            {
                foreach (Edge connection in edge.input.connections)
                {
                    if (connection != edge)
                        this.m_EdgesToDelete.Add((GraphElement)connection);
                }
            }
            if (edge.output.capacity == Port.Capacity.Single)
            {
                foreach (Edge connection in edge.output.connections)
                {
                    if (connection != edge)
                        this.m_EdgesToDelete.Add((GraphElement)connection);
                }
            }
            if (this.m_EdgesToDelete.Count > 0)
                graphView.DeleteElements((IEnumerable<GraphElement>)this.m_EdgesToDelete);
            List<Edge> edgesToCreate = this.m_EdgesToCreate;
            if (graphView.graphViewChanged != null)
                edgesToCreate = graphView.graphViewChanged(this.m_GraphViewChange).edgesToCreate;
            foreach (Edge edge1 in edgesToCreate)
            {
                graphView.AddElement((GraphElement)edge1);
                edge.input.Connect(edge1);
                edge.output.Connect(edge1);
            }
        }
    }
}

public class PortType
{
    public PortPrimaryType PortPrimaryType { get; set; }
    public PortSecondaryType PortSecondaryType { get; set; }
    public int portIndex;
}

public enum PortPrimaryType
{
    InPort, OutPort
}

public enum PortSecondaryType
{
    Player, Npc, Root, End, Condition
}
