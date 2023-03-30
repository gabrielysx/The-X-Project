using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NodeState
{
    SUCCESS, FAILURE, RUNNING
}

public abstract class Node
{
    public abstract NodeState Evaluate();

}

//Create different control nodes for the behaviour tree

//Create the selector node class
public class SelectorNode : Node
{
    private List<Node> childNodes = new List<Node>();

    public SelectorNode(params Node[] nodes)
    {
        childNodes.AddRange(nodes);
    }

    public override NodeState Evaluate()
    {
        foreach (Node node in childNodes)
        {
            switch (node.Evaluate())
            {
                case NodeState.SUCCESS:
                    return NodeState.SUCCESS;
                case NodeState.FAILURE:
                    continue;
                case NodeState.RUNNING:
                    return NodeState.RUNNING;
            }
        }
        return NodeState.FAILURE;
    }
}


//Create the sequence node class
public class SequenceNode : Node
{
    private List<Node> childNodes = new List<Node>();
    public SequenceNode(params Node[] nodes)
    {
        childNodes.AddRange(nodes);
    }
    public override NodeState Evaluate()
    {
        foreach (Node node in childNodes)
        {
            switch (node.Evaluate())
            {
                case NodeState.SUCCESS:
                    continue;
                case NodeState.FAILURE:
                    return NodeState.FAILURE;
                case NodeState.RUNNING:
                    return NodeState.RUNNING;
            }
        }
        return NodeState.SUCCESS;
    }
}
//Create the parallel node class that return success if a defined portion of the nodes have returned success
public class ParallelNode : Node
{
    private List<Node> childNodes = new List<Node>();
    private float successRatio;
    public ParallelNode(float successRatio, params Node[] nodes)
    {
        childNodes.AddRange(nodes);
        this.successRatio = successRatio;
    }
    public override NodeState Evaluate()
    {
        int successCount = 0;
        int runningCount = 0;
        float successThreshold = childNodes.Count * successRatio;

        foreach (Node node in childNodes)
        {
            switch (node.Evaluate())
            {
                case NodeState.SUCCESS:
                    successCount++;
                    break;
                case NodeState.FAILURE:
                    continue;
                case NodeState.RUNNING:
                    runningCount++;
                    break;
            }
        }
        //When the number of success nodes is greater than the success threshold, return success
        if (successCount >= successThreshold)
        {
            return NodeState.SUCCESS;
        }
        //When the sum of success nodes and running nodes is less than the success threshold, return failure
        if (runningCount + successCount < successThreshold)
        {
            return NodeState.FAILURE;
        }
        //When running nodes are present, return running
        if (runningCount > 0)
        {
            return NodeState.RUNNING;
        }
        //Otherwise return failure
        return NodeState.FAILURE;
    }
}