using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class SlimeStatesManager : FSM_Manager
{
    private Slime slimeController;
    private void Start()
    {
        slimeController = GetComponent<Slime>();
        currentState = SwitchBetweenStates(StateType.Idle);
    }
    protected override IState SwitchBetweenStates(StateType typeName)
    {
        switch (typeName)
        {
            case StateType.Idle:
                return new ESlimeIdleState(this, slimeController);
            case StateType.Attack:
                return new ESlimeDashState(this, slimeController);
            case StateType.Patrol:
                return new ESlimeRandomMoveState(this, slimeController);
            case StateType.MoveToPlayer:
                return new EMoveToPlayerState(this, slimeController);
            case StateType.Hitback:
                return new ESlimeHitbackState(this, slimeController);
            case StateType.Die:
                return new EDieState(this, slimeController);
            default:
                return new EIdleState(this, slimeController);
        }

    }
}

public class ESlimeDashState : IState
{
    private FSM_Manager E_Manager;
    private Slime slimeController;
    public ESlimeDashState(FSM_Manager manager, Slime slimeController)
    {
        this.E_Manager = manager;
        this.slimeController = slimeController;
    }
    public void OnEnter()
    {
        slimeController.EnterDash();
    }
    public void OnExit()
    {
        slimeController.ExitDash();
    }
    public void OnUpdate()
    {
        //Dashing
        slimeController.Dash();
    }
    public StateType ExitConditions()
    {
        if (slimeController.GetIsDashing())
        {
            return StateType.Attack;
        }
        else
        {
            return StateType.Idle;
        }
    }
}

public class ESlimeHitbackState : EHitbackState<Slime>
{
    public ESlimeHitbackState(FSM_Manager manager, Slime slimeController)
    {
        E_Manager= manager;
        enemyController = slimeController;
    }

}

public class ESlimeIdleState : IState
{
    protected FSM_Manager E_Manager;
    protected Slime enemyController;

    public ESlimeIdleState(FSM_Manager manager, Slime ec)
    {
        E_Manager = manager;
        enemyController = ec;
    }

    public void OnEnter()
    {
        Debug.Log("Enter Idle State");
        //Play Idle Animation
    }

    public void OnExit()
    {
        Debug.Log("Exit Idle State");
        //Stop playing Idle Animation
    }

    public void OnUpdate()
    {

    }

    public StateType ExitConditions()
    {
        //If player is within the attack range, transite to Attack state
        if (enemyController.IfWithinAttackRange())
        {
            return StateType.Attack;
        }
        //If player is within the detect range but outside attack range, transite to MoveToPlayer state
        else if (enemyController.IfWithinDetectRange())
        {
            return StateType.MoveToPlayer;
        }
        //Otherwise patrol automaticly
        else
        {
            return StateType.Patrol;
        }

    }
}

public class ESlimeRandomMoveState : IState
{
    private FSM_Manager E_Manager;
    private Slime slimeController;

    public ESlimeRandomMoveState(FSM_Manager manager, Slime slimeController)
    {
        this.E_Manager = manager;
        this.slimeController = slimeController;
    }

    public void OnEnter()
    {
        slimeController.EnterRandomMove();
        slimeController.SetAnimatorMoving(true);
    }

    public void OnExit()
    {
        Debug.Log("Exit Patrol State");
    }

    public void OnUpdate()
    {

        slimeController.RandomMove();

    }
    public StateType ExitConditions()
    {
        //If player is within the attack range, transite to Attack state
        if (slimeController.IfWithinAttackRange())
        {
            return StateType.Attack;
        }
        //If player is within the detect range but outside attack range, transite to MoveToPlayer state
        else if (slimeController.IfWithinDetectRange())
        {
            return StateType.MoveToPlayer;
        }
        //Otherwise continuing patrol automaticly
        else if (slimeController.GetIsRandomMoving())
        {
            return StateType.Patrol;
        }
        else
        {
            return StateType.Idle;
        }
    }
}
