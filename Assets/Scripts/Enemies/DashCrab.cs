using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashCrab : Enemy, IObserver
{
    private bool isChasing,isEndure,isAngry;
    private float chaseTimer;
    private float originalSpeed,pauseSpeed;
    [SerializeField] private float chaseMaxSpeed, endureTime, stunTime;
    public bool GetIsChasing() { return isChasing; }
    public bool GetIsEndure() { return isEndure; }

    protected override void Start()
    {
        base.Start();
        originalSpeed = baseSpeed;
        if(LevelManager.instance.crabOpinion == Opinion.Hate)
        {
            isAngry = true;
        }
        else
        {
            isAngry = false;
        }
        LevelManager.instance.AddObserver(this);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void EnterChasing()
    {
        if(!isEndure && !isChasing)
        {
            isEndure= true;
            chaseTimer = 0;
            isChasing = true;
        }
        else if (isEndure && isChasing)
        {
            baseSpeed = pauseSpeed;
        }
        else
        {
            //resume stun 
        }
    }

    public void OnChasing()
    {
        chaseTimer += Time.fixedDeltaTime;
        if(chaseTimer < endureTime)
        {
            //Set color effect
            float factor = baseSpeed / chaseMaxSpeed;
            if(factor > 0.8) factor = 0.8f;
            sr.material.SetFloat("_FlashAmount", factor);
            sr.material.SetColor("_FlashColor", Color.red);

            float step = (chaseMaxSpeed - originalSpeed) / endureTime * Time.fixedDeltaTime;
            baseSpeed += step;
            if(baseSpeed >= chaseMaxSpeed)
            {
                baseSpeed = chaseMaxSpeed;
            }
            MovetoPlayer();
        }
        else if(chaseTimer < endureTime + stunTime)
        {
            sr.material.SetColor("_FlashColor", Color.black);
            isEndure = false;
            //stunned, stay still
        }
        else
        {
            //reset
            sr.material.SetFloat("_FlashAmount", 0);
            sr.material.SetColor("_FlashColor", Color.white);
            isChasing = false;
        }
    }

    public void ExitChasing()
    {
        if(isEndure)
        {
            pauseSpeed = baseSpeed;
        }
        else
        {
            //reset
            sr.material.SetFloat("_FlashAmount", 0);
            sr.material.SetColor("_FlashColor", Color.white);
        }
        baseSpeed = originalSpeed;

    }

    public override void TakeDamage(int damage)
    {
        if(LevelManager.instance.crabOpinion == Opinion.Friendly)
        {
            return;
        }
        else
        {
            isAngry = true;
            LevelManager.instance.AddCrabHateValue(5);
            base.TakeDamage(damage);
        }
        
    }

    public override bool IfWithinAttackRange()
    {
        if(isAngry)
        {
            return base.IfWithinAttackRange();
        }
        else
        {
            return false;
        }
    }
    public override bool IfWithinDetectRange()
    {
        if(isAngry)
        {
            return base.IfWithinDetectRange();
        }
        else
        {
            return false;
        }
    }

    public bool GetIsAngry()
    {
        return isAngry;
    }

    public void OnNotify()
    {
        isAngry = true;
    }
}
