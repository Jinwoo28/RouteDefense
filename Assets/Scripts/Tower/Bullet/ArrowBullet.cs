using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowBullet : Bullet
{
    private Enemy enemy = null;
    private bool ontile = false;
    private bool Ontarget = false;
    private LineRenderer LR = null;

    private void Awake()
    {
        LR = this.GetComponent<LineRenderer>();
    }

    protected override void Update()
    {
        DestroyTimer += Time.deltaTime;

        if(DestroyTimer >= 2.0f)
        {
            ArrowReturnBullet();
        }

        if (!target.gameObject.activeInHierarchy)
        {
            ArrowReturnBullet();
        }

        if (!ontile)
        {
                Vector3 dir = target.transform.position- this.transform.position;
                this.transform.position += dir.normalized * bullspeed * Time.deltaTime;
                this.transform.LookAt(target.position);
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

    public void SetUP()
    {
        LR.enabled = true;
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
                //AtkCharactor();
                Ontarget = true;

                ontile = true;
                if (other.GetComponent<IEnumyAttacked>().GetPos().gameObject.activeInHierarchy)
                {

                    this.transform.SetParent(other.gameObject.transform);
                    Return();
                }
                else
                {
                    ArrowReturnBullet();
                }
                other.GetComponent<IEnumyAttacked>().Attacked(damage);

            }
        }
    }

    IEnumerator Return()
    {
        yield return new WaitForSeconds(0.5f);
        ArrowReturnBullet();
    }
}
