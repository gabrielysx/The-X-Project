using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeaponHandler
{
    void Fire(Vector2 dir, Vector3 initialPos);
    float GetFireRate();
}

public class WeaponBase : MonoBehaviour
{
    [SerializeField]protected float fireRate = 1f;
    [SerializeField]protected GameObject bulletPrefab;
    protected void GenerateBulletPrefab(Vector2 dir,Vector3 pos, GameObject curBulletPrefab)
    {
        //Calculate rotation angle
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion rot = new Quaternion();
        rot.eulerAngles = new Vector3(0, 0, angle - 90f);

        //Generate bullet prefab
        GameObject bullet = Instantiate(curBulletPrefab, pos, rot);

        //Set bullet fly direction
        bullet.GetComponent<IProjectile>().SetFlyDirection(dir);
    }

    protected void MultiShot(Vector2 dir, Vector3 pos, GameObject curBulletPrefab, float spreadAngle, int amount)
    {
        if(amount == 1)
        {
            //shot only one bullet
            GenerateBulletPrefab(dir, pos, curBulletPrefab);
            return;
        }
        //initialize variables
        List<Vector2> bulletsDir = new List<Vector2>();
        int intervalAmount = amount - 1;
        float intervalAngle = spreadAngle / intervalAmount;
        //Get bullets directions
        if (spreadAngle == 360)
        {
            //surround shot
            intervalAngle = spreadAngle / amount;
            for (int i = 0; i < amount; i++)
            {
                Vector2 curDir = Quaternion.AngleAxis(intervalAngle * i, Vector3.forward) * dir.normalized;
                bulletsDir.Add(curDir);
            }
        }
        else
        {
            //get the toppest dir
            Vector2 beginDir = Quaternion.AngleAxis(-spreadAngle/2, Vector3.forward) * dir.normalized;
            bulletsDir.Add(beginDir);
            for(int i = 1;i<=intervalAmount;i++)
            {
                Vector2 curDir = Quaternion.AngleAxis(intervalAngle * i, Vector3.forward) * beginDir.normalized;
                bulletsDir.Add(curDir);
            }
        }

        foreach(Vector2 curDir in bulletsDir)
        {
            GenerateBulletPrefab(curDir, pos, curBulletPrefab);
        }

    }



}
