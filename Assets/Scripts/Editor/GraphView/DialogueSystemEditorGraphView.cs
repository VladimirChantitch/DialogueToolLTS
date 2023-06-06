
using dialogues.node;
using System;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Experimental.GraphView.GraphView;

public class DialogueSystemEditorGraphView : GraphView
{
    public new class UxmlFactory : UxmlFactory<DialogueSystemEditorGraphView, GraphView.UxmlTraits>
    {

    }

    public event Action OnNodeSelected;
    public DialogueSystemEditorWindow relatedEditorWin;
    StyleSheet ss = null;
    TreeNode currentRoot = null;

    private Vector2 localMousePosition;

    GraphViewChangedHandler graphViewChangedHandler;

    public DialogueSystemEditorGraphView()
    {
        Insert(0, new GridBackground());

        this.AddManipulator(new ContentZoomer());
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());
        
        RegisterCallback<PointerMoveEvent>(OnPointerMoveEvent, TrickleDown.TrickleDown);
        Undo.undoRedoPerformed += OnUndoRedo;
        graphViewChangedHandler = new GraphViewChangedHandler();
    }

    public void Init(StyleSheet styleSheet, DialogueSystemEditorWindow relatedEditorWin)
    {
        this.ss = styleSheet;
        styleSheets.Add(ss);
        this.relatedEditorWin = relatedEditorWin;
    }

    public void PopulateView(TreeNode currentRoot)
    {
        this.currentRoot = currentRoot;
        graphViewChanged -= OnGraphViewChanged;
        DeleteElements(graphElements); 
        graphViewChanged += OnGraphViewChanged;

        //EditorUtility.SetDirty(currentRoot);
        //AssetDatabase.SaveAssets();
    }

    private void OnPointerMoveEvent(PointerMoveEvent evt)
    {
        localMousePosition = evt.localPosition;
    }

    private void OnUndoRedo()
    {
        PopulateView(currentRoot);
        AssetDatabase.SaveAssets();
    }

    private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
    {
        return graphViewChangedHandler.HandleGraphViewChanged(graphViewChange);
    }

    
}
