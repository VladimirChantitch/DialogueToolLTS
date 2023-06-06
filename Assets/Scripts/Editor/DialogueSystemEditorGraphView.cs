using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueSystemEditorGraphView : GraphView
{
    public new class UxmlFactory : UxmlFactory<DialogueSystemEditorGraphView, GraphView.UxmlTraits>
    {

    }

    public event Action OnNodeSelected;
    public DialogueSystemEditorWindow relatedEditorWin;


    private Vector2 localMousePosition;

    public DialogueSystemEditorGraphView()
    {
        Insert(0, new GridBackground());

        this.AddManipulator(new ContentZoomer());
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        //var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(DialoguesSystemUtils.DIALOGUE_EDITOR_STYLE);
        //styleSheets.Add(styleSheet);

        this.RegisterCallback<PointerMoveEvent>(OnPointerMoveEvent, TrickleDown.TrickleDown);
    }

    private void OnPointerMoveEvent(PointerMoveEvent evt)
    {
        localMousePosition = evt.localPosition;
    }
}
