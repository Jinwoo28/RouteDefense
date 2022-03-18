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

    protected float MortarRange = 0;

    protected Camera cam = null;


    protected ObjectPooling particle = null;

    public void SetPool(ObjectPooling pop)
    {
        if (particle == null)
        {
            particle = pop;
            Debug.Log(particle+"Ddddddd");
        }
    }

    protected Vector3 moveDir = Vector3.zero;
    public void SetUp(Transform _target, float _damage, ObjectPooling _objpooling,float _speed)
    {
        bullspeed = _speed;
        target = _target;
        damage = _damage;
        objectpooling = _objpooling;
    }

    public void MortarSetDestination(Vector3 _desti,Vector3 _shootpos)
    {
        destiNation = _desti;
        shootPos = _shootpos;
        Invoke("ReturnBullet", 5.0f);
    }

    public void SetArrowDir(Vector3 _target)
    {
        Vector3 Dir = _target/*- shootPos*/;
        moveDir =  Dir.normalized;
    }

    public void SetMortarRange(float _ragne)
    {
        MortarRange = _ragne;
    }

    private void Start()
    {
        MultipleSpeed.speedup += SpeedUP;
        cam = Camera.main;
    }

    private void SpeedUP(int x)
    {
        Time.timeScale = x;
    }

    protected virtual void Update()
    {
        if (target == null)
        {
            //objectpooling.ReturnObject(this);
        }
        else
        {

        }
    }

    protected void ReturnBullet()
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
