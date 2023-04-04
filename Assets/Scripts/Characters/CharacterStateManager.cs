using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateManager : FSM_Manager
{
    //Basic components and variables for the character
    private Rigidbody2D rb;
    private int currentHP;


    [SerializeField] private int baseHP;
    [SerializeField] private float baseSpeed;

    protected override void Other_Init()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHP = baseHP;
    }

    protected override void Init_States()
    {
        states = new Dictionary<StateType, IState>();
        states.Add(StateType.Idle, new PIdleState(this));
        states.Add(StateType.Move, new PMoveState(this));
        currentStateType= StateType.Idle;
        currentState = states[StateType.Idle];
    }
    public void MoveToTargetPoint(Vector2 dir)
    {
        Vector2 pos = gameObject.transform.position;
        pos += dir.normalized * baseSpeed * Time.fixedDeltaTime;
        rb.MovePosition(pos);

    }
}
