using dialogues.editor;
using System;
using UnityEditor;
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

    [MenuItem("Tool/DialogueTool")]
    public static void DisplayWindow()
    {
        DialogueSystemEditorWindow wnd = GetWindow<DialogueSystemEditorWindow>();
        wnd.titleContent = new GUIContent("DialogueSystem");
    }

    public void CreateGUI()
    {
        VisualElement root = rootVisualElement;
        m_editorWindow.CloneTree(rootVisualElement);

        styleSheet = ss;
        InstantiateServices();
        GetReferences();
    }

    private void InstantiateServices()
    {
        treeHandler = new TreeHandler();
    }

    private void GetReferences()
    {
        DialogueSystemEditorGraphView graphView = rootVisualElement.Q<DialogueSystemEditorGraphView>("DialogueGraphView");
        graphView.Init(styleSheet, this, treeHandler);
    }
}
