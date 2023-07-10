using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.Linq;
namespace dialogues
{
    //public class NodeView : UnityEditor.Experimental.GraphView.Node
    //{
    //    public Action<NodeView> OnNodeSelected;
    //    public Node node = null;
    //    public Port input = null;
    //    public List<Port> output = null;
    //    public List<string> outPortNode = null;

    //    List<VisualElement> conditionOrEventContainers = null;
    //    VisualElement ConditionsContainer = null;

    //    public NodeView(Node node, string uxmlFile) : base(uxmlFile)
    //    {
    //        this.node = node;
    //        this.title = node.name;
    //        //this.viewDataKey = node.guid;

    //        //style.left = node.position.x;
    //        //style.top = node.position.y;

    //        CreateInputPorts();
    //        CreateOutputPorts();
    //        SetUpClasses();
    //        BindUI(node);
    //    }

    //    private void BindUI(Node node)
    //    {
    //        Label descriptionLabel = null;

    //        switch (node)
    //        {
    //            //case IListableNode:
    //            //    //BindListableNodes(node);
    //            //    break;
    //            //case IQuestStep:
    //            //    descriptionLabel = this.Q<Label>("description");
    //            //    descriptionLabel.bindingPath = "step_name";
    //            //    break;
    //            //case RootNode:
    //            //    capabilities = Capabilities.Deletable;
    //            //    break;
    //            //default:
    //            //    descriptionLabel = this.Q<Label>("description");
    //            //    descriptionLabel.bindingPath = "description";
    //            //    break;
    //        }

    //        //descriptionLabel?.Bind(new SerializedObject(node));
    //    }

    //    #region Listable Node bindings

    //    private void BindListableNodes(Node node)
    //    {
    //        conditionOrEventContainers = new List<VisualElement>();
    //        ConditionsContainer = this.Q<Foldout>("OptionsList").Q<VisualElement>("unity-content");
    //        this.Q<Label>("description").bindingPath = "description";

    //        switch (node)
    //        {
    //            case ConditionalNode conditionalNode:
    //                conditionalNode?.UiUpdates.AddListener((abs_con_node) => BindCondtionalNode(abs_con_node));

    //                if (conditionalNode.Conditions.Count > 0)
    //                {
    //                    conditionalNode.Conditions.ForEach(condition =>
    //                    {
    //                        AddAConditionContainer(conditionalNode, "Assets/UI/ConditionalOption.uxml");
    //                    });
    //                }

    //                break;

    //            case EventNode eventNode:
    //                eventNode?.UiUpdates.AddListener((evt_node) => BindEventNode(evt_node));

    //                if (eventNode.Events.Count > 0)
    //                {
    //                    eventNode.Events.ForEach(condtion =>
    //                    {
    //                        AddAConditionContainer(eventNode, "Assets/UI/EventOption.uxml");
    //                    });
    //                }

    //                break;
    //        }

    //        Debug.Log(node.name);

    //        BindAddButtont(node);
    //        BindRemoveButton(node);
    //    }

    //    private void BindCondtionalNode(AbstractConditionalNode conditionalNode)
    //    {
    //        if (conditionalNode == null) return;
    //        this.Q<Label>("quantity").text = conditionalNode.Conditions.Count().ToString();

    //        if (conditionalNode.Conditions.Count() > conditionOrEventContainers.Count())
    //        {
    //            AddAConditionContainer(conditionalNode, "Assets/UI/ConditionalOption.uxml");
    //        }
    //        else if (conditionalNode.Conditions.Count() < conditionOrEventContainers.Count())
    //        {
    //            RemoveACondtionContainer();
    //        }

    //        for (int i = 0; i < conditionOrEventContainers.Count(); i++)
    //        {
    //            UpdateSingleConditionContent(conditionalNode.Conditions[i], conditionOrEventContainers[i]);
    //        }
    //    }

    //    private void BindEventNode(AbstractEventNode eventNode)
    //    {
    //        if (eventNode == null) return;
    //        this.Q<Label>("quantity").text = eventNode.Events.Count().ToString();

    //        if (eventNode.Events.Count() > conditionOrEventContainers.Count())
    //        {
    //            AddAConditionContainer(eventNode, "Assets/UI/EventOption.uxml");
    //        }
    //        else if (eventNode.Events.Count() < conditionOrEventContainers.Count())
    //        {
    //            RemoveACondtionContainer();
    //        }

    //        for (int i = 0; i < conditionOrEventContainers.Count(); i++)
    //        {
    //            UpdateSingleConditionContent(eventNode.Events[i], conditionOrEventContainers[i]);
    //        }
    //    }

    //    private void BindAddButtont(Node node)
    //    {
    //        Button add = this.Q<Button>("add");
    //        switch (node)
    //        {
    //            case ConditionalNode conditionalNode:
    //                add.clicked += () =>
    //                {
    //                    conditionalNode.AddAnEmptyField();
    //                    AddAConditionContainer(conditionalNode, "Assets/UI/ConditionalOption.uxml");
    //                };
    //                break;
    //            case EventNode eventNode:
    //                add.clicked += () =>
    //                {
    //                    eventNode.AddAnEmptyField();
    //                    AddAConditionContainer(eventNode, "Assets/UI/ConditionalOption.uxml");
    //                };
    //                break;
    //        }

    //    }

    //    private void BindRemoveButton(Node node)
    //    {
    //        Button remove = this.Q<Button>("remove");
    //        switch (node)
    //        {
    //            case ConditionalNode conditionalNode:
    //                remove.clicked += () =>
    //                {
    //                    conditionalNode.RemoveACondtion();
    //                    RemoveACondtionContainer();
    //                };
    //                break;
    //            case EventNode eventNode:
    //                remove.clicked += () =>
    //                {
    //                    eventNode.RemoveAnEvent();
    //                    RemoveACondtionContainer();
    //                };
    //                break;
    //        }

    //    }

    //    private void AddAConditionContainer(Node node, string uxml)
    //    {
    //        VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(uxml);
    //        visualTree.CloneTree(ConditionsContainer);
    //        conditionOrEventContainers.Add(ConditionsContainer.Children().Last());
    //        this.Q<Label>("quantity").text = conditionOrEventContainers.Count().ToString();

    //        switch (node)
    //        {
    //            case ConditionalNode conditionalNode:
    //                UpdateSingleConditionContent(conditionalNode.Conditions.Last(), conditionOrEventContainers.Last());
    //                break;
    //            case EventNode eventNode:
    //                UpdateSingleConditionContent(eventNode.Events.Last(), conditionOrEventContainers.Last());
    //                break;
    //        }
    //    }

    //    private void RemoveACondtionContainer(int id = -1)
    //    {
    //        if (conditionOrEventContainers.Count() == 0) return;
    //        if (id >= 0)
    //        {
    //            ConditionsContainer.RemoveAt(id);
    //            conditionOrEventContainers.RemoveAt(id);
    //        }
    //        else
    //        {
    //            ConditionsContainer.Remove(conditionOrEventContainers.Last());
    //            conditionOrEventContainers.RemoveAt(conditionOrEventContainers.Count - 1);
    //        }
    //        this.Q<Label>("quantity").text = conditionOrEventContainers.Count().ToString();
    //    }

    //    private void UpdateSingleConditionContent(ListableData data, VisualElement visual)
    //    {
    //        if (visual != null)
    //        {
    //            switch (data)
    //            {
    //                case ConditionalNode.ConditionData conditionData:
    //                    visual.Q<Label>("ConditionName").text = conditionData.condtionName;
    //                    visual.Q<Label>("ConditionType").text = Enum.GetName(typeof(ConditionTypes), conditionData.ConditionType);
    //                    visual.Q<Toggle>("IsAdditive").value = conditionData.isAdditiveOrSoloCarry;
    //                    visual.Q<Label>("Quantity").text = conditionData.amount.ToString();
    //                    break;
    //                case EventNode.EventableData eventData:
    //                    visual.Q<Label>("ConditionName").text = eventData.eventName;
    //                    visual.Q<Label>("ConditionType").text = Enum.GetName(typeof(ConditionTypes), eventData.EventType);
    //                    visual.Q<Label>("Quantity").text = eventData.repeatAmount.ToString();
    //                    break;
    //            }
    //        }
    //    }
    //    #endregion

    //    /// <summary>
    //    /// Set ups the UCSS classes depending on the type of node
    //    /// </summary>
    //    private void SetUpClasses()
    //    {
    //        switch (node)
    //        {
    //            case PlayerDialogueNode:
    //                AddToClassList(DialoguesSystemUtils.NodesNames.GetValueOrDefault("PLAYER"));
    //                break;
    //            case NPCDialogueNode:
    //                AddToClassList(DialoguesSystemUtils.NodesNames.GetValueOrDefault("NPC"));
    //                break;
    //            case ConditionalNode:
    //                AddToClassList(DialoguesSystemUtils.NodesNames.GetValueOrDefault("CONDITION"));
    //                break;
    //            case EventNode:
    //                AddToClassList(DialoguesSystemUtils.NodesNames.GetValueOrDefault("EVENT"));
    //                break;
    //            case RootNode:
    //                AddToClassList(DialoguesSystemUtils.NodesNames.GetValueOrDefault("ROOT"));
    //                break;
    //            case EndNode:
    //                AddToClassList(DialoguesSystemUtils.NodesNames.GetValueOrDefault("END"));
    //                break;
    //            case QuestStep:
    //                AddToClassList(DialoguesSystemUtils.NodesNames.GetValueOrDefault("QUEST"));
    //                break;
    //            default:
    //                break;
    //        }
    //    }

    //    private void CreateInputPorts()
    //    {
    //        switch (node)
    //        {
    //            case PlayerDialogueNode:
    //                input = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Multi, typeof(bool));
    //                break;
    //            case NPCDialogueNode:
    //                input = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Multi, typeof(bool));
    //                break;
    //            case ConditionalNode:
    //                input = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Multi, typeof(bool));
    //                break;
    //            case EventNode:
    //                input = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Multi, typeof(bool));
    //                break;
    //            case RootNode:
    //                break;
    //            case EndNode:
    //                input = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(bool));
    //                break;
    //            case QuestStep:
    //                input = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Multi, typeof(bool));
    //                break;
    //            default:
    //                break;
    //        }

    //        if (input != null)
    //        {
    //            input.portName = "";
    //            input.style.flexDirection = FlexDirection.Column;
    //            inputContainer.Add(input);
    //        }
    //    }

    //    private void CreateOutputPorts()
    //    {
    //        output = new List<Port>();
    //        outPortNode = new List<string>();
    //        switch (node)
    //        {
    //            case PlayerDialogueNode:
    //                output.Add(InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Single, typeof(bool)));
    //                outPortNode.Add("outPlayer");
    //                break;
    //            case NPCDialogueNode:
    //                output.Add(InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Multi, typeof(bool)));
    //                outPortNode.Add("outNPC");
    //                break;
    //            case ConditionalNode:
    //                output.Add(InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Multi, typeof(bool)));
    //                outPortNode.Add("outTrue");
    //                output.Add(InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Multi, typeof(bool)));
    //                outPortNode.Add("outFalse");
    //                break;
    //            case EventNode:
    //                output.Add(InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Multi, typeof(bool)));
    //                outPortNode.Add("outEvent");
    //                break;
    //            case RootNode:
    //                output.Add(InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Multi, typeof(bool)));
    //                outPortNode.Add("outRoot");
    //                break;
    //            case EndNode:
    //                break;
    //            case QuestStep:
    //                output.Add(InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Multi, typeof(bool)));
    //                outPortNode.Add("outQuest");
    //                break;
    //            default:
    //                break;
    //        }

    //        if (output != null)
    //        {
    //            int portIndex = 0;
    //            List<VisualElement> used_container = new List<VisualElement>();
    //            VisualElement left_port = outputContainer.Q<VisualElement>("left_port");
    //            VisualElement middle_port = outputContainer.Q<VisualElement>("middle_port");
    //            VisualElement right_port = outputContainer.Q<VisualElement>("right_port");
    //            foreach (Port port in output)
    //            {
    //                port.name = outPortNode[portIndex];
    //                port.portName = "";

    //                port.style.height = 25;
    //                port.style.width = 25;
    //                port.style.flexDirection = FlexDirection.Column;
    //                portIndex++;
    //                switch (output.Count)
    //                {
    //                    case 1:
    //                        if (!used_container.Contains(middle_port))
    //                        {
    //                            middle_port.Add(port);
    //                            used_container.Add(middle_port);
    //                        }
    //                        break;
    //                    case 2:
    //                        if (!used_container.Contains(left_port))
    //                        {
    //                            Label label = new Label("T");
    //                            left_port.Add(label);
    //                            left_port.Add(port);
    //                            used_container.Add(left_port);
    //                        }
    //                        else if (!used_container.Contains(right_port))
    //                        {
    //                            Label label = new Label("F");
    //                            right_port.Add(label);
    //                            right_port.Add(port);
    //                            used_container.Add(right_port);
    //                        }
    //                        break;
    //                }
    //            }
    //        }
    //    }

    //    public override void SetPosition(Rect newPos)
    //    {
    //        base.SetPosition(newPos);
    //        Undo.RecordObject(node, "Behaviour Tree (Set Position)");
    //        node.position.x = newPos.xMin;
    //        node.position.y = newPos.yMin;
    //        EditorUtility.SetDirty(node);
    //    }

    //    public override void OnSelected()
    //    {
    //        base.OnSelected();
    //        OnNodeSelected?.Invoke(this);
    //    }

    //    public void SortChildren()
    //    {
    //        if (node is AbstractDialogueNode abs_node)
    //        {
    //            abs_node.children.Sort(SortByHorizontalPosition);
    //        }
    //    }

    //    private int SortByHorizontalPosition(Node left, Node right)
    //    {
    //        return left.position.x < right.position.x ? -1 : 1;
    //    }
    //}
}

