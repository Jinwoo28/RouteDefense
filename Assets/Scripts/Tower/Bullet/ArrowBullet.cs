using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowBullet : Bullet
{
    protected override void AtkCharactor()
    {
        target.GetComponent<Enemy>().EnemyAttacked(damage);
    }
}
