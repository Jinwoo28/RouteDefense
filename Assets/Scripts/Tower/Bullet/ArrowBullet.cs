using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowBullet : Bullet
{
    private Enemy enemy = null;
    private bool ontile = false;
    private bool Ontarget = false;



    protected override void Update()
    {
        DestroyTimer += Time.deltaTime;

        if(DestroyTimer >= 2.0f)
        {
            ReturnBullet();
        }


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

    public override void ResetBullet()
    {
        ontile = false;
        Ontarget = false;
    }

    protected override void AtkCharactor()
    {
        enemy.EnemyAttacked(damage);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (!ontile)
            {
                enemy = other.GetComponent<Enemy>();
                AtkCharactor();
                Ontarget = true;
                
                ReturnBullet();
            }
        }

        else if (other.CompareTag("Tile"))
        {
            ontile = true;
            Invoke("ReturnBullet", 0.5f);

        }
    }
}
