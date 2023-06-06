using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Experimental.GraphView.Port;

public class PortView : Port
{
    protected PortView(Orientation portOrientation, Direction portDirection, Capacity portCapacity, Type type, PortType portType) : base(portOrientation, portDirection, portCapacity, type)
    {
        this.portType = portType;
    }

    public PortType portType;

    public static PortView Create<TEdge>(Orientation orientation, Direction direction, Capacity capacity, Type type, PortType portType) where TEdge : Edge, new()
    {
        EdgeConnectorListener listener = new EdgeConnectorListener();
        PortView port = new PortView(orientation, direction, capacity, type, portType)
        {
            m_EdgeConnector = new EdgeConnector<TEdge>(listener)
        };
        port.AddManipulator(port.m_EdgeConnector);
        return port;
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
