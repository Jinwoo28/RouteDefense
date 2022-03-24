using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowBullet : Bullet
{
    private Enemy enemy = null;
    private bool ontile = false;
    private bool Ontarget = false;
    private Transform hitTarget = null;

    protected override void Update()
    {
        base.Update();
        if (!ontile)
        {
            if (!Ontarget)
            {
                this.transform.position += moveDir * bullspeed * Time.deltaTime;
                this.transform.LookAt(this.transform.position + moveDir);
            }
            else
            {
                this.transform.position += Vector3.zero;
            }
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
            enemy = other.GetComponent<Enemy>();
            AtkCharactor();
            Ontarget = true;
            Invoke("ReturnBullet", 0.1f);
        }

        else if (other.CompareTag("Tile"))
        {
            ontile = true;
            Invoke("ReturnBullet", 1.0f);

        }
    }
}
