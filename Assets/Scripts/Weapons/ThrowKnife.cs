using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowKnife : WeaponBase, IWeaponHandler
{

    public void Fire(Vector2 dir, Vector3 initialPos)
    {
        GenerateBulletPrefab(dir, initialPos, bulletPrefab);
    }

    public float GetFireRate()
    {
        return fireRate;
    }
}
