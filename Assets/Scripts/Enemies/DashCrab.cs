using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashCrab : Enemy
{
    private bool isDashing;
    private float dashTimer;
    private Vector2 dashDirection;
    [SerializeField] private float dashSpeed, dashAimTime, dashTime, dashEndTime;
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
    }

    public void Dash()
    {
        dashTimer += Time.fixedDeltaTime;
        //Aiming for dash
        if (dashTimer < dashAimTime)
        {
            rb.velocity = Vector2.zero;
            dashDirection = (Vector2)(player.transform.position - transform.position);
        }
        //dashing
        else if(dashTimer < dashAimTime + dashTime)
        {
            rb.MovePosition((Vector2)gameObject.transform.position + dashSpeed * Time.fixedDeltaTime * dashDirection.normalized);
        }
        //stop dashing and stun for a while
        else if (dashTimer < dashAimTime + dashTime + dashEndTime)
        {
            rb.velocity = Vector2.zero;
        }
        //finish dashing
        else
        {
            isDashing = false;
        }
    }
}
