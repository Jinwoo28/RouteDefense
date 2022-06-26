using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mortarbullet : Bullet
{
    [SerializeField] private LayerMask enemylayer;
   // [SerializeField] private float Range = 0;

    private float Timer = 0;

    public override void Attack()
    {
        StartCoroutine(BulletMove(shootPos, destiNation));
    }


    IEnumerator BulletMove(Vector3 _current, Vector3 _Desti)
    {
        Timer = 0;
        while (true)
        {

            Timer += Time.deltaTime * 2.5f;
            Vector3 MovePos = ParaBolaMove(_current, _Desti, 2.0f, 1, Timer);

            this.transform.position = MovePos;

            Vector3 distance = destiNation - this.transform.position;


            yield return null;
        }
    }
    private Vector3 ParaBolaMove(Vector3 _start, Vector3 _end, float _height, float _power, float _time)
    {
        float heightvalue = -_power * _height * _time * _time + _power * _height * _time;

        Vector3 pos = Vector3.Lerp(_start, _end, _time);
        //Debug.Log(pos.x);

        return new Vector3(pos.x, heightvalue + pos.y, pos.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Tile"))
        {
            
            AtkCharactor();
            objectpooling.ReturnObject(this);
        }
    }

    

    protected override void AtkCharactor()
    {
        Debug.Log("ÀÌÆåÆ®");
        Vector3 Insight = cam.transform.position - this.transform.position;
        var obj = particle.GetObject(this.transform.position + Insight.normalized);
        obj.SetPool(particle);

        Collider[] E_collider = Physics.OverlapSphere(this.transform.position, MortarRange, enemylayer);

        if (E_collider.Length > 0)
        {
            foreach (Collider EC in E_collider)
            {
                if (Vector3.Distance(EC.transform.position,this.transform.position) < MortarRange * 0.3f)
                {
                    EC.GetComponent<Enemy>().EnemyAttacked(damage);
                }
                else if (Vector3.Distance(EC.transform.position,this.transform.position) < MortarRange * 0.6f)
                {
                    float decrease = damage - (int)(damage*0.6f);
                    if (decrease <= 0) continue;
                    EC.GetComponent<Enemy>().EnemyAttacked(decrease);
                }
                else
                {
                    float decrease = damage - (int)(damage * 0.3f);
                    if (decrease <= 0) continue;
                    EC.GetComponent<Enemy>().EnemyAttacked(decrease);
                }
            }
        }



    }



}
