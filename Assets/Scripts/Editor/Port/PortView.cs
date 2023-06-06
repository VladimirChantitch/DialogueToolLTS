using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PortView : Port
{
    protected PortView(Orientation portOrientation, Direction portDirection, Capacity portCapacity, Type type) : base(portOrientation, portDirection, portCapacity, type)
    {

    }

    public void Init(PortType portType)
    {
        this.portType = portType;
    }

    public PortType portType;
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
