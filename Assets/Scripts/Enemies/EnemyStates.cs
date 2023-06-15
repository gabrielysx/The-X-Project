using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EIdleState : IState
{
    protected FSM_Manager E_Manager;
    protected Enemy enemyController;

    public EIdleState(FSM_Manager manager, Enemy ec)
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
    private FSM_Manager E_Manager;
    private Enemy enemyController;

    public EPatrolState(FSM_Manager manager, Enemy enemyController)
    {
        this.E_Manager = manager;
        this.enemyController = enemyController;
    }

    public void OnEnter()
    {
        enemyController.EnterPatrol();
        enemyController.SetAnimatorMoving(true);
    }

    public void OnExit()
    {
        Debug.Log("Exit Patrol State");
        enemyController.SetAnimatorMoving(false);
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
    private FSM_Manager E_Manager;
    private Enemy enemyController;
    public EMoveToPlayerState(FSM_Manager manager, Enemy ec)
    {
        this.E_Manager = manager;
        this.enemyController= ec;
    }
    public void OnEnter()
    {
        Debug.Log("Player Detected!");
        enemyController.SetAnimatorMoving(true);
    }
    public void OnExit()
    {
        Debug.Log("Lost the Target :(");
        enemyController.SetAnimatorMoving(false);
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
    private FSM_Manager E_Manager;
    private Enemy enemyController;
    public EAttackState(FSM_Manager manager, Enemy enemyController)
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
        //If player is within the attack range, keep attacking
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

public abstract class EHitbackState<T> : IState where T: Enemy
{
    protected FSM_Manager E_Manager;
    protected T enemyController;
    
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

    public virtual StateType ExitConditions()
    {
        //If still hitback
        if (enemyController.IfHitback())
        {
            return StateType.Hitback;
        }
        else if (enemyController.IfFleeing())
        {
            return StateType.Flee;
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
    private FSM_Manager E_Manager;
    private Enemy enemyController;
    public EDieState(FSM_Manager manager, Enemy enemyController)
    {
        this.E_Manager = manager;
        this.enemyController = enemyController;
    }
    public void OnEnter()
    {
        enemyController.EnterDead(); 
        Debug.Log("I'm dead");
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

//Flee State
public class EFleeState : IState
{
    private FSM_Manager E_Manager;
    private Enemy enemyController;

    public EFleeState(FSM_Manager manager, Enemy enemyController)
    {
        this.E_Manager = manager;
        this.enemyController = enemyController;
    }

    public StateType ExitConditions()
    {
        return StateType.Flee;
    }

    public void OnEnter()
    {
        Debug.Log("Enter Fleeing");
        enemyController.SetAnimatorMoving(true);
    }

    public void OnExit()
    {
        Debug.Log("Exit Fleeing");
        enemyController.SetAnimatorMoving(false);
    }

    public void OnUpdate()
    {
        enemyController.FleeAway();
    }
}

