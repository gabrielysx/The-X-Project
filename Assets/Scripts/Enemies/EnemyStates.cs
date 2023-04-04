using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

public class EIdleState : IState
{
    private EnemyStateManager E_Manager;

    public EIdleState(EnemyStateManager manager)
    {
        this.E_Manager = manager;
    }

    public void OnEnter()
    {
        //Play Idle Animation
    }

    public void OnExit()
    {
        Debug.Log("Exit Idle State");
        //Stop playing Idle Animation
    }

    public void OnUpdate()
    {
        //Keep playing Idle Animation
        //If player is within the attack range, transite to Attack state
        if (Vector2.Distance(E_Manager.player.transform.position, E_Manager.transform.position) <= E_Manager.attackRange)
        {
            E_Manager.TransiteToState(StateType.Attack);
        }
        //If player is within the detect range, transite to MoveToPlayer state
        else if (Vector2.Distance(E_Manager.player.transform.position, E_Manager.transform.position) <= E_Manager.detectRange)
        {
            E_Manager.TransiteToState(StateType.MoveToPlayer);
        }
        //Otherwise patrol automaticly
        else if (E_Manager.patrolPoints != null)
        {
            E_Manager.TransiteToState(StateType.Patrol);
        }
    }
}

public class EPatrolState : IState
{
    private EnemyStateManager E_Manager;

    public EPatrolState(EnemyStateManager manager)
    {
        this.E_Manager = manager;
    }

    public void OnEnter()
    {
        //calculate which patrol point is the closest to the player
        float minDis = Vector2.Distance(E_Manager.transform.position, E_Manager.patrolPoints[0].transform.position);
        int minIndex = 0;
        foreach (GameObject point in E_Manager.patrolPoints)
        {
            if (Vector2.Distance(E_Manager.transform.position, point.transform.position) < minDis)
            {
                minDis = Vector2.Distance(E_Manager.transform.position, point.transform.position);
                minIndex = E_Manager.patrolPoints.IndexOf(point);
            }
        }
        E_Manager.currentPatrolPoint = minIndex;

    }

    public void OnExit()
    {
        Debug.Log("Exit Patrol State");
    }

    public void OnUpdate()
    {
        //If player is within the attack range, transite to Attack state
        if (Vector2.Distance(E_Manager.player.transform.position, E_Manager.transform.position) <= E_Manager.attackRange)
        {
            E_Manager.TransiteToState(StateType.Attack);
        }
        //If player is within the detect range, transite to MoveToPlayer state
        else if (Vector2.Distance(E_Manager.player.transform.position, E_Manager.transform.position) <= E_Manager.detectRange)
        {
            E_Manager.TransiteToState(StateType.MoveToPlayer);
        }

        //If the enemy is close enough to the current patrol point, move to the next patrol point
        if (Vector2.Distance(E_Manager.transform.position, E_Manager.patrolPoints[E_Manager.currentPatrolPoint].transform.position) < 0.01f)
        {
               E_Manager.currentPatrolPoint = (E_Manager.currentPatrolPoint + 1) % E_Manager.patrolPoints.Count;
        }
        //Get direction to the patrol point and move to it
        E_Manager.MoveToTargetPoint(E_Manager.patrolPoints[E_Manager.currentPatrolPoint].transform.position - E_Manager.transform.position);
    }

}

public class EMoveToPlayerState: IState
{
    private EnemyStateManager E_Manager;
    public EMoveToPlayerState(EnemyStateManager manager)
    {
        this.E_Manager = manager;
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
        //If player is within the attack range, transite to Attack state
        if (Vector2.Distance(E_Manager.player.transform.position, E_Manager.transform.position) <= E_Manager.attackRange)
        {
            E_Manager.TransiteToState(StateType.Attack);
        }
        //If player is outside the detect range, transite to Patrol state
        else if (Vector2.Distance(E_Manager.player.transform.position, E_Manager.transform.position) > E_Manager.detectRange)
        {
            E_Manager.TransiteToState(StateType.Patrol);
        }
        //Get direction to the player and move to him
        E_Manager.MoveToTargetPoint(E_Manager.player.transform.position- E_Manager.transform.position);
    }
}

public class EAttackState: IState
{
    private EnemyStateManager E_Manager;
    public EAttackState(EnemyStateManager manager)
    {
        this.E_Manager = manager;
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

        //If player is outside the attack range, transite to MoveToPlayer state
        if (Vector2.Distance(E_Manager.player.transform.position, E_Manager.transform.position) > E_Manager.attackRange)
        {
            E_Manager.TransiteToState(StateType.MoveToPlayer);
        }
    }

}