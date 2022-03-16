using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mortarbullet : Bullet
{
    [SerializeField] private LayerMask enemylayer;
    [SerializeField] private float Range = 3.0f;
    
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
            
            Timer += Time.deltaTime/2.0f;
            Vector3 MovePos = ParaBolaMove(_current, _Desti, 4.0f, 1, Timer);

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
        Collider[] E_collider = Physics.OverlapSphere(this.transform.position, Range, enemylayer);

        if (E_collider.Length > 0)
        {
            foreach (Collider EC in E_collider)
            {
                if (Vector3.Magnitude(EC.transform.position - this.transform.position) < 0.5f)
                {
                    EC.GetComponent<Enemy>().EnemyAttacked(damage);
                }
                else if (Vector3.Magnitude(EC.transform.position - this.transform.position) < 0.8f)
                {
                    float decrease = damage - 1;
                    if (decrease <= 0) continue;
                    EC.GetComponent<Enemy>().EnemyAttacked(decrease);
                }
                else if(Vector3.Magnitude(EC.transform.position - this.transform.position) < 1.0f)
                {
                    float decrease = damage - 2;
                    if (decrease <= 0) continue;
                    EC.GetComponent<Enemy>().EnemyAttacked(decrease);
                }
            }
        }
        


    }



}
