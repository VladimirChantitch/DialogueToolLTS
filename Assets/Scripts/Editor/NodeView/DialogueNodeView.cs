using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using dialogues.node;
using System;
using UnityEditor;

public class DialogueNodeView : Node
{
    public new class UxmlFactory : UxmlFactory<DialogueNodeView, Node.UxmlTraits>
    {

    }

    VisualElement topContainer = null;
    VisualElement bottomContainer = null;
    VisualElement bodyContainer = null;

    Label nodeType = null;

    public event EventHandler<NodeData> OnNodeSelected;
    public PortView inPort = null;
    public List<PortView> outPorts = new List<PortView>();

    NodeData nodeData;

    public DialogueNodeView() : base("Assets/Templates/NodeTemplates/NodeViewTemplate.uxml")
    {
        UseDefaultStyling();
        GetUI();
    }

    private void GetUI()
    {
        topContainer = this.Q<VisualElement>("Top");
        bottomContainer = this.Q<VisualElement>("bottom");
        bodyContainer = this.Q<VisualElement>("Body");

        nodeType = this.Q<Label>("Name");
    }

    public void Init(NodeData nodeData)
    {
        CreateInputPorts();
        CreateOutputPorts();
        SetUI();
    }

    private void SetUI()
    {
        //Set the UI in the body
    }

    public override void SetPosition(Rect newPos)
    {
        base.SetPosition(newPos);
        Vector2 newPosition = new Vector2(newPos.x, newPos.y);
        nodeData.Position = newPosition;
    }

    public override void OnSelected()
    {
        base.OnSelected();
        OnNodeSelected?.Invoke(this, nodeData);
    }

    private void CreateInputPorts()
    {
        PortType portType = new PortType();
        portType.portIndex = 0;
        portType.PortPrimaryType = PortPrimaryType.InPort;
        switch (nodeData)
        {
            case ConditionalData conditionalData:
                portType.PortSecondaryType = PortSecondaryType.Condition;
                inPort = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Multi, typeof(bool), portType);
                break;
            case DialogueData dialogueData:
                if (dialogueData.DialogueSpeakerType == DialogueSpeakerType.NPC) portType.PortSecondaryType = PortSecondaryType.Npc;
                if (dialogueData.DialogueSpeakerType == DialogueSpeakerType.Player) portType.PortSecondaryType = PortSecondaryType.Player;
                inPort = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Multi, typeof(bool), portType);
                break;
            case EndData endData:
                portType.PortSecondaryType = PortSecondaryType.End;
                inPort = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Multi, typeof(bool), portType);
                break;
            case RootData rootData:
                portType.PortSecondaryType = PortSecondaryType.Root;
                inPort = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Multi, typeof(bool), portType);
                break;
            default:
                break;
        }

        if (inPort != null)
        {
            inPort.portName = "";
            inPort.style.flexDirection = FlexDirection.Column;
            topContainer.Add(inPort);
        }
    }

    private void CreateOutputPorts()
    {
        PortType portType = new PortType();
        portType.portIndex = 0;
        portType.PortPrimaryType = PortPrimaryType.OutPort;
        switch (nodeData)
        {
            case ConditionalData conditionalData:
                portType.PortSecondaryType = PortSecondaryType.Condition;
                outPorts.Add(InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Multi, typeof(bool), portType));
                portType.portIndex = 1;
                portType.PortSecondaryType = PortSecondaryType.Condition;
                outPorts.Add(InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Multi, typeof(bool), portType));
                break;
            case DialogueData dialogueData:
                if (dialogueData.DialogueSpeakerType == DialogueSpeakerType.NPC)
                {
                    portType.PortSecondaryType = PortSecondaryType.Npc;
                    outPorts.Add(InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Multi, typeof(bool), portType));
                }
                else if (dialogueData.DialogueSpeakerType == DialogueSpeakerType.Player)
                {
                    portType.PortSecondaryType = PortSecondaryType.Player;
                    outPorts.Add(InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(bool), portType));
                }

                break;
            case RootData rootData:
                portType.PortSecondaryType = PortSecondaryType.Root;
                outPorts.Add(InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(bool), portType));
                break;
            default:
                break;
        }

        if (outPorts != null)
        {
            outPorts.ForEach(op =>
            {
                op.portName = "";
                op.style.flexDirection = FlexDirection.Column;
                bottomContainer.Add(op);
            });
        }
    }

    public PortView InstantiatePort(Orientation orientation, Direction direction, Port.Capacity capacity, Type type, PortType portType)
    {
        return PortView.Create<Edge>(orientation, direction, capacity, type, portType);
    }
}
