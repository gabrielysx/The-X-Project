using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateManager : FSM_Manager
{
    private Enemy enemyController;
    //public Enemy GetEnemyController() { return enemyController; };

    private void Start()
    {
        enemyController = GetComponent<Enemy>();
        currentState = SwitchBetweenStates(StateType.Idle);
    }

    protected override IState SwitchBetweenStates(StateType typeName)
    {
        switch (typeName)
        {
            case StateType.Idle:
                return new EIdleState(this, enemyController);
            case StateType.Attack:
                return new EAttackState(this, enemyController);
            case StateType.Patrol:
                return new EPatrolState(this, enemyController);
            case StateType.MoveToPlayer:
                return new EMoveToPlayerState(this, enemyController);
            case StateType.Hitback:
                return new EHitbackState(this, enemyController);
            case StateType.Die:
                return new EDieState(this, enemyController);
            default:
                return new EIdleState(this, enemyController);
        }

    }
}
