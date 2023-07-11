using dialogues.editor;
using dialogues.editor.treeHandler;
using dialogues.node;
using System;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueSystemEditorWindow : EditorWindow
{
    [SerializeField] private VisualTreeAsset m_editorWindow = default;
    [SerializeField] private VisualTreeAsset m_nodeView = default;
    [SerializeField] private StyleSheet ss = default;
    [SerializeField] private StyleSheet nodeViewStyle = default;
    [SerializeField] private VisualTreeAsset NodeViewTemplate = default;
    [SerializeField] private VisualTreeAsset E_CTemplate = default;

    DialogueSystemEditorGraphView graphView = null;
    DialogueSystemEditorInspector inspector = null;
    StyleSheet styleSheet = null;
    ITreeHandler treeHandlerService = null;

    static RootNode currentRootNode = null;
    public static string nodeViewStylePath = "Assets/DialogueTool/Templates/NodeTemplates/nodeviewStyle.uss";
    public static string nodeViewTemplate = "Assets/DialogueTool/Templates/NodeTemplates/NodeViewTemplate.uxml";

    [MenuItem("Tool/DialogueTool")]
    public static void DisplayWindow()
    {
        DialogueSystemEditorWindow wnd = GetWindow<DialogueSystemEditorWindow>();
        wnd.titleContent = new GUIContent("DialogueSystem");
    }

    [OnOpenAsset]
    public static bool OnOpenAsset(int instanceID, int lineNumber)
    {
        if (Selection.activeObject is RootNode rootNode)
        {
            currentRootNode = rootNode;
            DisplayWindow();
            return true;
        }
        return false;
    }

    public void CreateGUI()
    {
        VisualElement root = rootVisualElement;
        m_editorWindow.CloneTree(rootVisualElement);    

        styleSheet = ss;
        InstantiateServices();
        GetReferences();
        SetReferences();
        if (currentRootNode != null)
        {
            LoadNodeView(currentRootNode.nodesModel);
        }
    }

    private void InstantiateServices()
    {
        if (currentRootNode != null)
        {
            treeHandlerService = new TreeHandler(currentRootNode);
        }
        else
        {
            treeHandlerService = new TreeHandler();
        }
    }

    private void GetReferences()
    {
        graphView = rootVisualElement.Q<DialogueSystemEditorGraphView>();
        graphView.Init(styleSheet, this, treeHandlerService);

        inspector = rootVisualElement.Q<DialogueSystemEditorInspector>();
    }

    private void SetReferences()
    {
        graphView.OnNodeSelected += (sender, data) => OnNodeSelectionChanged(data);
        inspector.OnNodeDataChanged += (sender, data) => OnNodeDataChanged(data);
        treeHandlerService.OnNodeModelLoaded += (sender, data) => LoadNodeView(data);

        inspector.Init(nodeViewStyle, E_CTemplate);
    }

    private void OnSelectionChange()
    {
        if (Selection.activeObject is RootNode rootNode)
        {
            ReadAsset(rootNode);
        }
    }

    private void ReadAsset(RootNode rootNode)
    {
        treeHandlerService?.UseAnotherRoot(rootNode);
    }

    void OnNodeSelectionChanged(NodeData nodeData)
    {
        inspector.ChangeInspectorBinding(nodeData);
    }

    void OnNodeDataChanged(NodeData nodeData)
    {
        treeHandlerService?.UpdateNode(nodeData);
    }

    private void LoadNodeView(List<TreeNode> nodeModel)
    {
        graphView.LoadNodeView(nodeModel);
    }
}
