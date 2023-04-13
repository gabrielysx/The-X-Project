using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IState
{
    void OnEnter();
    void OnUpdate();
    void OnExit();
}
public enum StateType
{
    Idle, Patrol, Attack, MoveToPlayer, Move, Hitback, Die
}

public class FSM_Manager : MonoBehaviour
{
    public IState currentState;
    //public List<IState> states;
    public StateType currentStateType;
    public Dictionary<StateType, IState> states = new Dictionary<StateType, IState>();

    protected virtual void Start()
    {
        Init_States();
        Other_Init();
    }

    protected virtual void Update()
    {
        currentState.OnUpdate();
    }

    protected virtual void Init_States()
    {
        //Add all the states needed here
    }

    protected virtual void Other_Init()
    {
        //Add all other actions that needed in Start() phase
    }


    /// <summary>
    /// The base method to transite to specified state
    /// </summary>
    /// <param name="newState">specified state</param>
    public virtual void TransiteToState(StateType stateType)
    {
        if (currentState != null)
        {
            currentState.OnExit();
            if (states.ContainsKey(stateType) == false)
            {
                Debug.LogError("The state type is not in the dictionary");
                //Reenter the current state
                currentState.OnEnter();
                return;
            }
        }
        else
        {
            Debug.LogError("No valid current state");
            return;
        }
        
        currentStateType = stateType;
        currentState = states[stateType];
        currentState.OnEnter();
    }

}