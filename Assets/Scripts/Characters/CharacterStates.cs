using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PIdleState : IState
{
    private CharacterStateManager P_Manager;

    public PIdleState(CharacterStateManager manager) { this.P_Manager = manager; }

    public void OnEnter()
    {
        Debug.LogWarning("Idle now");
    }
    public void OnExit()
    {
        Debug.LogWarning("Exit Idle State");
        //Stop playing Idle Animation
    }
    public void OnUpdate()
    {
        float y = Input.GetAxis("Vertical");
        float x = Input.GetAxis("Horizontal");
        if (y != 0 || x != 0)
        {
            P_Manager.TransiteToState(StateType.Move);
        }
    }
}
public class PMoveState : IState
{
    private CharacterStateManager P_Manager;
    public PMoveState(CharacterStateManager manager) { this.P_Manager = manager; }
    public void OnEnter()
    {
        Debug.LogWarning("Start Moving");
    }
    public void OnExit()
    {
        Debug.LogWarning("Stop Moving");
    }
    public void OnUpdate()
    {
        float y = Input.GetAxis("Vertical");
        float x = Input.GetAxis("Horizontal");
        if (y != 0 || x != 0)
        {
            P_Manager.MoveToTargetPoint(new Vector2(x, y));
        }
        else
        {
            P_Manager.TransiteToState(StateType.Idle);
        }
    }
}


