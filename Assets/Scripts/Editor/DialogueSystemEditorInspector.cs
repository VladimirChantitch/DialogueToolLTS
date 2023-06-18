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
    ScrollView conditionContainer;
    VisualTreeAsset listTree;

    NodeData currentNodeData = null;

    public event EventHandler<NodeData> OnNodeDataChanged;

    private int eventIndex = -1;
    private int conditionIndex = -1;

    public DialogueSystemEditorInspector()
    {
        this.styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(DialogueSystemEditorWindow.nodeViewStylePath);
        this.listTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(DialogueSystemEditorWindow.listTemplate);
        styleSheets.Add(styleSheet);
    }

    public void ChangeInspectorBinding(NodeData nodeData)
    {
        eventIndex = -1;
        conditionIndex = -1;
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
        SetUpConditionList(conditionalData);
    }

    private void ShowRootInspector(RootData rootData)
    {
        ShowBasicInspector("Root Node");
    }

    private void ShowDialogueInspector(DialogueData dialogueData)
    {
        ShowBasicInspector("Dialogue Node");

        TextField tf_name = new TextField();
        tf_name.AddToClassList("subContainer");
        tf_name.label = "speakerName";
        Add(tf_name);
        TextField tf_dialogue = new TextField();
        tf_dialogue.AddToClassList("subContainer");
        tf_dialogue.label = "dialogue";
        tf_dialogue.style.flexDirection = FlexDirection.Column;
        tf_dialogue.multiline = true;
        tf_dialogue.style.flexShrink = 1;
        tf_dialogue.style.overflow = Overflow.Hidden;
        tf_dialogue.style.textOverflow = TextOverflow.Ellipsis;

        tf_dialogue.Q<VisualElement>("unity-text-input").style.whiteSpace = WhiteSpace.Normal;

        tf_dialogue.style.height = 200;
        Add(tf_dialogue);

        ObjectField of_spriteIcone = new ObjectField();
        of_spriteIcone.AddToClassList("subContainer");
        of_spriteIcone.label = "Speaker Sprite";
        of_spriteIcone.objectType = typeof(Sprite);
        Add(of_spriteIcone);

        tf_name.RegisterValueChangedCallback((data) =>
        {
            dialogueData.SpeakerName = data.newValue;
            OnNodeDataChanged?.Invoke(this, currentNodeData);
        });

        tf_dialogue.RegisterValueChangedCallback((data) =>
        {
            dialogueData.Dialogue = data.newValue;
            OnNodeDataChanged?.Invoke(this, currentNodeData);
        });

        of_spriteIcone.RegisterValueChangedCallback((data) =>
        {
            dialogueData.SpeakerIcone = data.newValue as Sprite;
            OnNodeDataChanged?.Invoke(this, currentNodeData);
        });
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
                OnNodeDataChanged?.Invoke(this,currentNodeData);
            });
            of.label = "Event Object";
            oFields.Add(of);
            eventContainer.Add(of);
        };

        rem_btn.clicked += () =>
        {
            if (eventIndex >= 0)
            {
                currentNodeData.RemoveEventAtIndex(eventIndex);
                ObjectField oField = oFields[eventIndex];
                oFields.Remove(oField);
                oField.RemoveFromHierarchy();
                OnNodeDataChanged?.Invoke(this, currentNodeData);
            }

            eventIndex--;
            if (eventIndex < 0)
            {
                eventIndex = -1;
            }
        };
    }

    private void SetUpConditionList(ConditionalData conditionalData)
    {
        VisualElement ve = new VisualElement();
        listTree.CloneTree(ve);
        conditionContainer = ve.Q<ScrollView>("Container");
        Button add_btn = ve.Q<Button>("Add");
        Button rem_btn = ve.Q<Button>("Remove");
        Label containerName = ve.Q<Label>("ContainerName");
        containerName.text = "Conditions";
        containerName.AddToClassList("subContainer");

        Add(ve);

        add_btn.clicked += () =>
        {
            conditionIndex++;
            ObjectField of = new ObjectField();
            of.objectType = typeof(DialogueConditionsBaseClass);
            of.RegisterValueChangedCallback((data) =>
            {
                conditionalData.InsertConditionAtIndex(data.newValue as DialogueConditionsBaseClass, conditionIndex);
                OnNodeDataChanged?.Invoke(this, currentNodeData);
            });
            of.label = "Condition Object";
            oFields.Add(of);
            conditionContainer.Add(of);
        };

        rem_btn.clicked += () =>
        {
            if (conditionIndex >= 0)
            {
                conditionalData.RemoveConditionAtIndex(conditionIndex);
                ObjectField oField = oFields[conditionIndex];
                oFields.Remove(oField);
                oField.RemoveFromHierarchy();
                OnNodeDataChanged?.Invoke(this, currentNodeData);
            }

            conditionIndex--;
            if (conditionIndex < 0)
            {
                conditionIndex = -1;
            }
        };
    }
}

