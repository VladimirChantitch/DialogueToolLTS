using dialogues.editor;
using System;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueSystemEditorWindow : EditorWindow
{
    [SerializeField] private VisualTreeAsset m_VisualTreeAsset = default;

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
        m_VisualTreeAsset.CloneTree(rootVisualElement);

        styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Templates/DialogueSystemEditorWindow.uss");
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
