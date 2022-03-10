using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTest : MonoBehaviour
{
    private Transform target;
    protected float damage = 0;
    protected float speed = 10;

    public void SetBulletTest(Transform _target, float _damage)
    {
        target = _target;
        damage = _damage;
    }

    private void Start()
    {
        MultipleSpeed.speedup += SpeedUP;
    }

    private void SpeedUP(int x)
    {
        Time.timeScale = x;
    }


    private void Update()
    {
        if(target == null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Vector3 distance = target.position - this.transform.position;

            this.transform.position += distance.normalized * Time.deltaTime * speed;

            if (Vector3.Magnitude(distance) < 0.1f)
            {
                target.GetComponent<Enemy>().EnemyAttacked(damage);
                Destroy(this.gameObject);
            }
        }
    }


}
