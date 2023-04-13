using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateManager : FSM_Manager
{
    //Basic components and variables for the enemy
    private Rigidbody2D rb;
    private int currentPatrolPointIndex = 0;
    private Vector2 nextTargetPosition;
    private int currentHP;
    private GameObject player;
    private bool isHitback;
    private float hitbackTimer;
    private Vector2 hitbackDir;
    private float hitbackForce;
    private float hitbackDuration;

    [SerializeField] private int detectRange = 3;
    [SerializeField] private int attackRange = 1;
    [SerializeField] private List<GameObject> patrolPoints = new List<GameObject>();
    [SerializeField] private int baseHP;
    [SerializeField] private float baseSpeed;
    [SerializeField] private float DisThreshold = 0.1f; //The threshold to determine whether the enemy has reached the point



    protected override void Other_Init()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        currentHP = baseHP;
    }

    protected override void Init_States()
    {
        states = new Dictionary<StateType, IState>();
        states.Add(StateType.Idle, new EIdleState(this,patrolPoints.Count));
        states.Add(StateType.Patrol, new EPatrolState(this));
        states.Add(StateType.MoveToPlayer, new EMoveToPlayerState(this,patrolPoints.Count));
        states.Add(StateType.Attack, new EAttackState(this));
        states.Add(StateType.Hitback, new EHitbackState(this, patrolPoints.Count));
        states.Add(StateType.Die, new EDieState(this));
        currentStateType = StateType.Idle;
        currentState = states[StateType.Idle];
    }

    /// <summary>
    /// The method to move to the next updated point towards the specified position 
    /// </summary>
    /// <param name="TargetPosition">specified position</param>
    public void MoveTowardsTargetPosition(Vector2 targetPos)
    {
        Vector2 currentPos = gameObject.transform.position;
        rb.MovePosition(currentPos + baseSpeed * Time.fixedDeltaTime * (targetPos - currentPos).normalized);
    }

    public void EnterPatrol()
    {
        //Special condition
        if (patrolPoints.Count == 0)
        {
            Debug.LogError("No patrol points assigned");
            return;
        }
        if (patrolPoints.Count == 1)
        {
            nextTargetPosition = patrolPoints[0].transform.position;
            return;
        }

        //Set initial min value
        int PrevPatrolIndex = (currentPatrolPointIndex - 1) % patrolPoints.Count;
        float minDis = 0f;
        int minIndex = 0;
        if (PrevPatrolIndex == 0 )
        {
            minDis = Vector2.Distance(transform.position, patrolPoints[1].transform.position);
            minIndex = 1;
        }
        else
        {
            minDis = Vector2.Distance(transform.position, patrolPoints[0].transform.position);
            minIndex = 0;
        }
        //Calculate which next patrol point is the closest
        foreach (GameObject point in patrolPoints)
        {
            if(patrolPoints.IndexOf(point) != PrevPatrolIndex)
            {
                if (Vector2.Distance(transform.position, point.transform.position) <= minDis)
                {
                    minDis = Vector2.Distance(transform.position, point.transform.position);
                    minIndex = patrolPoints.IndexOf(point);
                }
            }
        }
        currentPatrolPointIndex = minIndex;
    }

    public void PatrolAlongRoute()
    {
        if (patrolPoints.Count == 0)
        {
            Debug.LogError("No patrol points assigned");
            return;
        }
        if (Vector2.Distance(transform.position, patrolPoints[currentPatrolPointIndex].transform.position) < DisThreshold)
        {
            currentPatrolPointIndex = (currentPatrolPointIndex + 1) % patrolPoints.Count;
            nextTargetPosition = patrolPoints[currentPatrolPointIndex].transform.position;
        }
        MoveTowardsTargetPosition(nextTargetPosition);
    }

    public void MovetoPlayer()
    {
        MoveTowardsTargetPosition(player.transform.position);
    }

    public void EnterHitback()
    {
        
    }

    public void Hitback()
    {
        hitbackTimer += Time.fixedDeltaTime;
        if (hitbackTimer >= hitbackDuration)
        {
            isHitback = false;
        }
        rb.AddForce(hitbackDir * hitbackForce);
    }
    public void StopHitback()
    {
        rb.velocity = Vector3.zero;
    }

    public void EnemyDead()
    {
        Destroy(gameObject);
    }

    public bool IfWithinDetectRange()
    {
        float dis = Vector2.Distance(player.transform.position, transform.position);
        return (dis <= detectRange);
    }

    public bool IfWithinAttackRange()
    {
        float dis = Vector2.Distance(player.transform.position, transform.position);
        return (dis <= attackRange);
    }

    public bool IfHitback()
    {
        return isHitback;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerBullet"))
        {
            isHitback = true;
            hitbackTimer = 0;
            hitbackDir = collision.gameObject.transform.GetComponent<ProjectileBase>().flyDir;
            hitbackForce = 10f;
            hitbackDuration = 0.2f;
        }
    }

}
