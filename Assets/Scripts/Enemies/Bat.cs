using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : Enemy
{
    private bool isAttacking;
    [SerializeField] private float hoveringTime, chargeTime, dashTime, stunTime;
    private float timer;
    [SerializeField] private float hoverRange, overShootRange;
    [SerializeField] private float dashSpeed;
    private Vector2 dashDir, hoverMoveDir;
    private bool isOverShoot,isReturn;
    private float curDashSpeed;

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, hoverRange);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)dashDir);
    }

    public bool GetIsAttacking() { return isAttacking; }

    public void EnterAttack()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            timer = 0;
            curDashSpeed = dashSpeed;
        }

    }
    public void ExitAttack()
    {
        //reset
        sr.material.SetFloat("_FlashAmount", 0);
        sr.material.SetColor("_FlashColor", Color.white);
    }

    public void OnAttack()
    {
        timer += Time.fixedDeltaTime;
        if (timer < hoveringTime)
        {
            Hover();
        }
        else if (timer < hoveringTime + chargeTime)
        {
            //Set color effect
            float factor = (timer - hoveringTime) / chargeTime;
            if (factor > 0.8) factor = 0.8f;
            sr.material.SetFloat("_FlashAmount", factor);
            sr.material.SetColor("_FlashColor", Color.red);

            dashDir = (Vector2)(player.transform.position - transform.position);
            FaceToDirection(dashDir.x);
        }
        else if (timer < hoveringTime + chargeTime + dashTime)
        {
            Dash();
        }
        else if (timer < hoveringTime + chargeTime + dashTime + stunTime)
        {
            //Stun
            sr.material.SetColor("_FlashColor", Color.black);
        }
        else
        {
            isAttacking = false;
            sr.material.SetFloat("_FlashAmount", 0);
            sr.material.SetColor("_FlashColor", Color.white);
        }
    }

    public void Hover()
    {
        float dis = Vector2.Distance(player.transform.position, transform.position);
        if (isOverShoot && !isReturn)
        {
            if (dis >= hoverRange + overShootRange)
            {
                isOverShoot = false;
                isReturn = true;
            }
            else
            {
                rb.MovePosition((Vector2)transform.position + baseSpeed * Time.fixedDeltaTime * hoverMoveDir.normalized);
            }

        }
        else if(isReturn)
        {
            if(dis >= hoverRange)
            {
                MovetoPlayer();
            }
            else
            {
                isReturn = false;
            }
            
        }
        else if (!isOverShoot && !isReturn)
        {
            if(dis < hoverRange)
            {
                hoverMoveDir = transform.position - player.transform.position;
                float randAngle = Random.Range(-30f, 30f);
                hoverMoveDir = Quaternion.AngleAxis(randAngle, Vector3.forward) * hoverMoveDir.normalized;
                rb.MovePosition((Vector2)transform.position + baseSpeed * Time.fixedDeltaTime * hoverMoveDir.normalized);
            }
            else
            {
                isOverShoot= true;
            }
        }

    }

    public void Dash()
    {
        curDashSpeed = Mathf.Lerp(curDashSpeed, 0, 1.2f * Time.fixedDeltaTime);
        rb.MovePosition((Vector2)transform.position + curDashSpeed * Time.fixedDeltaTime * dashDir.normalized);
    }

}
