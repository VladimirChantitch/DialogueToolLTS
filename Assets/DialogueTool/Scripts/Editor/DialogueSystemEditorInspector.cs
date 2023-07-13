using dialogues.eventSystem;
using dialogues.data;
using System;
using System.Collections.Generic;
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

    List<ObjectField> oEventFields = new List<ObjectField>();
    List<ObjectField> oConditionFields = new List<ObjectField>();

    public DialogueSystemEditorInspector()
    {

    }

    public void Init(StyleSheet styleSheet, VisualTreeAsset listTree)
    {
        this.styleSheet = styleSheet;
        this.listTree = listTree;
        styleSheets.Add(styleSheet);
    }

    public void ChangeInspectorBinding(NodeData nodeData)
    {
        eventIndex = -1;
        conditionIndex = -1;
        currentNodeData = nodeData;

        oEventFields.Clear();
        oConditionFields.Clear();
        Clear();
        switch (nodeData)
        {
            case EndNodeData endData:
                ShowEndInspector(endData);
                break;

            case ConditionNodeData conditionalData:
                ShowConditionInspector(conditionalData);
                break;

            case RootNodeData rootData:
                ShowRootInspector(rootData);
                break;

            case DialogueNodeData dialogueData:
                ShowDialogueInspector(dialogueData);
                break;
        }
    }


    private void ShowBasicInspector(string nodeName)
    {
        AddToClassList("Container");
        Label name = new Label();
        name.text = nodeName;
        name.AddToClassList("subContainer");
        Add(name);

        SetUpEventList();
        LoadUpEventLists();
    }

    private void LoadUpEventLists()
    {
        currentNodeData.EventContainers.ForEach(ec =>
        {
            AddAnEventField().value = ec.EventableObject as DialogueEventsBaseClass;

        });
    }

    private void ShowEndInspector(EndNodeData endData)
    {
        ShowBasicInspector("End Node");
    }

    private void ShowConditionInspector(ConditionNodeData conditionalData)
    {
        ShowBasicInspector("Condition Node");
        SetUpConditionList(conditionalData);
        LoadUpConditionsList(conditionalData);
    }

    private void LoadUpConditionsList(ConditionNodeData conditionalData)
    {
        conditionalData.ConditionContainers.ForEach(cc =>
        { 
            AddAnConditionField().value = cc?.ConditionableObject as DialogueConditionsBaseClass;
        });
    }

    private void ShowRootInspector(RootNodeData rootData)
    {
        ShowBasicInspector("Root Node");
    }

    private void ShowDialogueInspector(DialogueNodeData dialogueData)
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

        tf_dialogue.value = dialogueData.Dialogue;
        tf_name.value = dialogueData.SpeakerName;
        of_spriteIcone.value = dialogueData.SpeakerIcone;
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
            AddAnEventField();
            UpdateEvents();
        };

        rem_btn.clicked += () =>
        {
            if (eventIndex >= 0)
            {
                ObjectField oField = oEventFields[eventIndex];
                oEventFields.Remove(oField);
                oField.RemoveFromHierarchy();

                UpdateEvents();
            }

            eventIndex--;
            if (eventIndex < 0)
            {
                eventIndex = -1;
            }
        };
    }

    private void SetUpConditionList(ConditionNodeData conditionalData)
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
            AddAnConditionField();
            UpdateConditions();
        };

        rem_btn.clicked += () =>
        {
            if (conditionIndex >= 0)
            {
                ObjectField oField = oConditionFields[conditionIndex];
                oConditionFields.Remove(oField);
                oField.RemoveFromHierarchy();

                UpdateConditions();
            }

            conditionIndex--;
            if (conditionIndex < 0)
            {
                conditionIndex = -1;
            }
        };
    }

    private void UpdateEvents()
    {
        List<DialogueEventsBaseClass> dialogueEventsBaseClasses = new List<DialogueEventsBaseClass>();
        oEventFields.ForEach(o =>
        {
            dialogueEventsBaseClasses.Add(o.value as DialogueEventsBaseClass);
        });
        currentNodeData.UpdateEventsBasedOnFields(dialogueEventsBaseClasses);
        OnNodeDataChanged?.Invoke(this, currentNodeData);
    }

    private void UpdateConditions()
    {
        List<DialogueConditionsBaseClass> dialogueConditionsBaseClasses = new List<DialogueConditionsBaseClass>();
        oConditionFields.ForEach(o =>
        {
            dialogueConditionsBaseClasses.Add(o.value as DialogueConditionsBaseClass);
        });
        (currentNodeData as ConditionNodeData).UpdateConditionsBasedOnFields(dialogueConditionsBaseClasses);
        OnNodeDataChanged?.Invoke(this, currentNodeData);
    }

    private ObjectField AddAnEventField()
    {
        eventIndex++;
        ObjectField of = new ObjectField();
        of.objectType = typeof(DialogueEventsBaseClass);
        of.RegisterValueChangedCallback((data) =>
        {
            UpdateEvents();
        });
        of.label = "Event Object";
        oEventFields.Add(of);
        eventContainer.Add(of);

        return of;
    }

    private ObjectField AddAnConditionField()
    {
        conditionIndex++;
        ObjectField of = new ObjectField();
        of.objectType = typeof(DialogueConditionsBaseClass);
        of.RegisterValueChangedCallback((data) =>
        {
            UpdateConditions();
        });
        of.label = "Condition Object";
        oConditionFields.Add(of);
        conditionContainer.Add(of);

        return of;
    }
}

