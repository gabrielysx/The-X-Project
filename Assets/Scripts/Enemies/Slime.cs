using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy
{
    private bool isDashing;
    private float dashTimer;
    private Vector2 dashDirection;
    [SerializeField] private float dashSpeed, dashAimTime, dashTime, dashEndTime;
    private float curDashSpeed;
    [SerializeField] private float minRandomMoveInterval, maxRandomMoveInterval, minMoveTime, maxMoveTime;
    private float curMoveInterval ,curMoveTime, moveTimer;
    private Vector2 randomDir;
    private bool isRandomMoving;
    public bool GetIsDashing() { return isDashing; }

    protected override void Start()
    {
        base.Start();
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)dashDirection);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void EnterDash()
    {
        isDashing = true;
        dashTimer = 0;
        if (animator != null)
        {
            animator.SetBool("isDashing", true);
            animator.SetBool("isCharging", true);
        }
        curDashSpeed = dashSpeed;
    }

    public void Dash()
    {
        dashTimer += Time.fixedDeltaTime;
        //Aiming for dash
        if (dashTimer < dashAimTime)
        {
            rb.velocity = Vector2.zero;
            dashDirection = (Vector2)(player.transform.position - transform.position);
            //set the enemy sprite to face the direction of the dash
            FaceToDirection(dashDirection.x);
        }
        //dashing
        else if (dashTimer < dashAimTime + dashTime)
        {
            if (animator != null)
            {
                animator.SetBool("isCharging", false);
            }
            curDashSpeed = Mathf.Lerp(curDashSpeed, 0, 1.2f * Time.fixedDeltaTime);
            rb.MovePosition((Vector2)transform.position + curDashSpeed * Time.fixedDeltaTime * dashDirection.normalized);
        }
        //stop dashing and stun for a while
        else if (dashTimer < dashAimTime + dashTime + dashEndTime)
        {
            if (animator != null)
            {
                animator.SetBool("isDashing", false);
            }
        }
        //finish dashing
        else
        {
            isDashing = false;
        }
    }

    public void ExitDash()
    {
        if (animator != null)
        {
            animator.SetBool("isDashing", false);
            animator.SetBool("isCharging", false);
        }
    }

    public bool GetIsRandomMoving()
    {
        return isRandomMoving;
    }

    public void RandomMove()
    {
        if(moveTimer < curMoveTime)
        {
            moveTimer += Time.fixedDeltaTime;
            rb.MovePosition((Vector2)transform.position + baseSpeed*randomDir*Time.fixedDeltaTime);
        }
        else if(moveTimer < curMoveTime+curMoveInterval) 
        {
            moveTimer += Time.fixedDeltaTime;
            SetAnimatorMoving(false);
        }
        else
        {
            isRandomMoving = false;
        }
    }

    public void EnterRandomMove()
    {
        if (animator != null)
        {
            animator.SetBool("isMoving", true);
        }
        curMoveInterval = Random.Range(minRandomMoveInterval, maxRandomMoveInterval);
        curMoveTime = Random.Range(minMoveTime, maxMoveTime);
        moveTimer = 0;
        float x = Random.Range(-1f, 1f);
        float y = Random.Range(-1f, 1f);
        randomDir = new Vector2(x, y).normalized;
        isRandomMoving = true;
    }


}
