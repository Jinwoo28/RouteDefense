using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowBullet : Bullet
{
    private Enemy enemy = null;


    protected override void Update()
    {
        base.Update();
        this.transform.position += moveDir *bullspeed * Time.deltaTime;
        this.transform.LookAt(this.transform.position + moveDir);
    }



    protected override void AtkCharactor()
    {
        enemy.EnemyAttacked(damage);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemy = other.GetComponent<Enemy>();
            AtkCharactor();
            objectpooling.ReturnObject(this);
        }
    }
}
