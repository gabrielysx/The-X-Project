using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttackManager : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    //private GameObject bulletHolder;
    private bool isShootFiring, isSlashAttacking;
    [SerializeField] private Camera cam;
    [SerializeField] private float firingRate = 2f;
    [SerializeField] private float slashRate = 1f;
    [SerializeField] private float slashRange = 1f;
    [SerializeField] private float slashAngle = 60f;
    [SerializeField] private GameObject slashPrefab;
    private float shootTimer, slashTimer;


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, slashRange);
        Vector3 pos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = pos - transform.position;
        Vector3 temp = Quaternion.AngleAxis(slashAngle / 2, Vector3.forward) * dir.normalized;
        temp = temp * slashRange + transform.position;
        Gizmos.DrawLine(transform.position, new Vector3(temp.x,temp.y,transform.position.z));
        temp = Quaternion.AngleAxis(-slashAngle / 2, Vector3.forward) * dir.normalized;
        temp = temp * slashRange + transform.position;
        Gizmos.DrawLine(transform.position, new Vector3(temp.x, temp.y, transform.position.z));
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 5);
    }

    // Start is called before the first frame update
    void Start()
    {
        //bulletHolder = GameObject.Find("BulletHolder");
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        ShootBullets();
        SlashAttack();
    }

    private void ShootBullets()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isShootFiring = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isShootFiring = false;
        }

        if (!isShootFiring)
        {
            if (shootTimer > 0f)
            {
                shootTimer += Time.deltaTime;
                if (shootTimer > (1 / firingRate))
                {
                    shootTimer = 0f;
                }
            }
        }
        else
        {

            if (shootTimer == 0f)
            {
                Vector3 pos = cam.ScreenToWorldPoint(Input.mousePosition);
                Vector2 dir = pos - transform.position;
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                Quaternion rot = new Quaternion();
                rot.eulerAngles = new Vector3(0, 0, angle - 90f);
                GameObject bullet = Instantiate(bulletPrefab, transform.position, rot);
                if(bullet.GetComponent<ProjectileBase>() == null)
                {
                    bullet.GetComponent<BoomerangProjectile>().flyDir = dir;
                }
                else
                {
                    bullet.GetComponent<ProjectileBase>().flyDir = dir;
                }
                shootTimer += Time.deltaTime;
            }
            else if (shootTimer <= (1 / firingRate))
            {
                shootTimer += Time.deltaTime;
            }
            else
            {
                shootTimer = 0f;
            }
        }
    }



    private void SlashAttack()
    {
        if (Input.GetMouseButtonDown(1))
        {
            isSlashAttacking = true;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            isSlashAttacking = false;
        }

        if (!isSlashAttacking)
        {
            if (slashTimer > 0f)
            {
                slashTimer += Time.deltaTime;
                if (slashTimer > (1 / slashRate))
                {
                    slashTimer = 0f;
                }
            }
        }
        else
        {

            if (slashTimer == 0f)
            {
                //Generate Slash effect
                Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
                Vector2 dir = mousePos - transform.position;
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                Quaternion rot = new Quaternion();
                rot.eulerAngles = new Vector3(0, 0, angle);
                Vector3 spawnPos = dir.normalized * slashRange * 0.75f;
                spawnPos += transform.position;
                GameObject slash = Instantiate(slashPrefab, spawnPos, rot);
                slash.transform.localScale = new Vector3(slashRange * 0.9f, slashRange * 0.75f, 1f);

                //Judge if there is anything being slashed
                SlashJudgment(dir);
                //Set up timer
                slashTimer += Time.deltaTime;
            }
            else if (slashTimer <= (1 / slashRate))
            {
                slashTimer += Time.deltaTime;
            }
            else
            {
                slashTimer = 0f;
            }
        }

    }

    public void SlashJudgment(Vector2 attackDir)
    {
        List<GameObject> slashed_objects = new List<GameObject>();
        List<Collider2D> tmp_colliders = Physics2D.OverlapCircleAll(transform.position, slashRange, LayerMask.GetMask("Enemy")).ToList();

        foreach (Collider2D hitbox in tmp_colliders)
        {
            GameObject temp = hitbox.transform.gameObject;
            if (slashed_objects.Contains(temp) == false)
            {
                Vector2 enemyDir = temp.transform.position - transform.position;
                float tmp_angle = Mathf.Acos(Vector2.Dot(enemyDir.normalized, attackDir.normalized)) * Mathf.Rad2Deg;
                if (tmp_angle <= slashAngle / 2)
                {
                    slashed_objects.Add(temp);
                }
            }
        }

        foreach (GameObject hittedObject in slashed_objects)
        {
            //slash those things
            Vector2 atk_dir = hittedObject.transform.position - transform.position;
            Enemy hittedEnemy = hittedObject.GetComponent<Enemy>();
            if (hittedEnemy != null)
            {
                hittedEnemy.TakeHit(atk_dir, 10f, 0.2f, 0.1f);
            }
        }

    }




}
