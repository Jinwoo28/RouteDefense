using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NomalBullet : Bullet
{

    protected override void AtkCharactor()
    {
        target.GetComponent<Enemy>().EnemyAttacked(damage);
        Destroy(this.gameObject);
    }
}
