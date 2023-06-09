using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;


public interface IState
{
    void OnEnter();
    void OnUpdate();
    void OnExit();
    StateType ExitConditions();
}
public enum StateType
{
    Idle, Patrol, Attack, MoveToPlayer, Move, Hitback, Die, Dash, Flee
}

public class DefaultIdle : IState
{
    public void OnEnter()
    {

    }

    public void OnExit()
    {

    }

    public void OnUpdate()
    {

    }

    public StateType ExitConditions()
    {
        return StateType.Idle;
    }
}

public class FSM_Manager : MonoBehaviour
{
    protected IState currentState;
    private StateType curStateType,nextStateType;


    protected virtual void FixedUpdate()
    {
        RunCurrentState();
    }

    protected virtual void Update()
    {
        
    }

    /// <summary>
    /// The base method to transite to specified state
    /// </summary>
    /// <param name="NewState">specified state</param>
    public virtual void ChangeState(StateType newStateType)
    {
        nextStateType = newStateType;
    }

    protected void RunCurrentState()
    {
        //When state changed, exit current state and enter new state
        if(nextStateType != curStateType)
        {
            currentState.OnExit();
            curStateType = nextStateType;
            currentState = SwitchBetweenStates(curStateType);
            currentState.OnEnter();
        }

        //Check the state conditions and get the next state
        nextStateType = currentState.ExitConditions();

        if (nextStateType != curStateType)
        {
            return;
        }
        else
        {
            currentState.OnUpdate();
        }
    }

   protected virtual IState SwitchBetweenStates(StateType typeName)
    {
        //Switch the state
        switch (typeName)
        {
            default:
                return new DefaultIdle();
        }
    }

}