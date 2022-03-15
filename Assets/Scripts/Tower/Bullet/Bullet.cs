using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private ObjectPooling objectpooling = null;
    protected Transform target;
    protected float damage = 0;
    protected float bullspeed = 0;

    public void SetUp(Transform _target, float _damage, ObjectPooling _objpooling,float _speed)
    {
        bullspeed = _speed;
        target = _target;
        damage = _damage;
        objectpooling = _objpooling;
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
        if (target == null)
        {
            objectpooling.ReturnObject(this);
        }
        else
        {
            Attack();
        }
    }

    protected virtual void Attack()
    {
        Vector3 distance = target.position - this.transform.position;

        this.transform.position += distance.normalized * Time.deltaTime * bullspeed;

        if (Vector3.SqrMagnitude(distance) < 0.1f)
        {
            AtkCharactor();
            objectpooling.ReturnObject(this);
        }
    }


    protected virtual void AtkCharactor()
    {

    }




}
