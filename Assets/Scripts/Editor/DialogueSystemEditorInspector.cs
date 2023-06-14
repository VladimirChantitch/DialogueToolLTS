using dialogues.node;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueSystemEditorInspector : VisualElement
{
    public new class UxmlFactory : UxmlFactory<DialogueSystemEditorInspector, VisualElement.UxmlTraits>
    {

    }

    public void ChangeInspectorBinding(NodeData nodeData)
    {
        Debug.Log("TODO ::: Show inspector");
    }
}
