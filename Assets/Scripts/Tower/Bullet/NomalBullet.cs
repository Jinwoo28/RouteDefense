using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NomalBullet : Bullet
{
    private void Start()
    {
        bullspeed = 3.0f;
    }

    protected override void AtkCharactor()
    {
        target.GetComponent<Enemy>().EnemyAttacked(damage);
       // Destroy(this.gameObject);
    }

    
}
