using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class PIdleState : IState
{
    private CharacterStateManager P_Manager;
    private Character characterController;

    public PIdleState(CharacterStateManager manager,Character cc) 
    { 
        this.P_Manager = manager;
        this.characterController = cc;
    }

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
        //Do nothing
        
    }
    public StateType ExitConditions()
    {
        if(characterController.IfMoving())
        {
            return StateType.Move;
        }
        else
        {
            //Stay in Idle state
            return StateType.Idle;
        }
        
    }
}
public class PMoveState : IState
{
    private CharacterStateManager P_Manager;
    private Character characterController;
    public PMoveState(CharacterStateManager manager, Character cc)
    {
        this.P_Manager = manager;
        this.characterController = cc;
    }
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

        characterController.MoveToTargetPoint();
    }

    public StateType ExitConditions()
    {
        if (characterController.IfMoving())
        {
            //Stay in Move state
            return StateType.Move;
        }
        else
        {
            return StateType.Idle;
        }
    }

}

public class PDashState: IState
{
    private CharacterStateManager P_Manager;
    private Character characterController;
    public PDashState(CharacterStateManager manager, Character cc)
    {
        this.P_Manager = manager;
        this.characterController = cc;
    }
    public void OnEnter()
    {
        characterController.EnterDash();
    }
    public void OnExit()
    {
        Debug.LogWarning("Stop Dashing");
    }
    public void OnUpdate()
    {
        characterController.Dash();
    }
    public StateType ExitConditions()
    {
        if (characterController.IfDashing())
        {
            //Stay in Move state
            return StateType.Dash;
        }
        else
        {
            return StateType.Idle;
        }
    }
}


