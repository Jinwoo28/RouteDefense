using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System;
using UnityEngine.UI;

   [System.Serializable]
   public class  UnitState
{
    public float unitspeed = 0;
    public int unitcoin = 0;
    public float unithp = 0;
    public int unitamour = 0;
    public int avoidancerate = 0;
}



public class Enemy : MonoBehaviour
{
    //적의 이동 경로
    private Vector3[] Waypoint;
    private EnemyManager EM = null;
    [SerializeField] protected UnitState unitstate = null;

    private GameObject hpbar = null;
    private GameObject damagenum = null;
    private GameObject hpbarprefab = null;
    
    private Camera cam = null;
    protected enum currentstate { nomal, posion, fire, ice, }
    protected currentstate CS;

    protected enum EnemyType { creture, machine, unconfineobject }
    protected EnemyType enemytype;

    private Transform canvas = null;

    private Transform Water = null;
    private bool underTheSea = false;
    private bool speedInit = false;

    private float unitspeed = 0;
    public float GetUnitSpeed { get => unitspeed; }
    private float Timer = 0;

    bool jump = false;

    private EnemyPooling EP = null;
    private int enemyNum = 0;
    float Hp = 0;

    public void SetPooling(EnemyPooling  _pooling, int _num)
    {
        EP = _pooling;
        enemyNum = _num;
    }

    public void ResetHp()
    {
        unitstate.unithp = Hp;

    }

    protected virtual void Start()
    {
        Hp = unitstate.unithp;
        MultipleSpeed.speedup += SpeedUP;
    }

    private void SpeedUP(int x)
    {
        Time.timeScale = x;
    }



    public void SetUpEnemy(EnemyManager _enemymanager, Vector3[] _waypoint,Transform _canvas,GameObject _hpbar, GameObject _damagenum,Transform _water)
    {
        Water = _water;
        hpbar = _hpbar;
        damagenum = _damagenum;
        Waypoint = _waypoint;
        EM = _enemymanager;
        canvas = _canvas;
    }

   public void StartMove()
    {

        unitspeed = unitstate.unitspeed;

        if (hpbarprefab == null)
        {
            hpbarprefab = Instantiate(hpbar);
            hpbarprefab.transform.SetParent(canvas);
            hpbarprefab.GetComponent<EnemyHpbar>().SetUpEnemy(this,this.transform);
            hpbar.GetComponentInChildren<RectTransform>().localScale = new Vector3(1+ 0.03f * (1+unitstate.unithp*0.1f), 1 + 0.03f * (1 + unitstate.unithp * 0.1f), 1);
        }

        StartCoroutine("MoveUnit");
    }

    public IEnumerator MoveUnit()
    {
        int waypointindex = 0;

        Vector3 MoveToPoint = Waypoint[waypointindex];

        Vector3 currentPos = this.transform.position;

        while (waypointindex != Waypoint.Length - 1)
        {

            if (this.transform.position.y < Water.transform.position.y)
            {
                underTheSea = true;
                speedInit = false;
            }
            else
            {
                if(!speedInit) unitspeed *= 1.15f;
                speedInit = true;
                underTheSea = false;
            }

            MoveToPoint = Waypoint[waypointindex];

            if (waypointindex < Waypoint.Length)
            {

                if (Vector3.Magnitude(MoveToPoint-this.transform.position)<0.05f)
                {
                    currentPos = Waypoint[waypointindex];
                    waypointindex++;
                    
                }

                //오르막길
                if (Waypoint[waypointindex].y > currentPos.y)
                {
                    if (!jump)
                    {
                        if(underTheSea) unitspeed = unitstate.unitspeed * 0.5f;
                        else unitspeed *= 0.8f;
                        StartCoroutine(MoveToNext(currentPos, Waypoint[waypointindex]));
                    }
                }

                //내리막길
                else if (Waypoint[waypointindex].y < currentPos.y)
                {
                    
                    if (!jump)
                    {
                        if (underTheSea) unitspeed = unitstate.unitspeed * 0.5f;
                        else unitspeed *= 1.2f;
                        StartCoroutine(MoveToNext(currentPos, Waypoint[waypointindex]));
                    }
                }

                //평지
                else
                {
                    
                    float X = unitspeed - unitstate.unitspeed;

                    //느린상태
                    if (underTheSea) unitspeed = unitstate.unitspeed * 0.5f;
                    else
                    {
                        if (X < -0.05f) unitspeed += Time.deltaTime;
                        else if (X > 0.05f) unitspeed -= Time.deltaTime;
                        else unitspeed = unitstate.unitspeed;
                    }
                    //다음 목적지로 이동
                    this.transform.position = Vector3.MoveTowards(transform.position, MoveToPoint, unitspeed * Time.deltaTime);
                }

                //Debug.Log("유닛 스피드 : " + unitspeed);
                Vector3 relativePos = Waypoint[waypointindex] - this.transform.position;
                //현재 위치에서 타겟위치로의 방향값
                Quaternion rotationtotarget = Quaternion.LookRotation(relativePos);

                //현재의 rotation값을 타겟위치로의 방향값으로 변환 후 Vector3로 형태로 저장
                Vector3 TowerDir = Quaternion.RotateTowards(this.transform.rotation, rotationtotarget, 360 * Time.deltaTime).eulerAngles;

                //현재의 rotation값에 Vector3형태로 저장한 값 사용
                this.transform.rotation = Quaternion.Euler(0, TowerDir.y, 0);
                
                yield return null;
            }
            else
            {
                
                yield break;
            }
        }
        EM.EnemyArriveDestination(this);
        Destroy(hpbarprefab);
        EP.ReturnEnemy(this, enemyNum);
    }

    //포물선 이동
    private Vector3 parabola(Vector3 _start, Vector3 _end, float _height, float _power,float _time)
    {
        //y축은 파워값과 높이값에 time.deltatime을 곱한다.
        //time이 1보다 작을 때는 양수, 1보다 클 때는(음수)이기 때문에 포물선 이동이 가능
        float heightvalue = -_power * _height * _time * _time + _power * _height * _time;

        //Mathf.sin
        //시작 지점의 좌표와 도착지점의 좌표 보간값
        //x축과 z축은 보간된 값으로 업데이트

        Vector3 pos = Vector3.Lerp(_start, _end, _time);

        if (heightvalue <= 0)
        {
            heightvalue = 0;
        }

        //Debug.Log(heightvalue+ "현재 Parabola");

        //Debug.Log(heightvalue + pos.y + "현재 Y");
        return new Vector3(pos.x, heightvalue + pos.y, pos.z);
    }

    

    private IEnumerator MoveToNext(Vector3 _current, Vector3 _next)
    {
        jump = true;
        Timer = 0;
        while (Vector3.Magnitude(_next - this.transform.position) > 0.05f)
        {
            Timer += Time.deltaTime;
           
            Vector3 MovePos =  parabola(_current, _next, 1.5f, 1, Timer*unitspeed);

            this.transform.position = MovePos;
            yield return null;
        }
        jump = false;
    }











    public virtual void EnemyAttacked(float _damage)
    {
        float realdamage = 0;

        realdamage = _damage - unitstate.unitamour;

        int X = Random.Range(1, 101);
        if (X < unitstate.avoidancerate)
        {
            realdamage = 0;
        }

        ShowDamage(realdamage);
        if (realdamage >= unitstate.unithp)
        {
            EnemyDie();
        }
        else
        {
            unitstate.unithp -= realdamage;
        }
    }

    public void firedamage(float _damage)
    {
        ShowDamage(_damage);
        if (_damage >= unitstate.unithp)
        {
            EnemyDie();
        }
        else
        {
            unitstate.unithp -= _damage;
        }
    }

    public void EnemyDie()
    {
        jump = false;

        Destroy(hpbarprefab);
        EM.EnemyDie(this, unitstate.unitcoin);
        EP.ReturnEnemy(this, enemyNum);
    }

    

    public void ShowDamage(float _damage)
    {
        cam = Camera.main;
    
        GameObject damagecount = Instantiate(damagenum, cam.WorldToScreenPoint(this.transform.position),
            Quaternion.identity);
        damagecount.transform.SetParent(canvas);
        damagecount.GetComponent<HpNum>().SetUp(this.transform.position.x, this.transform.position.y, this.transform.position.z, _damage);
 
    }

    virtual protected void UnitCharacteristic() { }

    virtual protected void UnitSkill() { }

    public float GetHp
    {
        get
        {
            return unitstate.unithp;
        }

        set
        {
            unitstate.unithp += value;
        }
    }

    public float SetOriginHp
    {
        set
        {
            unitstate.unithp = value;
        }
    }

}
