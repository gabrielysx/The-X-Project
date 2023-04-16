using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //Basic components and variables for the enemy
    private EnemyStateManager ESM;
    private Rigidbody2D rb;
    [SerializeField]private int currentPatrolPointIndex = 0;
    [SerializeField]private Vector2 nextTargetPosition;
    private int currentHP;
    private GameObject player;
    private bool isHitback;
    private float hitbackTimer;
    private Vector2 hitbackDir;
    private float hitbackForce;
    private float hitbackDuration;

    [SerializeField] private List<GameObject> patrolPoints = new List<GameObject>();
    public int GetPatrolPointsCount()
    {
        return patrolPoints.Count;
    }
    [SerializeField] private int detectRange = 3;
    [SerializeField] private int attackRange = 1;
    [SerializeField] private int baseHP;
    [SerializeField] private float baseSpeed;
    [SerializeField] private float DisThreshold = 0.1f; //The threshold to determine whether the enemy has reached the point

    // Start is called before the first frame update
    void Start()
    {
        ESM= GetComponent<EnemyStateManager>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        currentHP = baseHP;
    }

    // Update is called once per frame
    void Update()
    {
        
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
        if (PrevPatrolIndex == 0)
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
            if (patrolPoints.IndexOf(point) != PrevPatrolIndex)
            {
                if (Vector2.Distance(transform.position, point.transform.position) <= minDis)
                {
                    minDis = Vector2.Distance(transform.position, point.transform.position);
                    minIndex = patrolPoints.IndexOf(point);
                }
            }
        }
        currentPatrolPointIndex = minIndex;
        nextTargetPosition = patrolPoints[currentPatrolPointIndex].transform.position;
    }

    public void PatrolAlongRoute()
    {
        if (patrolPoints.Count == 0)
        {
            Debug.LogError("No patrol points assigned");
            return;
        }
        if (Vector2.Distance(transform.position, nextTargetPosition) < DisThreshold)
        {
            currentPatrolPointIndex = (currentPatrolPointIndex + 1) % patrolPoints.Count;
            nextTargetPosition = patrolPoints[currentPatrolPointIndex].transform.position;
        }
        MoveTowardsTargetPosition(nextTargetPosition);
    }

    public void MoveTowardsTargetPosition(Vector2 targetPos)
    {
        Vector2 currentPos = gameObject.transform.position;
        Vector2 step = baseSpeed * Time.fixedDeltaTime * (targetPos - currentPos).normalized;
        if (step.magnitude >= (targetPos - currentPos).magnitude)
        {
            rb.MovePosition(targetPos);
        }
        else
        {
            rb.MovePosition(currentPos + step);
        }
    }

    public void MovetoPlayer()
    {
        MoveTowardsTargetPosition(player.transform.position);
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

    public void EnterHitback()
    {
        rb.AddForce(hitbackDir * hitbackForce, ForceMode2D.Impulse);
        isHitback = true;
    }

    public void Hitback()
    {
        hitbackTimer += Time.deltaTime;
        if(hitbackTimer >= hitbackDuration)
        {
            isHitback = false;
        }
    }
    public void StopHitback()
    {
        //rb.velocity = Vector2.zero;
    }

    public void EnemyDead()
    {
        Destroy(gameObject);
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
            ESM.ChangeState(StateType.Hitback);
            hitbackTimer = 0;
            hitbackDir = collision.gameObject.transform.GetComponent<ProjectileBase>().flyDir;
            hitbackForce = 2f;
            hitbackDuration = 0.2f;
        }
    }
}
