using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateManager : FSM_Manager
{
    private Character characterController;
    //public Character GetCharacterController()
    //{
    //    return characterController;
    //}

    private void Start()
    {
        characterController = GetComponent<Character>();
        currentState = SwitchBetweenStates(StateType.Idle);
    }

    protected override IState SwitchBetweenStates(StateType typeName)
    {
        switch (typeName)
        {
            case StateType.Idle:
                return new PIdleState(this,characterController);
            case StateType.Move:
                return new PMoveState(this, characterController);
            default:
                return new PIdleState(this, characterController);
        }
    }


}
