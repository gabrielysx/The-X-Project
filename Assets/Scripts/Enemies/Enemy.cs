using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //Basic components and variables for the enemy
    [SerializeField] protected Animator animator;
    protected FSM_Manager ESM;
    protected Rigidbody2D rb;
    protected SpriteRenderer sr;
    protected GameObject player;
    [SerializeField]protected int currentHP;
    protected bool isDead;
    
    [SerializeField] protected bool isFlee;
    protected bool isFleeing;
    //Hitback
    protected bool isHitback;
    protected float hitbackTimer;
    protected Vector2 hitbackDir;
    protected float hitbackForce;
    protected float hitbackDuration,hitbackStunDur;
    protected float hitbackCurSpeed;
    //Wayfinding
    protected Vector2 nextTargetPosition;
    protected List<PathNodeBase> pathDebug = new List<PathNodeBase>();
    protected List<PathNodeBase> fleePathDebug = new List<PathNodeBase>();
    [SerializeField] protected float DisThreshold = 0.1f; //The threshold to determine whether the enemy has reached the point
    //Patrol
    [SerializeField] protected List<GameObject> patrolPoints = new List<GameObject>();
    [SerializeField] protected int currentPatrolPointIndex = 0;
    public int GetPatrolPointsCount()
    {
        return patrolPoints.Count;
    }

    [SerializeField] protected int detectRange = 3;
    [SerializeField] protected int attackRange = 1;
    [SerializeField] protected int baseHP = 5;
    [SerializeField] protected float baseSpeed = 2f;

    //loot drops
    [SerializeField] protected List<LootItem> lootDropsInfo;
    [SerializeField] protected GameObject lootDropPrefab;
    [SerializeField] protected GameObject goldCoinPrefab;
    [SerializeField] protected int goldDropAmount;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        ESM= GetComponent<FSM_Manager>();
        rb = GetComponent<Rigidbody2D>();
        sr= GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");
        currentHP = baseHP;
    }

    // Update is called once per frame
    void Update()
    {
        if(isFlee)
        {
            if (!isFleeing)
            {
                ESM.ChangeState(StateType.Flee);
                isFleeing = true;
            }
        }
        else
        {
            if (isFleeing)
            {
                ESM.ChangeState(StateType.Patrol);
                isFleeing = false;
            }
            
        }
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        PathFinder pfdebug = PathFinder.instance;
        //path debug
        if (pathDebug != null)
        {
            if (pathDebug.Count > 0)

            {
                Vector3 prev = pfdebug.GridToWorldPos(pathDebug[0].gridPosition);
                
                foreach (PathNodeBase node in pathDebug)
                {
                    Vector3 cur = pfdebug.GridToWorldPos(node.gridPosition);
                    Gizmos.color = Color.green;
                    Gizmos.DrawLine(prev, cur);
                    prev = cur;
                }
            }
        }
        //flee debug
        if (fleePathDebug != null)
        {
            if (fleePathDebug.Count > 0)

            {
                Vector3 prev = pfdebug.GridToWorldPos(fleePathDebug[0].gridPosition);

                foreach (PathNodeBase node in fleePathDebug)
                {
                    Vector3 cur = pfdebug.GridToWorldPos(node.gridPosition);
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawLine(prev, cur);
                    prev = cur;
                }
            }
        }

    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerBullet"))
        {
            TakeHit(collision.gameObject.transform.GetComponent<ProjectileBase>().flyDir, 10f, 0.4f, 0.1f);
        }
    }
    
    //Patrol
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
        Vector2 nextWaypoint = GetNextWayPoint(nextTargetPosition);
        MoveTowardsTargetPosition(nextWaypoint,baseSpeed);
    }
    
    //Movement
    public void MoveTowardsTargetPosition(Vector2 targetPos, float speed)
    {
        Vector2 currentPos = gameObject.transform.position;
        Vector2 step = speed * Time.fixedDeltaTime * (targetPos - currentPos).normalized;
        //set the slime face towards moving direction
        FaceToDirection(step.x);
        //When close to the target position, let the enemy stop at the target position
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
        Vector2 nextWaypoint = GetNextWayPoint(player.transform.position);
        MoveTowardsTargetPosition(nextWaypoint,baseSpeed);
    }

    //Detection
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

    //Hitback methods
    public bool IfHitback()
    {
        return isHitback;
    }

    public void TakeHit(Vector2 dir, float force, float duration, float stunDur)
    {
        isHitback = true;
        ESM.ChangeState(StateType.Hitback);
        hitbackTimer = 0;

        hitbackDir = dir.normalized;
        hitbackForce = force;
        hitbackDuration = duration;
        hitbackStunDur = stunDur;
        TakeDamage(1);
    }
    public void EnterHitback()
    {
        Debug.Log("Enter hitback");
        sr.material.SetFloat("_FlashAmount", 1);
        if (animator != null)
            animator.SetTrigger("EnterHit");
        hitbackCurSpeed = hitbackForce;
    }

    public void Hitback()
    {
        hitbackTimer += Time.fixedDeltaTime;
        //setup hitback effect
        float factor = Mathf.Lerp(sr.material.GetFloat("_FlashAmount"), 0, hitbackDuration);
        sr.material.SetFloat("_FlashAmount", factor);
        //set hiback process
        hitbackCurSpeed = Mathf.Lerp(hitbackCurSpeed, 0, Time.fixedDeltaTime * 1.2f);
        if (hitbackTimer < hitbackDuration)
        {
            rb.MovePosition((Vector2)transform.position + hitbackCurSpeed * hitbackDir.normalized * Time.fixedDeltaTime);
        }
        //when hitback is over stun for a while
        else if (hitbackTimer >= hitbackDuration)
        {
            rb.velocity = Vector2.zero;
        }
        //After stun, stop hitback process
        if(hitbackTimer >= hitbackDuration + hitbackStunDur)
        {
            isHitback = false;
        }
    }
    public void StopHitback()
    {
        //things happen when hitback is over
        sr.material.SetFloat("_FlashAmount", 0);
        if (animator != null)
            animator.SetTrigger("ExitHit");
        hitbackCurSpeed = 0;
        Debug.Log("Hitback is over");
    }

    //Dead
    public void EnterDead()
    {
        List<Collider2D> allColliders = gameObject.GetComponents<Collider2D>().ToList();
        foreach (Collider2D col in allColliders)
        {
            col.enabled = false;
        }
        if (animator != null)
        {
            SpawnDrops();
            animator.SetTrigger("EnterDie");
        }
        else
        {
            SpawnDrops();
            EnemyDead();
        }
    }

    public void EnemyDead()
    {
        gameObject.SetActive(false);
        Destroy(gameObject);
    }

    public void SpawnDrops()
    {
        foreach(LootItem item in lootDropsInfo)
        {
           GameObject loot = Instantiate(lootDropPrefab, transform.position, new Quaternion(0, 0, 0, 0), GameManager.instance.lootHolder.transform);
           loot.GetComponent<Loot>().SetInitCondition(item, 1);
        }
        for(int i = 0; i < goldDropAmount; i++)
        {
            GameObject coin = Instantiate(goldCoinPrefab, transform.position, new Quaternion(0, 0, 0, 0), GameManager.instance.lootHolder.transform);
            coin.GetComponent<GoldCoin>().SetCoinInitCondition();
        }
    }

    public void TakeDamage(int damage)
    {
        if (currentHP <= 0)
        {
            isDead = true;
            Debug.Log("Change to Die State!!");
            ESM.ChangeState(StateType.Die);
        }
        else
        {
            currentHP -= damage;
        }
    }

    //Flee

    public bool IfFleeing()
    {
        return isFleeing;
    }

    public void FleeAway()
    {
        if(Vector2.Distance(transform.position,player.transform.position) > 5)
        {
            return;
        }
        Vector2 nextWaypoint = GetNextFleeWayPoint();
        MoveTowardsTargetPosition(nextWaypoint, baseSpeed);
    }


    
    //apearance
    public void FaceToDirection(float xValue)
    {
        if (xValue > 0)
        {
            sr.flipX = false;
        }
        else if (xValue < 0)
        {
            sr.flipX = true;
        }
    }

    public void SetAnimatorMoving(bool moving)
    {
        if (animator != null)
        {
            animator.SetBool("isMoving", moving);
        }
    }

    //Navigation
    public Vector2 GetNextFleeWayPoint()
    {
        PathFinder pf = PathFinder.instance;
        Vector3 start = transform.position;
        Vector2 fleepoint = pf.FindFleePoint(transform.position, player.transform.position, 5f);
        List<PathNodeBase> nodes = pf.FindPath(pf.WorldToGridPos(start), pf.WorldToGridPos(fleepoint));
        fleePathDebug = nodes;
        if (nodes == null)
        {
            Debug.LogWarning("No valid fleepoint, stay the same position");
            return start;
        }
        else if (nodes.Count == 1)
        {
            //Current position is in the same grid of the target position
            Debug.LogWarning("Reach the same grid, move to flee point now");
            return fleepoint;
        }
        else
        {
            return pf.GridToWorldPos(nodes[1].gridPosition);
        }
    }
    public Vector2 GetNextWayPoint(Vector2 targetPos)
    {
        List<PathNodeBase> nodes = new List<PathNodeBase>();
        PathFinder pf = PathFinder.instance;
        Vector3 start = transform.position;
        nodes = pf.FindPath(pf.WorldToGridPos(start), pf.WorldToGridPos(targetPos));
        pathDebug = nodes;
        if (nodes == null)
        {
            Debug.Log("Unreachable position, stay the same position");
            return start;
        }
        else if (nodes.Count == 1)
        {
            //Current position is in the same grid of the target position
            Debug.Log("Reach the same grid, move to target now");
            return targetPos;
        }
        else
        {
            return pf.GridToWorldPos(nodes[1].gridPosition);
        }
    }

}
