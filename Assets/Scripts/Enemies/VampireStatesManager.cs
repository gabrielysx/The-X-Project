using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class VampireStatesManager : FSM_Manager
{
    private Vampire vampireController;

    private void Start()
    {
        vampireController = GetComponent<Vampire>();
        currentState = SwitchBetweenStates(StateType.Idle);
    }

    protected override IState SwitchBetweenStates(StateType typeName)
    {
        switch (typeName)
        {
            case StateType.Idle:
                return new EIdleState(this, vampireController);
            case StateType.Attack:
                return new EVampireAttackState(this, vampireController);
            case StateType.Patrol:
                return new EPatrolState(this, vampireController);
            case StateType.MoveToPlayer:
                return new EMoveToPlayerState(this, vampireController);
            case StateType.Hitback:
                return new EVampireHitbackState(this, vampireController);
            case StateType.Die:
                return new EDieState(this, vampireController);
            case StateType.Flee:
                return new EFleeState(this, vampireController);
            default:
                return new EIdleState(this, vampireController);
        }

    }
}

public class EVampireHitbackState : EHitbackState<Vampire>
{
    public EVampireHitbackState(FSM_Manager manager, Vampire vampireController)
    {
        E_Manager = manager;
        enemyController = vampireController;
    }

}

public class EVampireAttackState : IState
{
    private FSM_Manager E_Manager;
    private Vampire vampireController;

    public EVampireAttackState(FSM_Manager e_Manager, Vampire vampireController)
    {
        E_Manager = e_Manager;
        this.vampireController = vampireController;
    }

    public StateType ExitConditions()
    {
        if (vampireController.IfWithinAttackRange())
        {
            return StateType.Attack;
        }
        else if (vampireController.IfWithinDetectRange())
        {
            return StateType.MoveToPlayer;
        }
        else
        {
            return StateType.Idle;
        }
    }

    public void OnEnter()
    {
        vampireController.EnterAttack();
    }

    public void OnExit()
    {
        vampireController.ExitAttack();
    }

    public void OnUpdate()
    {
        vampireController.InAttack();
    }
}
