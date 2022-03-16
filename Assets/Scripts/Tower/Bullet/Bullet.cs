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

        }
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
