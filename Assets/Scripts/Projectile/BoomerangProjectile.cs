using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangProjectile : MonoBehaviour, IProjectile
{
    public Vector2 flyDir;
    private Rigidbody2D rb;
    private bool damaging, returning;
    private float movementTimer = 0f;
    private float dmgTimer = 0f;
    [SerializeField]private float returnTime = 2f;
    [SerializeField]private float speed = 3f;
    private float curSpeed;
    [SerializeField] AnimationCurve forwardCurve, returnCurve;

    private List<GameObject> hittedList = new List<GameObject>();

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
        curSpeed = speed;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && damaging && !hittedList.Contains(collision.gameObject))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(1);
                enemy.TakeHit(flyDir, 5, 0.3f, 0.1f);
                hittedList.Add(collision.gameObject);
            }
        }
    }

    private void FixedUpdate()
    {
        dmgTimer += Time.fixedDeltaTime;
        if(damaging)
        {
            damaging = false;
            hittedList.Clear();
        }
        if(dmgTimer >= 0.2f)
        {
            dmgTimer = 0f;
            damaging = true;
        }
        Movement();
        rb.MoveRotation(Mathf.Rad2Deg * movementTimer * 10);
    }

    private void Movement()
    {
        //update timer
        movementTimer += Time.fixedDeltaTime;
        if(movementTimer >= returnTime)
        {
            returning = true;
        }
        if(!returning)
        {
            float r = movementTimer / returnTime;
            curSpeed = DOVirtual.EasedValue(speed, 0, r, forwardCurve);
        }
        else
        {
            flyDir = Character.mainPlayerInstance.transform.position - transform.position;
            float ratio = (movementTimer - returnTime) / returnTime;
            curSpeed = DOVirtual.EasedValue(0, speed, ratio, returnCurve);
        }
        Vector2 pos = gameObject.transform.position;
        pos += flyDir.normalized * curSpeed * Time.fixedDeltaTime;
        rb.MovePosition(pos);
        if(returning && Vector2.Distance(transform.position, Character.mainPlayerInstance.transform.position) <= 0.1f)
        {
            Destroy(gameObject);
        }
    }

    public void SetFlyDirection(Vector2 dir)
    {
        flyDir = dir;
    }
}
