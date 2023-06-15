using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SlashSword : WeaponBase, IWeaponHandler
{
    [SerializeField] private float slashRange = 1f;
    [SerializeField] private float slashAngle = 60f;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, slashRange);
        Vector3 pos = Character.mainPlayerInstance.GetMainCamera().ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = pos - transform.position;
        Vector3 temp = Quaternion.AngleAxis(slashAngle / 2, Vector3.forward) * dir.normalized;
        temp = temp * slashRange + transform.position;
        Gizmos.DrawLine(transform.position, new Vector3(temp.x, temp.y, transform.position.z));
        temp = Quaternion.AngleAxis(-slashAngle / 2, Vector3.forward) * dir.normalized;
        temp = temp * slashRange + transform.position;
        Gizmos.DrawLine(transform.position, new Vector3(temp.x, temp.y, transform.position.z));
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 5);
    }

    public void Fire(Vector2 dir, Vector3 initialPos)
    {
        SlashAttack(dir, initialPos);
    }

    public float GetFireRate()
    {
        return fireRate;
    }

    private void SlashAttack(Vector2 dir, Vector3 initialPos)
    {
        //Calculate slash effect position
        Vector3 spawnPos = dir.normalized * slashRange * 0.75f;
        spawnPos += initialPos;
        //Calculate slash effect rotation
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion rot = new Quaternion();
        rot.eulerAngles = new Vector3(0, 0, angle);
        //Generate Slash effect
        GameObject slash = Instantiate(bulletPrefab, spawnPos, rot);
        slash.transform.localScale = new Vector3(slashRange * 0.9f, slashRange * 0.75f, 1f);

        //Judge if there is anything being slashed
        SlashJudgment(dir);
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
