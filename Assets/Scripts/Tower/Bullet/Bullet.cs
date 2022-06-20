using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    protected bulletpolling objectpooling = null;
    protected Transform target;
    protected float damage = 0;
    protected float bullspeed = 0;
    protected Vector3 destiNation = Vector3.zero;
    protected Vector3 shootPos = Vector3.zero;

    protected float MortarRange = 0;

    protected Camera cam = null;

    private Transform parent;

    public Transform SetParent { set => parent = value; }


    protected bulletpolling particle = null;

    protected float DestroyTimer = 0;

    public void SetPool(bulletpolling pop)
    {
        if (particle == null)
        {
            particle = pop;
        }
    }

    protected Vector3 moveDir = Vector3.zero;
    public void SetUp(Transform _target, float _damage, bulletpolling _objpooling,float _speed)
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
        
    }

    public virtual void ResetBullet()
    {
    }

    public void SetArrowDir(Vector3 forward)
    {
        moveDir = forward;
    }

    public void SetMortarRange(float _ragne)
    {
        MortarRange = _ragne;
    }

    protected virtual void Start()
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
       
    }

    protected void ReturnBullet()
    {
        
        DestroyTimer = 0;
        objectpooling.ReturnObject(this);
    }

    public void ArrowReturnBullet()
    {
        DestroyTimer = 0;
        objectpooling.ReturnObject(this);
        this.gameObject.transform.SetParent(parent);
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
