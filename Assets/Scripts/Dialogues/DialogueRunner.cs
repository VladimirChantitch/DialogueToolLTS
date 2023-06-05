using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using dialogues;
using dialogues.node;

public class DialogueRunner : MonoBehaviour
{
    [SerializeField] RootNode rootNode;
    [SerializeField][SerializeReference] List<TreeNode> currentNodes = new List<TreeNode>();
    bool isIt = false;

    private void Awake()
    {
        currentNodes.Add(rootNode);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            Debug.Log("Pressed");
            List<TreeNode> nodes = new List<TreeNode>();
            currentNodes.ForEach(n =>
            {
                nodes.AddRange(HandleNode(n));
            });
            currentNodes.Clear();
            currentNodes.AddRange(nodes);
        }
    }

    private List<TreeNode> HandleNode(TreeNode node)
    {
        switch (node)
        {
            case RootNode rn:
                Debug.Log("Root");
                return rn.GetNextNodes();
            case DialogueNode dn:
                Debug.Log("Dialogue");
                Debug.Log(dn.GetDialogue());
                return dn.GetNextNodes();
            case ConditionalNode cn:
                Debug.Log("Condition");
                return cn.GetNextNodes();
            case EndNode en:
                Debug.Log("End");
                return en.GetNextNodes();
        }

        return new List<TreeNode>();
    }
}
