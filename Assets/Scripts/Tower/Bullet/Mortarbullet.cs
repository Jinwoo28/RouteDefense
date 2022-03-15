using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mortarbullet : Bullet
{ 
    [SerializeField] private LayerMask enemylayer;
    [SerializeField] private float Range = 3.0f;
    private Vector3 destiNation = Vector3.zero;
    private float Timer = 0;
    private void Start()
    {
        bullspeed = 10f;
        StartCoroutine(BulletMove(this.transform.position, destiNation));
    }

    public void SetUp(Vector3 _destiNantion)
    {
        destiNation = _destiNantion;
    }

    private Vector3 ParaBolaMove(Vector3 _start, Vector3 _end, float _height, float _power, float _time)
    {
        float heightvalue = -_power * _height * _time * _time + _power * _height * _time;

        Vector3 pos = Vector3.Lerp(_start, _end, _time);

        return new Vector3(pos.x, heightvalue + pos.y, pos.z);
    }

    IEnumerator BulletMove(Vector3 _current, Vector3 _Desti)
    {
        Timer = 0;
        while (true)
        {
            Timer += Time.deltaTime;
            Vector3 thisMoving = ParaBolaMove(_current, _Desti, 1.5f, 1, Timer);
            this.transform.position = thisMoving;
            yield return null;
        }
    }

    protected override void AtkCharactor()
    {
        Collider[] E_collider = Physics.OverlapSphere(this.transform.position, Range, enemylayer);

        foreach(Collider EC in E_collider)
        {
            if (Vector3.Magnitude(EC.transform.position - this.transform.position) < 1.0f)
            {
                Debug.Log("ddd");
                EC.GetComponent<Enemy>().EnemyAttacked(damage);
            }
            else if(Vector3.Magnitude(EC.transform.position - this.transform.position) < 2.0f)
            {
                float decrease = damage - 1;
                if (decrease <= 0) continue;
                EC.GetComponent<Enemy>().EnemyAttacked(decrease);
            }
            else
            {
                float decrease = damage - 2;
                if (decrease <= 0) continue;
                EC.GetComponent<Enemy>().EnemyAttacked(decrease);
            }
        }
        Destroy(this.gameObject);


    }



}
