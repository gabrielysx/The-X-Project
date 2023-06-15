using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vampire : Enemy
{
    private bool isAngry;
    [SerializeField] private GameObject batPrefab, bulletPrefab;
    private float attackTimer, spawnTimer;
    [SerializeField] private float spawnTime, attackTime;



    public void LongAttack()
    {
        Vector2 bulletDir = (player.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(bulletDir.y, bulletDir.x) * Mathf.Rad2Deg;
        Quaternion rot = new Quaternion();
        rot.eulerAngles = new Vector3(0, 0, angle - 90f);
        GameObject bullet = Instantiate(bulletPrefab, transform.position, rot);
        bullet.GetComponent<IProjectile>().SetFlyDirection(bulletDir);
    }

    public void SpawnBat()
    {
        Instantiate(batPrefab, transform.position, Quaternion.identity);
    }

    public void EnterAttack()
    {
        
    }

    public void ExitAttack()
    {

    }

    public void InAttack()
    {
        Vector2 dir = player.transform.position - transform.position;
        FaceToDirection(dir.x);
        spawnTimer += Time.fixedDeltaTime;
        if (spawnTimer > spawnTime)
        {
            spawnTimer = 0;
            SpawnBat();
        }
        attackTimer += Time.fixedDeltaTime;
        if (attackTimer > attackTime)
        {
            attackTimer = 0;
            LongAttack();
        }

    }
}
