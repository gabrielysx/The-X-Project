using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EIdleState : IState
{
    private EnemyStateManager E_Manager;
    private Enemy enemyController;

    public EIdleState(EnemyStateManager manager, Enemy ec)
    {
        this.E_Manager = manager;
        this.enemyController = ec;
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
        else if (enemyController.GetPatrolPointsCount() != 0)
        {
            return StateType.Patrol;
        }
        //No conditions met then continue idle state
        else
        {
            return StateType.Idle;
        }
    }

}

public class EPatrolState : IState
{
    private EnemyStateManager E_Manager;
    private Enemy enemyController;

    public EPatrolState(EnemyStateManager manager, Enemy enemyController)
    {
        this.E_Manager = manager;
        this.enemyController = enemyController;
    }

    public void OnEnter()
    {
        enemyController.EnterPatrol();
    }

    public void OnExit()
    {
        Debug.Log("Exit Patrol State");
    }

    public void OnUpdate()
    {

       enemyController.PatrolAlongRoute();
        
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
        //Otherwise continuing patrol automaticly
        else
        {
            return StateType.Patrol;
        }
    }
}

public class EMoveToPlayerState: IState
{
    private EnemyStateManager E_Manager;
    private Enemy enemyController;
    public EMoveToPlayerState(EnemyStateManager manager, Enemy ec)
    {
        this.E_Manager = manager;
        this.enemyController= ec;
    }
    public void OnEnter()
    {
        Debug.Log("Player Detected!");
    }
    public void OnExit()
    {
        Debug.Log("Lost the Target :(");
    }
    public void OnUpdate()
    {
        //Get position of the player and move to him
        enemyController.MovetoPlayer();
    }
    public StateType ExitConditions()
    {
        //If player is within the attack range, transite to Attack state
        if (enemyController.IfWithinAttackRange())
        {
            return StateType.Attack;
        }
        //If player is outside the detect range, transite to Patrol state or idle
        else if (!enemyController.IfWithinDetectRange())
        {
            if(enemyController.GetPatrolPointsCount() != 0)
            {
                return StateType.Patrol;
            }
            else
            {
                return StateType.Idle;
            }
        }
        //Otherwise moving to player automaticly
        else
        {
            return StateType.MoveToPlayer;
        }
    }

}

public class EAttackState: IState
{
    private EnemyStateManager E_Manager;
    private Enemy enemyController;
    public EAttackState(EnemyStateManager manager, Enemy enemyController)
    {
        this.E_Manager = manager;
        this.enemyController = enemyController;
    }
    public void OnEnter()
    {
        Debug.Log("Attack!!!");
    }
    public void OnExit()
    {
        Debug.Log("I need to get closer");
    }
    public void OnUpdate()
    {
        //Do attack action

    }
    public StateType ExitConditions()
    {
        //If player is within the attack range, transite to MoveToPlayer state
        if (enemyController.IfWithinAttackRange())
        {
            return StateType.Attack;
        }
        //If player is within the detect range but outside attack range, transite to MoveToPlayer state
        else if (enemyController.IfWithinDetectRange())
        {
            return StateType.MoveToPlayer;
        }
        else
        {
            return StateType.Idle;
        }
    }

}

public class EHitbackState: IState
{
    private EnemyStateManager E_Manager;
    private Enemy enemyController;
    public EHitbackState(EnemyStateManager manager,Enemy ec)
    {
        this.E_Manager = manager;
        this.enemyController= ec;
    }
    public void OnEnter()
    {
        enemyController.EnterHitback();
    }
    public void OnExit()
    {
        enemyController.StopHitback();
    }
    public void OnUpdate()
    {
        enemyController.Hitback();
    }

    public StateType ExitConditions()
    {
        //If still hitback
        if (enemyController.IfHitback())
        {
            return StateType.Hitback;
        }
        //If player is within the attack range, transite to Attack state
        else if (enemyController.IfWithinAttackRange())
        {
            return StateType.Attack;
        }
        //If player is within the detect range but outside attack range, transite to MoveToPlayer state
        else if (enemyController.IfWithinDetectRange())
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

public class EDieState: IState
{
    private EnemyStateManager E_Manager;
    private Enemy enemyController;
    public EDieState(EnemyStateManager manager, Enemy enemyController)
    {
        this.E_Manager = manager;
        this.enemyController = enemyController;
    }
    public void OnEnter()
    {
        enemyController.EnemyDead();
    }
    public void OnExit()
    {
        Debug.Log("I'm dead");
    }
    public void OnUpdate()
    {
        //Do nothing
    }
    public StateType ExitConditions()
    {
        return StateType.Die;
    }
}