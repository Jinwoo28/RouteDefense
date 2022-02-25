using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

   
public class Enemy : MonoBehaviour
{
    private Vector3[] Waypoint;
    private EnemyManager EM = null;
    private int UnitCoin = 10;

    protected float unitSpeed = 1.0f;
    protected float unitHp = 10;

    enum currentstate { nomal, posion, fire, ice, }
    currentstate CS;

    void Start()
    {
        CS = currentstate.nomal;
    }

    private void Update()
    {

    }

    public void SetUpEnemy(EnemyManager _enemymanager, Vector3[] _waypoint)
    {
        Waypoint = _waypoint;
        EM = _enemymanager;
        StartCoroutine("MoveUnit");
    }

   public IEnumerator MoveUnit()
    {
        int waypointindex = 0;

        Vector3 MoveToPoint = Waypoint[waypointindex];
       
        while (waypointindex != Waypoint.Length - 1)
        {

            MoveToPoint = Waypoint[waypointindex];

            if (waypointindex < Waypoint.Length)
            {

                if (transform.position == MoveToPoint) 
                {
                    waypointindex++;
                }

                this.transform.position = Vector3.MoveTowards(transform.position, MoveToPoint, unitSpeed * Time.deltaTime);
                this.transform.forward = Waypoint[waypointindex];
                yield return null;
            }
            else
            {
                yield break;
            }
        }
        Debug.Log("ÀÌµ¿³¡");
        EM.EnemyArriveDestination(this);
        Destroy(this.gameObject);
    }



    public void EnemyDie(float _damage)
    {
        if (_damage > unitHp)
        {
            EM.EnemyDie(this, UnitCoin);
            Destroy(this.gameObject);
        }
        else
        {
            unitHp -= _damage;
        }
    }



}
