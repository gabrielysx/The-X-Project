using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatStatesManager : FSM_Manager
{
    private Bat batController;
    private void Start()
    {
        batController = GetComponent<Bat>();
        currentState = SwitchBetweenStates(StateType.Idle);
    }

    protected override IState SwitchBetweenStates(StateType typeName)
    {
        switch (typeName)
        {
            case StateType.Idle:
                return new EIdleState(this, batController);
            case StateType.Attack:
                return new EBatAttackState(this, batController);
            case StateType.Patrol:
                return new EPatrolState(this, batController);
            case StateType.MoveToPlayer:
                return new EMoveToPlayerState(this, batController);
            case StateType.Hitback:
                return new EBatHitbackState(this, batController);
            case StateType.Die:
                return new EDieState(this, batController);
            default:
                return new EIdleState(this, batController);
        }
    }
}

public class EBatAttackState : IState
{
    private FSM_Manager E_Manager;
    private Bat batController;
    public EBatAttackState(FSM_Manager manager, Bat batController)
    {
        this.E_Manager = manager;
        this.batController = batController;
    }

    public StateType ExitConditions()
    {
        if (batController.GetIsAttacking())
        {
            return StateType.Attack;
        }
        else
        {
            return StateType.Idle;
        }
    }

    public void OnEnter()
    {
        //Start Attack
        batController.EnterAttack();
    }
    public void OnExit()
    {
        //Reset speed
        batController.ExitAttack();
    }
    public void OnUpdate()
    {
        //Attack
        batController.OnAttack();
    }
}

public class EBatHitbackState : IState
{
    private FSM_Manager E_Manager;
    private Bat batController;
    public EBatHitbackState(FSM_Manager m, Bat batController)
    {
        this.E_Manager = m;
        this.batController = batController;
    }
    public void OnEnter()
    {
        batController.EnterHitback();
    }
    public void OnExit()
    {
        batController.StopHitback();
    }
    public void OnUpdate()
    {
        batController.Hitback();
    }

    public StateType ExitConditions()
    {
        
        //If still hitback
        if (batController.IfHitback())
        {
            return StateType.Hitback;
        }
        //if still attacking
        else if (batController.GetIsAttacking())
        {
            return StateType.Attack;
        }
        else if (batController.IfFleeing())
        {
            return StateType.Flee;
        }
        //If player is within the attack range, transite to Attack state
        else if (batController.IfWithinAttackRange())
        {
            return StateType.Attack;
        }
        //If player is within the detect range but outside attack range, transite to MoveToPlayer state
        else if (batController.IfWithinDetectRange())
        {
            return StateType.MoveToPlayer;
        }
        // If player is outside the detect range, transite to Patrol state or idle
        else
        {
            if (batController.GetPatrolPointsCount() != 0)
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
