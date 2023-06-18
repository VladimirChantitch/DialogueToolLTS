using dialogues.eventSystem;
using dialogues.node;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UI;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueSystemEditorInspector : VisualElement
{
    public new class UxmlFactory : UxmlFactory<DialogueSystemEditorInspector, VisualElement.UxmlTraits>
    {

    }

    VisualElement firstChild;
    StyleSheet styleSheet;
    ScrollView eventContainer;
    VisualTreeAsset listTree;

    NodeData currentNodeData = null;

    public event EventHandler<NodeData> OnNodeDataChanged;

    private int eventIndex = -1;

    public DialogueSystemEditorInspector()
    {
        this.styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(DialogueSystemEditorWindow.nodeViewStylePath);
        this.listTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(DialogueSystemEditorWindow.listTemplate);
        styleSheets.Add(styleSheet);
    }

    public void ChangeInspectorBinding(NodeData nodeData)
    {
        eventIndex = -1;
        currentNodeData = nodeData;
        Clear();
        switch (nodeData)
        {
            case EndData endData:
                ShowEndInspector(endData);
                break;

            case ConditionalData conditionalData:
                ShowConditionInspector(conditionalData);
                break;

            case RootData rootData:
                ShowRootInspector(rootData);
                break;

            case DialogueData dialogueData:
                ShowDialogueInspector(dialogueData);
                break;
        }
    }

    List<ObjectField> oFields = new List<ObjectField>();
    private void ShowBasicInspector(string nodeName)
    {
        AddToClassList("Container");
        Label name = new Label();
        name.text = nodeName;
        name.AddToClassList("subContainer");
        Add(name);

        SetUpEventList();
    }

    private void ShowEndInspector(EndData endData)
    {
        ShowBasicInspector("End Node");
    }

    private void ShowConditionInspector(ConditionalData conditionalData)
    {
        ShowBasicInspector("Condition Node");
    }

    private void ShowRootInspector(RootData conditionalData)
    {
        ShowBasicInspector("Root Node");
    }

    private void ShowDialogueInspector(DialogueData conditionalData)
    {
        ShowBasicInspector("Dialogue Node");
    }

    private void SetUpEventList()
    {
        VisualElement ve = new VisualElement();
        listTree.CloneTree(ve);
        eventContainer = ve.Q<ScrollView>("Container");
        Button add_btn = ve.Q<Button>("Add");
        Button rem_btn = ve.Q<Button>("Remove");
        Label containerName = ve.Q<Label>("ContainerName");
        containerName.text = "Events";
        containerName.AddToClassList("subContainer");

        Add(ve);

        add_btn.clicked += () =>
        {
            eventIndex++;
            ObjectField of = new ObjectField();
            of.objectType = typeof(DialogueEventsBaseClass);
            of.RegisterValueChangedCallback((data) =>
            {
                currentNodeData.InsertEventAtIndex(data.newValue as DialogueEventsBaseClass, eventIndex);
            });
            oFields.Add(of);
            eventContainer.Add(of);
        };

        rem_btn.clicked += () =>
        {
            if (eventIndex >= 0)
            {
                Debug.Log(eventIndex);
                currentNodeData.RemoveEventAtIndex(eventIndex);
                ObjectField oField = oFields[eventIndex];
                oFields.Remove(oField);
                oField.RemoveFromHierarchy();
            }

            eventIndex--;
            if (eventIndex < 0)
            {
                eventIndex = -1;
            }
        };
    }
}

