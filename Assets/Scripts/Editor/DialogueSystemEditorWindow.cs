using System;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueSystemEditorWindow : EditorWindow
{
    [SerializeField] private VisualTreeAsset m_VisualTreeAsset = default;

    DialogueSystemEditorGraphView graphView = null;

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

        GetReferences();

    }

    private void GetReferences()
    {
        DialogueSystemEditorGraphView graphView = rootVisualElement.Q<DialogueSystemEditorGraphView>();
        graphView.relatedEditorWin = this;
    }
}
