using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

   
public class Enemy : MonoBehaviour
{
    private Vector3[] Waypoint;
    private EnemyManager EM = null;
    protected int UnitCoin = 10;

    [SerializeField] private float unitSpeed = 0;
    [SerializeField] private float unitHp = 0;
    [SerializeField] private float Amour = 0;

    [SerializeField] private GameObject hpbar = null;

    private GameObject hpbarprefab = null;

    [SerializeField] private GameObject damagenum = null;
    private Camera cam = null;
    protected enum currentstate { nomal, posion, fire, ice, }
    protected currentstate CS;

    protected enum EnemyType { creture, machine, unconfineobject }
    protected EnemyType enemytype;

    private Transform canvas = null;
    

    public void SetUpEnemy(EnemyManager _enemymanager, Vector3[] _waypoint,Transform _canvas)
    {
        Waypoint = _waypoint;
        EM = _enemymanager;
        canvas = _canvas;
    }

   protected void StartMove()
    {

         hpbarprefab = Instantiate(hpbar);
        hpbarprefab.GetComponent<EnemyHpbar>().SetUpEnemy(this,this.transform);
        hpbarprefab.transform.SetParent(canvas);

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

                Vector3 relativePos = Waypoint[waypointindex] - this.transform.position;
                //현재 위치에서 타겟위치로의 방향값
                Quaternion rotationtotarget = Quaternion.LookRotation(relativePos);


                //현재의 rotation값을 타겟위치로의 방향값으로 변환 후 Vector3로 형태로 저장
                Vector3 TowerDir = Quaternion.RotateTowards(this.transform.rotation, rotationtotarget, 270 * Time.deltaTime).eulerAngles;

                //현재의 rotation값에 Vector3형태로 저장한 값 사용
                this.transform.rotation = Quaternion.Euler(TowerDir.x, TowerDir.y, 0);
                
                yield return null;
            }
            else
            {
                yield break;
            }
        }
        EM.EnemyArriveDestination(this);
        Destroy(hpbarprefab);
        Destroy(this.gameObject);
    }

    public void EnemyAttacked(float _damage)
    {
        float realdamage = _damage - Amour;
        if (realdamage >= unitHp)
        {
            EnemyDie();
        }
        else
        {
            ShowDamage(realdamage);
            unitHp -= realdamage;
        }
    }

    public void EnemyDie()
    {
        Destroy(hpbarprefab);
        EM.EnemyDie(this, UnitCoin);
        Destroy(this.gameObject);
    }


    IEnumerator DotDamage(float _damage,currentstate damagetype)
    {
        int damagecount = 5;
        while (damagecount >= 0)
        {
            if (_damage < unitHp)
            {
                damagecount--;
                unitHp -= _damage;
                CS = damagetype;
            }
            else
            {
                EnemyDie();
            }
            yield return new WaitForSeconds(0.5f);
        }
        CS = currentstate.nomal;
    }

    public void ShowDamage(float _damage)
    {
        cam = Camera.main;
    
        GameObject damagecount = Instantiate(damagenum, cam.WorldToScreenPoint(this.transform.position),
            Quaternion.identity);
        damagecount.transform.SetParent(canvas);
        damagecount.GetComponent<HpNum>().SetUp(this.transform, _damage);
 
    }

    virtual protected void UnitCharacteristic() { }

    virtual protected void UnitSkill() { }

    public float GetHp
    {
        get
        {
            return unitHp;
        }
    }

}
