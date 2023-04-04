using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateManager : FSM_Manager
{
    //Basic components and variables for the enemy
    private Rigidbody2D rb;
    public int currentPatrolPoint = 0;
    private int currentHP;
    public GameObject player;

    public int detectRange = 3;
    public int attackRange = 1;
    public List<GameObject> patrolPoints = new List<GameObject>();

    [SerializeField] private int baseHP;
    [SerializeField] private float baseSpeed;


    

    protected override void Other_Init()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        currentHP = baseHP;
    }

    protected override void Init_States()
    {
        states = new Dictionary<StateType, IState>();
        states.Add(StateType.Idle, new EIdleState(this));
        states.Add(StateType.Patrol, new EPatrolState(this));
        states.Add(StateType.MoveToPlayer, new EMoveToPlayerState(this));
        states.Add(StateType.Attack, new EAttackState(this));
        currentStateType = StateType.Idle;
        currentState = states[StateType.Idle];
    }

    public void MoveToTargetPoint(Vector2 dir)
    {
        Vector2 pos = gameObject.transform.position;
        pos += dir.normalized * baseSpeed * Time.fixedDeltaTime;
        rb.MovePosition(pos);
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

}
