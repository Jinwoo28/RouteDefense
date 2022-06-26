using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System;
using UnityEngine.UI;

public interface IEnumyAttacked
{
    void Attacked(float damage);
    Transform GetPos();
}

   public class  UnitState
{
    public float unitspeed = 0;
    public int unitcoin = 0;
    public float unithp = 0;
    public int unitamour = 0;
    public int avoidancerate = 0;
    public int type = 0;    //지상이면 0, 하늘이면 1
    public float feature = 0;
}

public class Enemy : MonoBehaviour, IEnumyAttacked
{

    [SerializeField] private int EnemyCode;

    //적의 이동 경로
    private Vector3[] Waypoint;
    private EnemyManager EM = null;
    protected UnitState unitstate = new UnitState();

    private GameObject hpbar = null;
    private GameObject damagenum = null;
    protected GameObject hpbarprefab = null;

    private Camera cam = null;
    protected enum currentstate { nomal, posion, fire, ice, }
    protected currentstate CS;

    protected enum EnemyType { creture, machine, unconfineobject }
    protected EnemyType enemytype;

    private Transform canvas = null;

    private Transform Water = null;
    protected bool underTheSea = false;
    public bool GetWet => underTheSea;
    private bool speedInit = false;

    private float unitspeed = 0;
    public float GetUnitSpeed { get => unitspeed; }
    private float Timer = 0;

    bool jump = false;

    private EnemyPooling EP = null;
    private int enemyNum = 0;
    float Hp = 0;


    private bool electricShock = false;
    public bool GetShock() => electricShock;

    public bool Fired = false;
    public bool Iced = false;
    public bool blood = false;


    public void SetPooling(EnemyPooling _pooling, int _num)
    {
        EP = _pooling;
        enemyNum = _num;
    }

    public void ResetHp()
    {
        unitstate.unithp = Hp;
    }

    private void Awake()
    {
        SetState(EnemyStateSetUp.GetData(EnemyCode));
        TimeScale = OriginTimeScale = Time.timeScale;
    }

    public void SetState(EnemyState stat)
    {
        unitstate.avoidancerate = stat.avoidance;
        unitstate.unitamour = stat.Amour;
        unitstate.unitspeed = stat.Speed;
        unitstate.unitcoin = stat.coin;
        unitstate.unithp = stat.Hp;
        unitstate.type = stat.enemytype;
        unitstate.feature = stat.feature;
    }

    protected virtual void Start()
    {
        Hp = unitstate.unithp;
        MultipleSpeed.speedup += SpeedUP;
        TimeScale = Time.timeScale;
    }

    protected float TimeScale = 1;
    private void SpeedUP(int x)
    {
        OriginTimeScale = x;

        TimeScale = x;
    }


    public void SetUpEnemy(EnemyManager _enemymanager, Vector3[] _waypoint, Transform _canvas, GameObject _hpbar, GameObject _damagenum, Transform _water)
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
            hpbarprefab.GetComponent<EnemyHpbar>().SetUpEnemy(this, this.transform);    //수정필요
            hpbarprefab.GetComponentInChildren<RectTransform>().localScale = new Vector3(1 + 0.03f * (1 + unitstate.unithp * 0.1f), 1 + 0.03f * (1 + unitstate.unithp * 0.1f), 1);
        }

        if (unitstate.type == 0)
        {
            StartCoroutine("MoveGroundUnit");
        }
        else
        {
            StartCoroutine("MoveFlyUnit");
        }
    }

    //공중유닛 움직임
    public IEnumerator MoveFlyUnit()
    {
        int waypointindex = 0;

        Vector3 MoveToPoint = Waypoint[waypointindex];

        Vector3 currentPos = this.transform.position;


        while (waypointindex != Waypoint.Length - 1)
        {
            MoveToPoint = Waypoint[waypointindex];

            if (waypointindex < Waypoint.Length)
            {

                //waypoint 변경
                if (Vector3.Magnitude(MoveToPoint - this.transform.position) < 0.05f)
                {
                    currentPos = Waypoint[waypointindex];
                    waypointindex++;
                    jump = false;
                }

                ////오르막길
                //if (Waypoint[waypointindex].y > currentPos.y)
                //{
                //    if (!jump)
                //    {
                //        jump = true;
                //        unitspeed *= 0.8f;
                //    }
                //}

                ////내리막길
                //else if (Waypoint[waypointindex].y < currentPos.y)
                //{
                //    if (!jump)
                //    {
                //        jump = true;
                //        unitspeed *= 1.2f;
                //    }
                //}

                ////평지
                //else
                //{
                //    float X = unitspeed - unitstate.unitspeed;

                //    //속도 안정화
                //    if (X < -0.05f) unitspeed += Time.unscaledDeltaTime * TimeScale;
                //    else if (X > 0.05f) unitspeed -= Time.unscaledDeltaTime * TimeScale;
                //    else unitspeed = unitstate.unitspeed;

                //    //다음 목적지로 이동
                //}
                
                this.transform.position = Vector3.MoveTowards(transform.position, MoveToPoint, unitspeed * Time.unscaledDeltaTime * TimeScale);

                //Debug.Log("유닛 스피드 : " + unitspeed);
                Vector3 relativePos = Waypoint[waypointindex] - this.transform.position;
                //현재 위치에서 타겟위치로의 방향값
                Quaternion rotationtotarget = Quaternion.LookRotation(relativePos);

                //현재의 rotation값을 타겟위치로의 방향값으로 변환 후 Vector3로 형태로 저장
                Vector3 TowerDir = Quaternion.RotateTowards(this.transform.rotation, rotationtotarget, 360 * Time.unscaledDeltaTime * TimeScale).eulerAngles;

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
        var prefab = hpbarprefab;
        hpbarprefab = null;
        Destroy(prefab);
        EP.ReturnEnemy(this, enemyNum);
    }

    public IEnumerator MoveGroundUnit()
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
                if (!speedInit) unitspeed *= 1.15f;
                speedInit = true;
                underTheSea = false;
            }

            MoveToPoint = Waypoint[waypointindex];

            if (waypointindex < Waypoint.Length)
            {

                if (Vector3.Magnitude(MoveToPoint - this.transform.position) < 0.05f)
                {
                    currentPos = Waypoint[waypointindex];
                    waypointindex++;

                }

                //오르막길
                if (Waypoint[waypointindex].y > currentPos.y)
                {
                    if (!jump)
                    {
                        if (underTheSea) unitspeed = unitstate.unitspeed * 0.5f;
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

                        if (X < -0.05f) unitspeed += Time.unscaledDeltaTime * TimeScale;
                        else if (X > 0.05f) unitspeed -= Time.unscaledDeltaTime * TimeScale;
                        else unitspeed = unitstate.unitspeed;
                    }
                    //다음 목적지로 이동
                    this.transform.position = Vector3.MoveTowards(transform.position, MoveToPoint, unitspeed * Time.unscaledDeltaTime * TimeScale);
                }

                //Debug.Log("유닛 스피드 : " + unitspeed);
                Vector3 relativePos = Waypoint[waypointindex] - this.transform.position;
                //현재 위치에서 타겟위치로의 방향값
                Quaternion rotationtotarget = Quaternion.LookRotation(relativePos);

                //현재의 rotation값을 타겟위치로의 방향값으로 변환 후 Vector3로 형태로 저장
                Vector3 TowerDir = Quaternion.RotateTowards(this.transform.rotation, rotationtotarget, 360 * Time.unscaledDeltaTime * TimeScale).eulerAngles;

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
        var prefab = hpbarprefab;
        hpbarprefab = null;
        Destroy(prefab);
        EP.ReturnEnemy(this, enemyNum);
    }

    //포물선 이동
    private Vector3 parabola(Vector3 _start, Vector3 _end, float _height, float _power, float _time)
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
            Timer += Time.unscaledDeltaTime * TimeScale;

            Vector3 MovePos = parabola(_current, _next, 1.5f, 1, Timer * unitspeed);

            this.transform.position = MovePos;
            yield return null;
        }
        jump = false;
    }

    public void ElectricDamage(float _damage)
    {

        if (hpbarprefab != null)
        {
            hpbarprefab.GetComponent<EnemyHpbar>().StateChange(enemyState.Electric);
            StopCoroutine("ElectricShock");
            StartCoroutine("ElectricShock");

            float Damage = underTheSea ? _damage * 2 : _damage;

            realDamage(Damage);
            electricShock = true;
        }
    }

    IEnumerator ElectricShock()
    {
        yield return new WaitForSeconds(1.0f);
        hpbarprefab.GetComponent<EnemyHpbar>().ReturnIcon(enemyState.Electric);
        electricShock = false;
    }

    

    public virtual void EnemyAttacked(float _damage)
    {
        float realdamage = 0;

        realdamage = _damage - unitstate.unitamour;

        int X = Random.Range(1, 101);
        if (X < unitstate.avoidancerate)
        {
            realDamage(0);
            ShowDamage(0, 0);
        }
        else
        {
            ShowDamage(realdamage, 1);
            realDamage(realdamage);
        }

    }

    public void realDamage(float _damage)
    {
        
        if (_damage >= unitstate.unithp)
        {
            EnemyDie();
        }
        else
        {
            unitstate.unithp -= _damage;
        }
    }


    float originSpeed;


    protected float OriginTimeScale = 1;

    private bool alreadyslow = false;

    public void SlowDown()
    {
        if (!alreadyslow)
        {
            alreadyslow = true;

            if (!underTheSea)
            {
                TimeScale *= 0.5f;
                Debug.Log("SLow");
            }
            else
            {
                Debug.Log("Stop");
                TimeScale = 0;
                Iced = true;
            }
        }

        if (underTheSea)
        {
            TimeScale = 0;
            Iced = true;
        }
    }

    public void returnSpeed()
    {
        ReturnTimeScale();
        Iced = false;
        alreadyslow = false;
    }

    public void ReturnTimeScale()
    {
        TimeScale = OriginTimeScale;
    }

    public void SpeedChange(float n)
    {
        TimeScale *= n; 
    }


    public IEnumerator IcedEnemy()
    {
        unitspeed = 0;

        yield return new WaitForSeconds(1.0f);
        unitspeed = originSpeed;

    }

  

    

    public void EnemyDie()
    {
        if(this.GetComponentInChildren<Bullet>() != null)
        {
            this.GetComponentInChildren<Bullet>().ArrowReturnBullet();

            Debug.Log("Asdf");
        }

        jump = false;

        Destroy(hpbarprefab);
        returnSpeed();
        EM.EnemyDie(this, unitstate.unitcoin);
        EP.ReturnEnemy(this, enemyNum);
    }

    

    public void ShowDamage(float _damage, int _Block)
    {
        cam = Camera.main;

        GameObject damagecount = null;

        if (unitstate.type == 0)
        {
            damagecount = Instantiate(damagenum, cam.WorldToScreenPoint(this.transform.position), Quaternion.identity);
        }
        else
        {
            damagecount = Instantiate(damagenum, cam.WorldToScreenPoint(this.GetComponentInChildren<FlyEnemy>().GetBody().position), Quaternion.identity);
        }
        damagecount.transform.SetParent(canvas);

        if (unitstate.type == 0)
        {
            damagecount.GetComponent<HpNum>().SetUp(this.transform.position.x, this.transform.position.y, this.transform.position.z, _damage, _Block);
        }
        else
        {
            Vector3 pos = this.GetComponentInChildren<FlyEnemy>().GetBody().position;
            damagecount.GetComponent<HpNum>().SetUp(pos.x, pos.y, pos.z, _damage, _Block);
        }
    }

    virtual protected void UnitCharacteristic() { }

    virtual protected void UnitSkill() { }

    public void Attacked(float damage)
    {
        EnemyAttacked(damage);
    }

    public Transform GetPos()
    {
        return this.gameObject.transform;
    }

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

    public int GetEnemyType => unitstate.type;



}
