using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortBow : WeaponBase,IWeaponHandler
{
    [SerializeField] private float multiSpreadAngle = 60f;
    [SerializeField] private int multiShotAmount = 3;
    public void Fire(Vector2 dir, Vector3 initialPos)
    {
        MultiShot(dir, initialPos,bulletPrefab,multiSpreadAngle,multiShotAmount);
    }

    public float GetFireRate()
    {
        return fireRate;
    }
}
