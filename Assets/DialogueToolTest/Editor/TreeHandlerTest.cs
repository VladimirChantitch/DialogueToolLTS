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
        Assert.IsTrue(treeHandlerService.RootNode.nodesModel.Count == 1);
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
        Assert.IsTrue(treeHandlerService.RootNode.nodesModel.Count == 2);
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
        Assert.IsTrue(treeHandlerService.RootNode.nodesModel.Count == 2);
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
        Assert.IsTrue(treeHandlerService.RootNode.nodesModel.Count == 2);
    }
    #endregion

    #region UpdateNode
    [Test]
    public void UpdateDialogueNodeWithNull()
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
            Dialogue = "Bonjour",
            Guid = UnityEditor.GUID.Generate().ToString()
        };

        TreeNode treeNode = treeHandlerService.CreateNodeFromData(dialogueData);
        Assert.IsTrue(treeNode is DialogueNode);

        //When
        treeHandlerService.UpdateNode(null);

        //Then
        Assert.IsTrue((treeNode as DialogueNode).Dialogue == dialogueData.Dialogue);
        Assert.IsTrue(treeHandlerService.RootNode.nodesModel.Count == 2);
    }

    [Test]
    public void UpdateDialogueNode()
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
            Dialogue = "Bonjour",
            Guid = UnityEditor.GUID.Generate().ToString()
        };

        TreeNode treeNode = treeHandlerService.CreateNodeFromData(dialogueData);
        Assert.IsTrue(treeNode is DialogueNode);

        //When
        dialogueData.Dialogue = "changed";
        treeHandlerService.UpdateNode(dialogueData);

        //Then
        Assert.IsTrue((treeNode as DialogueNode).Dialogue == dialogueData.Dialogue);
        Assert.IsTrue(treeHandlerService.RootNode.nodesModel.Count == 2);
    }

    #endregion

    #region DeleteNodefromdata
    [Test]
    public void DeleteInEmptyList()
    {
        NodeData nodeData = new NodeData()
        {
            EventContainers = new List<Utils.EventContainer>()
            {
                new Utils.EventContainer()
            },
            Position = new Vector2(0, 1),
            Guid = UnityEditor.GUID.Generate().ToString(),
        };

        DialogueData dialogueData = new DialogueData(nodeData)
        {
            SpeakerName = "hehe",
            Dialogue = "Bonjour",
            Guid = UnityEditor.GUID.Generate().ToString()
        };

        //When
        treeHandlerService.DeleteNodeFromData(dialogueData);

        //Then
        Assert.IsTrue(treeHandlerService.RootNode == null);

    }

    [Test]
    public void DeleteInexistantNode()
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
            Dialogue = "Bonjour",
            Guid = UnityEditor.GUID.Generate().ToString()
        };

        //When
        treeHandlerService.DeleteNodeFromData(dialogueData);

        //Then
        Assert.IsTrue(treeHandlerService.RootNode.nodesModel.Count == 1);
        Assert.IsTrue(treeHandlerService.RootNode.nodesModel.Find(n => n.guid == dialogueData.Guid) == null);
    }

    [Test]
    public void DeleteWithNullData()
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

        //When
        Assert.IsFalse(treeHandlerService.DeleteNodeFromData(null));

        //Then
        Assert.IsTrue(treeHandlerService.RootNode.nodesModel.Count == 1);
    }

    [Test]
    public void DeleteDialogueNode()
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
            Dialogue = "Bonjour",
            Guid = UnityEditor.GUID.Generate().ToString()
        };

        TreeNode treeNode = treeHandlerService.CreateNodeFromData(dialogueData);
        Assert.IsTrue(treeNode is DialogueNode);

        //When
        treeHandlerService.DeleteNodeFromData(dialogueData);

        //Then
        Assert.IsTrue(treeHandlerService.RootNode.nodesModel.Count == 1);
        Assert.IsTrue(treeHandlerService.RootNode.nodesModel.Find(n => n.guid == dialogueData.Guid) == null);
    }
    #endregion

    #region AddOrUpdateChild
    [Test]
    public void TestCreateOrUpdateNullChild()
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
            Dialogue = "Bonjour",
            Guid = UnityEditor.GUID.Generate().ToString(),
        };

        //When
        bool isOk = treeHandlerService.CreateOrUpdateChild(rootData, null);

        //Then
        Assert.IsFalse(isOk);
        Assert.IsTrue(treeHandlerService.RootNode.nodesModel.Count == 1);
    }

    [Test]
    public void TestCreateOrUpdateChildWithNullParent()
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
            Dialogue = "Bonjour",
            Guid = UnityEditor.GUID.Generate().ToString(),
        };

        //When
        bool isOk = treeHandlerService.CreateOrUpdateChild(null, dialogueData);
        TreeNode treeNode = treeHandlerService.RootNode.nodesModel.Find(n => n.guid == dialogueData.Guid);

        //Then
        Assert.IsFalse(isOk);
        Assert.IsTrue(treeNode == null);
        Assert.IsTrue(treeHandlerService.RootNode.nodesModel.Count == 1);
    }


    [Test]
    public void TestCreateOrUpdateNewDialogueChild()
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
            Dialogue = "Bonjour",
            Guid = UnityEditor.GUID.Generate().ToString(),
        };

        //When
        bool isOk = treeHandlerService.CreateOrUpdateChild(rootData ,dialogueData);
        TreeNode treeNode = treeHandlerService.RootNode.nodesModel.Find(n => n.guid == dialogueData.Guid);

        //Then
        Assert.IsTrue(isOk);
        Assert.IsTrue(treeNode is DialogueNode);
        Assert.IsTrue(treeNode.position == dialogueData.Position);
        Assert.IsTrue(treeNode.guid == dialogueData.Guid);
        Assert.IsTrue(treeNode.EventContainers[0] == dialogueData.EventContainers[0]);
        Assert.IsTrue((treeNode as DialogueNode).SpeakerName == dialogueData.SpeakerName);
        Assert.IsTrue((treeNode as DialogueNode).Dialogue == dialogueData.Dialogue);
        Assert.IsTrue(treeHandlerService.RootNode.nodesModel.Count == 2);
    }

    [Test]
    public void TestCreateOrUpdateDialogueChildMultipleRooting()
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
            Dialogue = "Bonjour",
            Guid = UnityEditor.GUID.Generate().ToString(),
        };


        //When
        int i = 10;
        bool isOk = true;
        for (int j = 0; j < i; j++)
        {
            dialogueData.Guid = UnityEditor.GUID.Generate().ToString();
            treeHandlerService.CreateNodeFromData(dialogueData);
            isOk = isOk && treeHandlerService.CreateOrUpdateChild(rootData, dialogueData);
        }
        TreeNode treeNode = treeHandlerService.RootNode.nodesModel.Find(n => n.guid == dialogueData.Guid);

        //Then
        Assert.IsTrue(isOk);
        Assert.IsTrue(treeNode is DialogueNode);
        Assert.IsTrue(treeNode.position == dialogueData.Position);
        Assert.IsTrue(treeNode.guid == dialogueData.Guid);
        Assert.IsTrue(treeNode.EventContainers[0] == dialogueData.EventContainers[0]);
        Assert.IsTrue((treeNode as DialogueNode).SpeakerName == dialogueData.SpeakerName);
        Assert.IsTrue((treeNode as DialogueNode).Dialogue == dialogueData.Dialogue);
        Assert.IsTrue(treeHandlerService.RootNode.nodesModel.Count == i + 1);
    }


    #endregion

    #region GetChildren
    [Test]
    public void TestGetChildren()
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
            Dialogue = "Bonjour",
            Guid = UnityEditor.GUID.Generate().ToString(),
        };

        List<TreeNode> children = new List<TreeNode>();
        TreeNode treeNode = treeHandlerService.CreateNodeFromData(dialogueData);
        treeHandlerService.CreateOrUpdateChild(rootData, dialogueData);

        int i = 10;
        for (int j = 0; j< i; j++)
        {
            dialogueData.Guid = UnityEditor.GUID.Generate().ToString();
            children.Add(treeHandlerService.CreateNodeFromData(dialogueData));
            treeHandlerService.CreateOrUpdateChild(treeNode.GetData(), dialogueData);
        }

        //When
        List<NodeData> childrenGotten = treeHandlerService.GetChildren(treeNode.GetData());

        //Then
        Assert.IsTrue(children.Count == childrenGotten.Count);
        Assert.IsTrue(childrenGotten.Count == i);
        Assert.IsTrue(treeHandlerService.RootNode.nodesModel.Count == i+2);

        for (int j = 0; j < i; j++)
        {
            Assert.IsTrue(children[j].guid == childrenGotten[j].Guid);
        }
    }

    #endregion

    #region Remove Child
    [Test]
    public void TestRemoveChild()
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
            Dialogue = "Bonjour",
            Guid = UnityEditor.GUID.Generate().ToString(),
        };

        List<TreeNode> children = new List<TreeNode>();
        TreeNode treeNode = treeHandlerService.CreateNodeFromData(dialogueData);
        treeHandlerService.CreateOrUpdateChild(rootData, dialogueData);

        //When
        bool isIt = treeHandlerService.RemoveChild(rootData, dialogueData);

        //Then
        Assert.IsTrue(isIt);
        Assert.IsTrue(treeHandlerService.RootNode.nodesModel.Count == 2);
        Assert.IsTrue(treeHandlerService.RootNode.DirectChildren.Count == 0);
    }
    [Test]
    public void TestRemoveNullChild()
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
            Dialogue = "Bonjour",
            Guid = UnityEditor.GUID.Generate().ToString(),
        };

        List<TreeNode> children = new List<TreeNode>();
        TreeNode treeNode = treeHandlerService.CreateNodeFromData(dialogueData);
        treeHandlerService.CreateOrUpdateChild(rootData, dialogueData);

        //When
        bool isIt = treeHandlerService.RemoveChild(rootData, null);

        //Then
        Assert.IsTrue(isIt);
        Assert.IsTrue(treeHandlerService.RootNode.nodesModel.Count == 2);
        Assert.IsTrue(treeHandlerService.RootNode.DirectChildren.Count == 1);
    }


    #endregion
}
