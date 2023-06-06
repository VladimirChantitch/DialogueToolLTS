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

    public void Init(NodeData nodeData)
    {

    }

    public event EventHandler<NodeData> OnNodeSelected;
    public Port inPort = null;
    public List<Port> outPorts = new List<Port>();

    NodeData nodeData;

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
}
