using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BehaviorContext
{
    public Vector2 ChaseTargetPosition { get; set; }
    public string TargetName { get; set; }
}

public class WaitAction : Node
{
    private float waitTime;
    private float elapsedTime;

    public override NodeState Evaluate()
    {
        //Return RUNNING until the wait time has elapsed.
        //Return SUCCESS once the wait time has elapsed.
        //Return FAILURE if unable to process wait.
    }
}
    public class SetChaseTargetAction : Node
{
    private BehaviorContext context;

    public SetChaseTargetAction(BehaviorContext context)
    {
        this.context = context;
    }

    public override NodeState Evaluate()
    {
        // Add logic to set the chase target position.
        context.ChaseTargetPosition = ...; // Set the target position here.
        context.TargetName = ...; // Set the target name here.

        // Return the appropriate NodeState based on the success or failure of the action.
        // For example, if the chase target is successfully set, return NodeState.SUCCESS.
        // If it fails to set the chase target, return NodeState.FAILURE.
    }
}

private class MoveToAction : Node
{
    private BehaviorContext context;

    public MoveToAction(BehaviorContext context)
    {
        this.context = context;
    }

    // Implement the logic to move the game object to its target position.
    public override NodeState Evaluate()
    {
        // Add logic to move the game object.

        // Return the appropriate NodeState based on the success or failure of the action.
        // If the game object successfully moves to the target position, return NodeState.SUCCESS.
        // If it is still moving, return NodeState.RUNNING.
        // If it fails to move, return NodeState.FAILURE.
    }
}

public class MobBehavior : MonoBehaviour
{
    private SelectorNode root;
    // Start is called before the first frame update
    void Start()
    {
        SequenceNode MoveToSequence = new SequenceNode(
            new DistanceCondition(),
            new SetChaseTargetAction(),
            new MoveToAction()
            new WaitAction()
        );

        SequenceNode PatrolSequence = new SequenceNode(
            new SetPatrolTargetAction(),
            new MoveToAction(),
            new WaitAction()
        );

        SelectorNode walkSelector = new SelectorNode(
            MoveToSequence,
            PatrolSequence
        );

        ParallelNode PickPocketAttemptParallel = new ParallelNode(
            0.5f, // Maximum percentage of child nodes that can fail
            new LuckAttemptCondition(),
            new AgiAttemptCondition(),
            new AbilityAttemptCondition()

        );

        SequenceNode PickPocketSequence = new SequenceNode(
            PickPocketAttemptParallel,
            new PickPocketAction()        
        );

        SelectorNode behaviorSelector = new SelectorNode(
            walkSelector,
            PickPocketSequence
        );

        root = new SelectorNode(
            behaviorSelector
        );
    }

    // Update is called once per frame
    void Update()
    {

    }
}

