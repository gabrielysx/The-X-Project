using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class DashCrabStatesManager : FSM_Manager
{
    private DashCrab dashCrabController;
    private void Start()
    {
        dashCrabController= GetComponent<DashCrab>();
        currentState = SwitchBetweenStates(StateType.Idle);
    }
    protected override IState SwitchBetweenStates(StateType typeName)
    {
        switch (typeName)
        {
            case StateType.Idle:
                return new EIdleState(this, dashCrabController);
            case StateType.Attack:
                return new EAttackState(this, dashCrabController);
            case StateType.Patrol:
                return new EPatrolState(this, dashCrabController);
            case StateType.MoveToPlayer:
                return new ECrabChasePlayerState(this, dashCrabController);
            case StateType.Hitback:
                return new ECrabHitbackState(this, dashCrabController);
            case StateType.Die:
                return new EDieState(this, dashCrabController);      
            default:
                return new EIdleState(this, dashCrabController);
        }

    }
}

public class ECrabChasePlayerState : IState
{
    private FSM_Manager E_Manager;
    private DashCrab crabController;
    public ECrabChasePlayerState(FSM_Manager manager, DashCrab crabController)
    {
        this.E_Manager = manager;
        this.crabController = crabController;
    }
    public void OnEnter()
    {
        //Start Chasing or Resume Chasing(if isEndure)
        crabController.EnterChasing();
    }
    public void OnExit()
    {
        //Reset speed
        crabController.ExitChasing();
    }
    public void OnUpdate()
    {
        //Chasing
        crabController.OnChasing();

    }
    public StateType ExitConditions()
    {
        if (crabController.GetIsChasing())
        {
            return StateType.MoveToPlayer;
        }
        else
        {
            return StateType.Idle;
        }
    }
}

public class ECrabHitbackState: EHitbackState<DashCrab>
{
    public ECrabHitbackState(FSM_Manager m, DashCrab crabController)
    {
        E_Manager = m;
        enemyController = crabController;
    }

    public override StateType ExitConditions()
    {
        //if endure
        if (enemyController.GetIsEndure())
        {
            return StateType.MoveToPlayer;
        }
        //If still hitback
        else if (enemyController.IfHitback())
        {
            return StateType.Hitback;
        }
        else if (enemyController.IfFleeing())
        {
            return StateType.Flee;
        }
        //If player is within the attack range, transite to Attack state
        else if (enemyController.IfWithinAttackRange() && enemyController.GetIsAngry())
        {
            return StateType.Attack;
        }
        //If player is within the detect range but outside attack range, transite to MoveToPlayer state
        else if ((enemyController.GetIsChasing() || enemyController.IfWithinDetectRange()) && enemyController.GetIsAngry())
        {
            return StateType.MoveToPlayer;
        }
        // If player is outside the detect range, transite to Patrol state or idle
        else
        {
            if (enemyController.GetPatrolPointsCount() != 0)
            {
                return StateType.Patrol;
            }
            else
            {
                return StateType.Idle;
            }
        }
    }

}
