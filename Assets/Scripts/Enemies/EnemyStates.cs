using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

public class EIdleState : IState
{
    private EnemyStateManager E_Manager;
    private int patrolPointsCount;

    public EIdleState(EnemyStateManager manager, int patrolPointsCount)
    {
        this.E_Manager = manager;
        this.patrolPointsCount = patrolPointsCount;
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
        //If got hit back
        if (E_Manager.IfHitback())
        {
            E_Manager.TransiteToState(StateType.Hitback);
            return;
        }
        //If player is within the attack range, transite to Attack state
        else if (E_Manager.IfWithinAttackRange())
        {
            E_Manager.TransiteToState(StateType.Attack);
            return;
        }
        //If player is within the detect range, transite to MoveToPlayer state
        else if (E_Manager.IfWithinDetectRange())
        {
            E_Manager.TransiteToState(StateType.MoveToPlayer);
            return;
        }
        
        //Otherwise patrol automaticly
        else if (patrolPointsCount != 0)
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
        E_Manager.EnterPatrol();

    }

    public void OnExit()
    {
        Debug.Log("Exit Patrol State");
    }

    public void OnUpdate()
    {
        //If got hit back
        if (E_Manager.IfHitback())
        {
            E_Manager.TransiteToState(StateType.Hitback);
            return;
        }
        //If player is within the attack range, transite to Attack state
        if (E_Manager.IfWithinAttackRange())
        {
            E_Manager.TransiteToState(StateType.Attack);
            return;
        }
        //If player is within the detect range, transite to MoveToPlayer state
        else if (E_Manager.IfWithinDetectRange())
        {
            E_Manager.TransiteToState(StateType.MoveToPlayer);
            return;
        }

        E_Manager.PatrolAlongRoute();
        
    }

}

public class EMoveToPlayerState: IState
{
    private EnemyStateManager E_Manager;
    private int patrolPointsCount;
    public EMoveToPlayerState(EnemyStateManager manager, int patrolPointsCount)
    {
        this.E_Manager = manager;
        this.patrolPointsCount = patrolPointsCount;
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
        //If got hit back
        if (E_Manager.IfHitback())
        {
            E_Manager.TransiteToState(StateType.Hitback);
            return;
        }
        //If player is within the attack range, transite to Attack state
        if (E_Manager.IfWithinAttackRange())
        {
            E_Manager.TransiteToState(StateType.Attack);
            return;
        }
        //If player is outside the detect range, transite to Patrol state
        else if (!E_Manager.IfWithinDetectRange())
        {
            if (patrolPointsCount != 0)
            {
                E_Manager.TransiteToState(StateType.Patrol);
                return;
            }
            else
            {
                E_Manager.TransiteToState(StateType.Idle);
                return;
            }
        }
        //Get position of the player and move to him
        E_Manager.MovetoPlayer();
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


        //If got hit back
        if (E_Manager.IfHitback())
        {
            E_Manager.TransiteToState(StateType.Hitback);
            return;
        }
        //If player is outside the attack range, transite to MoveToPlayer state
        if (!E_Manager.IfWithinAttackRange())
        {
            E_Manager.TransiteToState(StateType.MoveToPlayer);
        }
    }

}

public class EHitbackState: IState
{
    private EnemyStateManager E_Manager;
    private int patrolPointsCount;
    public EHitbackState(EnemyStateManager manager, int patrolPointsCount)
    {
        this.E_Manager = manager;
        this.patrolPointsCount = patrolPointsCount;
    }
    public void OnEnter()
    {
        E_Manager.EnterHitback();
    }
    public void OnExit()
    {
        E_Manager.StopHitback();
    }
    public void OnUpdate()
    {
        //If still hitback
        if (E_Manager.IfHitback())
        {
            E_Manager.Hitback();
            return;
        }

        //If player is within the attack range, transite to Attack state
        if (E_Manager.IfWithinAttackRange())
        {
            E_Manager.TransiteToState(StateType.Attack);
            return;
        }
        //If player is within the detect range, transite to MoveToPlayer state
        else if (E_Manager.IfWithinDetectRange())
        {
            E_Manager.TransiteToState(StateType.MoveToPlayer);
            return;
        }
        //If player is outside the detect range, transite to Patrol state
        else if (!E_Manager.IfWithinDetectRange())
        {
            if (patrolPointsCount != 0)
            {
                E_Manager.TransiteToState(StateType.Patrol);
                return;
            }
            else
            {
                E_Manager.TransiteToState(StateType.Idle);
                return;
            }
        }
    }
}

public class EDieState: IState
{
    private EnemyStateManager E_Manager;
    public EDieState(EnemyStateManager manager)
    {
        this.E_Manager = manager;
    }
    public void OnEnter()
    {
        E_Manager.EnemyDead();
    }
    public void OnExit()
    {
        Debug.Log("I'm dead");
    }
    public void OnUpdate()
    {
        //Do nothing
    }
}