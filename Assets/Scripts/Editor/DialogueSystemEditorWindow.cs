using dialogues.editor;
using dialogues.node;
using System;
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

    DialogueSystemEditorGraphView graphView = null;
    StyleSheet styleSheet = null;
    TreeHandler treeHandler = null;

    static RootNode currentRootNode = null;
    public static string nodeViewStylePath = "Assets/Templates/NodeTemplates/nodeviewStyle.uss";
    public static string nodeViewTemplate = "Assets/Templates/NodeTemplates/NodeViewTemplate.uxml";

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
    }

    private void InstantiateServices()
    {
        if (currentRootNode != null)
        {
            treeHandler = new TreeHandler(currentRootNode);
        }
        else
        {
            treeHandler = new TreeHandler();
        }

    }

    private void GetReferences()
    {
        graphView = rootVisualElement.Q<DialogueSystemEditorGraphView>();
        graphView.Init(styleSheet, this, treeHandler);
    }

    private void SetReferences()
    {
        graphView.OnNodeSelected += (sender, data) => OnNodeSelectionChanged(data);
    }

    private void OnSelectionChange()
    {
        if (Selection.activeContext is RootNode rootNode)
        {
            ReadAsset(rootNode);
        }
    }

    private void ReadAsset(RootNode rootNode)
    {
        if (AssetDatabase.CanOpenAssetInEditor(rootNode.GetInstanceID()))
        {
            treeHandler?.UseAnotherRoot(rootNode);
        }
    }

    void OnNodeSelectionChanged(NodeData nodeData)
    {

    }
}
