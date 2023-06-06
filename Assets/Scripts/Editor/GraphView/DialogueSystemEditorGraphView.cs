
using dialogues.editor;
using dialogues.node;
using System;
using System.Collections.Generic;
using System.Linq;
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
    TreeHandler treeHandler = null;

    private Vector2 localMousePosition;

    GraphViewChangedHandler graphViewChangedHandler;

    //List<NodeView>

    //private NodeView FindNodeView(dialogues.Node node)
    //{
    //    return GetNodeByGuid(node.guid) as NodeView;
    //}

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

    public void Init(StyleSheet styleSheet, DialogueSystemEditorWindow relatedEditorWin, TreeHandler treeHandler)
    {
        this.ss = styleSheet;
        styleSheets.Add(ss);
        this.relatedEditorWin = relatedEditorWin;
        this.treeHandler = treeHandler;
    }

    public void PopulateView(TreeNode currentRoot)
    {
        this.currentRoot = currentRoot;
        graphViewChanged -= OnGraphViewChanged;
        DeleteElements(graphElements); 
        graphViewChanged += OnGraphViewChanged;


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

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter adapter)
    {
        return ports.ToList();
    }

    private void ResetSelection()
    {
        ClearSelection();
        //List<NodeView> nodeViews = new List<NodeView>();
        //List<VisualElement> elems = this.Q<VisualElement>("contentViewContainer").Children().Last().Children().ToList();

        //elems?.ForEach(elem =>
        //{
        //    if (elem is NodeView nodeView)
        //    {
        //        nodeViews.Add(nodeView);
        //    }
        //});

        //for (int i = 0; i < nodeViews.Count; i++)
        //{
        //    newNodeViews.ForEach(nnv =>
        //    {
        //        if (nnv.node == nodeViews[i].node)
        //        {
        //            AddToSelection(nodeViews[i]);
        //        }
        //    });
        //}
    }

    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        base.BuildContextualMenu(evt);
    }
}
