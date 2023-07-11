using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Moq;
using dialogues.node;
using dialogues.editor.treeHandler;

public class TreeHandlerTest
{
    ITreeHandler treeHandlerService = null;

    [SetUp]
    public void SetUp()
    {
        treeHandlerService = new TreeHandler();
    }

    [TearDown]
    public void TearDown()
    {
         
    }

    #region CreateNodeFromData
    [Test]
    public void TestCreateNullNodeFromData()
    {
        TreeNode node = treeHandlerService.CreateNodeFromData(null);
        Assert.IsTrue(node == null);
    }

    [Test]
    public void TestCreateRootNodeFromData()
    {
        //Given
        NodeData nodeData = new NodeData()
        {
            EventContainers = new List<Utils.EventContainer>()
            {
                new Utils.EventContainer()
            },
            Position = new Vector2(0, 1),
            Guid = UnityEditor.GUID.Generate().ToString(),
        };

        RootData rootData = new RootData(nodeData);

        //When
        TreeNode treeNode = treeHandlerService.CreateNodeFromData(rootData);

        //Then
        Assert.IsTrue(treeNode is RootNode);
        Assert.IsTrue(treeNode.position == nodeData.Position);
        Assert.IsTrue(treeNode.guid == nodeData.Guid);
        Assert.IsTrue(treeNode.EventContainers[0] == nodeData.EventContainers[0]);
    }

    [Test]
    public void TestCreateDialogueNodeFromData()
    {
        //Given
        NodeData nodeData = new NodeData()
        {
            EventContainers = new List<Utils.EventContainer>()
            {
                new Utils.EventContainer()
            },
            Position = new Vector2(0, 1),
            Guid = UnityEditor.GUID.Generate().ToString(),
        };

        RootData rootData = new RootData(nodeData);
        TreeNode rootNode = treeHandlerService.CreateNodeFromData(rootData);
        Assert.IsTrue(rootNode is RootNode);

        DialogueData dialogueData = new DialogueData(nodeData)
        {
            SpeakerName = "hehe",
            Dialogue = "Bonjour"
        };

        //When
        TreeNode treeNode = treeHandlerService.CreateNodeFromData(dialogueData);

        //Then
        Assert.IsTrue(treeNode is DialogueNode);
        Assert.IsTrue(treeNode.position == dialogueData.Position);
        Assert.IsTrue(treeNode.guid == dialogueData.Guid);
        Assert.IsTrue(treeNode.EventContainers[0] == dialogueData.EventContainers[0]);
        Assert.IsTrue((treeNode as DialogueNode).SpeakerName == dialogueData.SpeakerName);
        Assert.IsTrue((treeNode as DialogueNode).Dialogue == dialogueData.Dialogue);
    }

    [Test]
    public void TestCreateEndNodeFromData()
    {
        //Given
        NodeData nodeData = new NodeData()
        {
            EventContainers = new List<Utils.EventContainer>()
            {
                new Utils.EventContainer()
            },
            Position = new Vector2(0, 1),
            Guid = UnityEditor.GUID.Generate().ToString(),
        };

        RootData rootData = new RootData(nodeData);
        TreeNode rootNode = treeHandlerService.CreateNodeFromData(rootData);
        Assert.IsTrue(rootNode is RootNode);

        EndData dialogueData = new EndData(nodeData);

        //When
        TreeNode treeNode = treeHandlerService.CreateNodeFromData(dialogueData);

        //Then
        Assert.IsTrue(treeNode is EndNode);
        Assert.IsTrue(treeNode.position == dialogueData.Position);
        Assert.IsTrue(treeNode.guid == dialogueData.Guid);
        Assert.IsTrue(treeNode.EventContainers[0] == dialogueData.EventContainers[0]);
    }

    [Test]
    public void TestCreateConditionNodeFromData()
    {
        //Given
        NodeData nodeData = new NodeData()
        {
            EventContainers = new List<Utils.EventContainer>()
            {
                new Utils.EventContainer()
            },
            Position = new Vector2(0, 1),
            Guid = UnityEditor.GUID.Generate().ToString(),
        };

        RootData rootData = new RootData(nodeData);
        TreeNode rootNode = treeHandlerService.CreateNodeFromData(rootData);
        Assert.IsTrue(rootNode is RootNode);

        ConditionData conditionData = new ConditionData(nodeData)
        {
            ConditionContainers = new List<Utils.ConditionContainer>()
            {
                new Utils.ConditionContainer(),
            }
        };

        //When
        TreeNode treeNode = treeHandlerService.CreateNodeFromData(conditionData);

        //Then
        Assert.IsTrue(treeNode is ConditionNode);
        Assert.IsTrue(treeNode.position == conditionData.Position);
        Assert.IsTrue(treeNode.guid == conditionData.Guid);
        Assert.IsTrue(treeNode.EventContainers[0] == conditionData.EventContainers[0]);
        Assert.IsTrue((treeNode as ConditionNode).ConditionContainers[0] == conditionData.ConditionContainers[0]);
    }
    #endregion
}
