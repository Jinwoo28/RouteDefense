using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowBullet : Bullet
{
    private Enemy enemy = null;
    private bool ontile = false;

    protected override void Update()
    {
        base.Update();
        if (!ontile)
        {
            this.transform.position += moveDir * bullspeed * Time.deltaTime;
            this.transform.LookAt(this.transform.position + moveDir);
        }
        else
        {
            this.transform.position += Vector3.zero;
        }
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

        else if (other.CompareTag("Tile"))
        {
            ontile = true;
            Invoke("ReturnBullet", 1.0f);

        }
    }
}
