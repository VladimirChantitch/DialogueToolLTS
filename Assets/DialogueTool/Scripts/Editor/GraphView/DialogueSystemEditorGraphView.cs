
using dialogues.editor;
using dialogues.editor.treeHandler;
using dialogues.node;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Plastic.Antlr3.Runtime.Tree;
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

    public event EventHandler<NodeData> OnNodeSelected;

    public DialogueSystemEditorWindow relatedEditorWin;
    StyleSheet ss = null;
    TreeNode currentRoot = null;
    ITreeHandler treeHandlerService = null;

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
        graphViewChangedHandler.OnNodeParented += (handler, args) => OnNodeParented(args);
        graphViewChangedHandler.OnNodeDeleted += (handler, args) => OnNodeDeleted(args);
        graphViewChangedHandler.OnNodeUnParented += (handler, args) => OnNodeUnParented(args);
        graphViewChangedHandler.OnNodeMoved += (handler, args) => OnNodeMoved(args);
    }

    public void Init(StyleSheet styleSheet, DialogueSystemEditorWindow relatedEditorWin, ITreeHandler treeHandlerService)
    {
        this.ss = styleSheet;
        styleSheets.Add(ss);
        this.relatedEditorWin = relatedEditorWin;
        this.treeHandlerService = treeHandlerService;
        PopulateView();
    }

    public void PopulateView()
    {
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
        AssetDatabase.SaveAssets();
    }

    private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
    {
        return graphViewChangedHandler.HandleGraphViewChanged(graphViewChange);
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter adapter)
    {
        List<Port> selectedPorts = new List<Port>();
        if ((startPort as PortView).portTypeInnerClass.PortSecondaryType == PortSecondaryType.Player)
        {
            selectedPorts.AddRange(ports.Where(p => (p as PortView).portTypeInnerClass.PortSecondaryType != PortSecondaryType.Player));
            return selectedPorts;
        }
        else if ((startPort as PortView).portTypeInnerClass.PortSecondaryType == PortSecondaryType.Npc)
        {
            selectedPorts.AddRange(ports.Where(p => (p as PortView).portTypeInnerClass.PortSecondaryType != PortSecondaryType.Npc));
            return selectedPorts; ;
        }
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
        //base.BuildContextualMenu(evt);

        var position = viewTransform.matrix.inverse.MultiplyPoint(evt.localMousePosition);
        Rect mousePose = new Rect { x = position.x, y = position.y };
        {
            var types = TypeCache.GetTypesDerivedFrom<TreeNode>();
            foreach (var type in types)
            {
                evt.menu.AppendAction($"[{type.BaseType.Name}]{type.Name}", (a) => CreateNode(type, mousePose));
            }
        }
    }

    void CreateNode(System.Type type, Rect mousePose)
    {
        (bool,NodeData) nodeCheck = treeHandlerService.CheckForRootNode();
        if (nodeCheck.Item1 == false)
        {
            CreateNodeView(nodeCheck.Item2, mousePose);
        }

        NodeData node = treeHandlerService.CreateNode(type);
        CreateNodeView(node, mousePose);
    }

    void CreateNodeView(NodeData nodeData, Rect mousePose)
    {
        if (nodeData == null) return;

        DialogueNodeView nodeView = new DialogueNodeView();

        nodeView.OnNodeSelected += OnNodeSelected;
        nodeView.Init(nodeData);
        nodeView.SetPosition(mousePose);
        nodeData.SetPosition(mousePose);
        AddElement(nodeView);
    }

    DialogueNodeView CreateNodeView(TreeNode treeNode)
    {
        if (treeNode == null) return new();

        DialogueNodeView nodeView = new DialogueNodeView();

        nodeView.OnNodeSelected += OnNodeSelected;
        nodeView.Init(treeNode.GetData());
        nodeView.SetPosition(new Rect() { x = treeNode.position.x, y = treeNode.position.y });
        AddElement(nodeView);
        return nodeView;
    }

    private void OnNodeParented(NodeParentingArgs args)
    {
        treeHandlerService.CreateOrUpdateChild(args.parentNode, args.childNode);
    }

    private void OnNodeDeleted(NodeData nodeData)
    {
        treeHandlerService.DeleteNodeFromData(nodeData);
    }

    private void OnNodeUnParented(NodeParentingArgs args)
    {
        treeHandlerService.RemoveChild(args.parentNode, args.childNode);
    }

    private void OnNodeMoved(NodeMovedArgs args)
    {
        args.nodeData.Position = new Vector2(args.newPosition.x, args.newPosition.y);
        treeHandlerService.UpdateNode(args.nodeData);
    }

    internal void LoadNodeView(List<TreeNode> nodeModel)
    {
        List<NodeData> nodeDataModel = new List<NodeData>();
        List<DialogueNodeView> dialogueNodeViews = new List<DialogueNodeView>();

        nodeModel.ForEach(n =>
        {
            dialogueNodeViews.Add(CreateNodeView(n));
        });

        nodeModel.ForEach(n =>
        {
            DialogueNodeView dialogueNodeView = dialogueNodeViews.Find(dnv => dnv.nodeData.Guid == n.guid);

            switch (n)
            {
                case ConditionNode conditionNode:
                    if (conditionNode.DirectChildren.Count > 0)
                    {
                        Edge trueEdge = dialogueNodeView.outPorts[0].ConnectTo(dialogueNodeViews.Find(dnv => dnv.nodeData.Guid == conditionNode.DirectChildren[0].guid).inPort);
                        AddElement(trueEdge);
                    }
                    if (conditionNode.DirectChildren.Count > 1)
                    {
                        Edge falseEdge = dialogueNodeView.outPorts[1].ConnectTo(dialogueNodeViews.Find(dnv => dnv.nodeData.Guid == conditionNode.DirectChildren[1].guid).inPort);
                        AddElement(falseEdge);
                    }

                    break;
                default:
                    n.DirectChildren.ForEach(dc =>
                    {
                        Edge edge = dialogueNodeView.outPorts[0].ConnectTo(dialogueNodeViews.Find(dnv => dnv.nodeData.Guid == dc.guid).inPort);
                        AddElement(edge);
                    });
 
                break;
            }
        });
    }
}
