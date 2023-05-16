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
                return new ECrabDashState(this, dashCrabController);
            case StateType.Patrol:
                return new EPatrolState(this, dashCrabController);
            case StateType.MoveToPlayer:
                return new EMoveToPlayerState(this, dashCrabController);
            case StateType.Hitback:
                return new EHitbackState(this, dashCrabController);
            case StateType.Die:
                return new EDieState(this, dashCrabController);      
            default:
                return new EIdleState(this, dashCrabController);
        }

    }
}

public class ECrabDashState : IState
{
    private FSM_Manager E_Manager;
    private DashCrab crabController;
    public ECrabDashState(FSM_Manager manager, DashCrab crabController)
    {
        this.E_Manager = manager;
        this.crabController = crabController;
    }
    public void OnEnter()
    {
        crabController.EnterDash();
    }
    public void OnExit()
    {

    }
    public void OnUpdate()
    {
        //Dashing
        crabController.Dash();
    }
    public StateType ExitConditions()
    {
        if (crabController.GetIsDashing())
        {
            return StateType.Attack;
        }
        else
        {
            return StateType.Idle;
        }
    }
}
