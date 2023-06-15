using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : ProjectileBase,IProjectile
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        WallDetection(collision);
        //Detect Player
    }

}
