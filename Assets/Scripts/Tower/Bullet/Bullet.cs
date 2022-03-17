using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    protected ObjectPooling objectpooling = null;
    protected Transform target;
    protected float damage = 0;
    protected float bullspeed = 0;
    protected Vector3 destiNation = Vector3.zero;
    protected Vector3 shootPos = Vector3.zero;

    protected Vector3 moveDir = Vector3.zero;
    public void SetUp(Transform _target, float _damage, ObjectPooling _objpooling,float _speed)
    {
        bullspeed = _speed;
        target = _target;
        damage = _damage;
        objectpooling = _objpooling;
        Debug.Log("초기화");
    }

    public void MortarSetDestination(Vector3 _desti,Vector3 _shootpos)
    {
        Debug.Log("초기2");
        destiNation = _desti;
        shootPos = _shootpos;
        Invoke("ReturnBullet", 3.0f);
    }

    public void SetArrowDir(Vector3 _target)
    {
        Vector3 Dir = _target/*- shootPos*/;
        moveDir =  Dir.normalized;
    }


    private void Start()
    {
        MultipleSpeed.speedup += SpeedUP;
        
    }

    private void SpeedUP(int x)
    {
        Time.timeScale = x;
    }

    protected virtual void Update()
    {
        if (target == null)
        {
            objectpooling.ReturnObject(this);
        }
        else
        {

        }
    }

    private void ReturnBullet()
    {
        objectpooling.ReturnObject(this);
    }

    public virtual void Attack()
    {
        
    }


    protected virtual void AtkCharactor()
    {

    }

    protected virtual void DDAtkCharactor()
    {

    }




}
