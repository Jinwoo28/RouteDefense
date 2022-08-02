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
    public int type = 0;    //�����̸� 0, �ϴ��̸� 1
    public float feature = 0;
}

public class Enemy : MonoBehaviour, IEnumyAttacked
{
    [SerializeField] private bool Boss;
    public bool GetBoss() => Boss;
    [SerializeField] private int EnemyCode;

    //���� �̵� ���
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
        SetState(EnemyDataSetUp.GetData(EnemyCode));
        TimeScale = OriginTimeScale = Time.timeScale;
    }

    public void SetState(EnemyDataFrame stat)
    {
        unitstate.avoidancerate = stat.avoidance;
        unitstate.unitamour = stat.amour;
        unitstate.unitspeed = stat.speed;
        unitstate.unitcoin = stat.coin;
        unitstate.unithp = stat.hp;
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
            hpbarprefab.GetComponent<EnemyHpbar>().SetUpEnemy(this, this.transform);    //�����ʿ�
            hpbarprefab.GetComponentInChildren<RectTransform>().localScale = new Vector3(1 + 0.03f * (1 + unitstate.unithp * 0.1f), 1 + 0.03f * (1 + unitstate.unithp*0.3f * 0.1f), 1);
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

    //�������� ������
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
                //waypoint ����
                if (Vector3.Magnitude(MoveToPoint - this.transform.position) < 0.05f)
                {
                    currentPos = Waypoint[waypointindex];
                    waypointindex++;
                    jump = false;
                }
                
                this.transform.position = Vector3.MoveTowards(transform.position, MoveToPoint, unitspeed * Time.unscaledDeltaTime * TimeScale);

                //Debug.Log("���� ���ǵ� : " + unitspeed);
                Vector3 relativePos = Waypoint[waypointindex] - this.transform.position;
                //���� ��ġ���� Ÿ����ġ���� ���Ⱚ
                Quaternion rotationtotarget = Quaternion.LookRotation(relativePos);

                //������ rotation���� Ÿ����ġ���� ���Ⱚ���� ��ȯ �� Vector3�� ���·� ����
                Vector3 TowerDir = Quaternion.RotateTowards(this.transform.rotation, rotationtotarget, 360 * Time.unscaledDeltaTime * TimeScale).eulerAngles;

                //������ rotation���� Vector3���·� ������ �� ���
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

                //��������
                if (Waypoint[waypointindex].y > currentPos.y)
                {
                    if (!jump)
                    {
                        if (underTheSea) unitspeed = unitstate.unitspeed * 0.5f;
                        else unitspeed *= 0.8f;
                        StartCoroutine(MoveToNext(currentPos, Waypoint[waypointindex]));
                    }
                }

                //��������
                else if (Waypoint[waypointindex].y < currentPos.y)
                {
                    if (!jump)
                    {
                        if (underTheSea) unitspeed = unitstate.unitspeed * 0.5f;
                        else unitspeed *= 1.2f;
                        StartCoroutine(MoveToNext(currentPos, Waypoint[waypointindex]));
                    }
                }

                //����
                else
                {

                    float X = unitspeed - unitstate.unitspeed;

                    //��������
                    if (underTheSea) unitspeed = unitstate.unitspeed * 0.5f;
                    else
                    {
                        if (X < -0.05f) unitspeed += Time.unscaledDeltaTime * TimeScale;
                        else if (X > 0.05f) unitspeed -= Time.unscaledDeltaTime * TimeScale;
                        else unitspeed = unitstate.unitspeed;
                    }
                    //���� �������� �̵�
                    this.transform.position = Vector3.MoveTowards(transform.position, MoveToPoint, unitspeed * Time.unscaledDeltaTime * TimeScale);
                }

                //Debug.Log("���� ���ǵ� : " + unitspeed);
                Vector3 relativePos = Waypoint[waypointindex] - this.transform.position;
                //���� ��ġ���� Ÿ����ġ���� ���Ⱚ
                Quaternion rotationtotarget = Quaternion.LookRotation(relativePos);

                //������ rotation���� Ÿ����ġ���� ���Ⱚ���� ��ȯ �� Vector3�� ���·� ����
                Vector3 TowerDir = Quaternion.RotateTowards(this.transform.rotation, rotationtotarget, 360 * Time.unscaledDeltaTime * TimeScale).eulerAngles;

                //������ rotation���� Vector3���·� ������ �� ���
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

    //������ �̵�
    private Vector3 parabola(Vector3 _start, Vector3 _end, float _height, float _power, float _time)
    {
        //y���� �Ŀ����� ���̰��� time.deltatime�� ���Ѵ�.
        //time�� 1���� ���� ���� ���, 1���� Ŭ ����(����)�̱� ������ ������ �̵��� ����
        float heightvalue = -_power * _height * _time * _time + _power * _height * _time;

        //Mathf.sin
        //���� ������ ��ǥ�� ���������� ��ǥ ������
        //x��� z���� ������ ������ ������Ʈ

        Vector3 pos = Vector3.Lerp(_start, _end, _time);

        if (heightvalue <= 0)
        {
            heightvalue = 0;
        }

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

            if (this.isActiveAndEnabled)
            {
                StopCoroutine("ElectricShock");
                StartCoroutine("ElectricShock");
            }

            float Damage = underTheSea ? _damage * 2 : _damage;

            realDamage(Damage,1);
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
            ShowDamage(0, 0);
        }
        else
        {
            realDamage(realdamage,1);
        }

    }

    public void realDamage(float _damage,int damagetype)
    {
        ShowDamage(_damage, damagetype);
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

            if (unitstate.type == 0)
            {
                if (!underTheSea)
                {
                    TimeScale *= 0.5f;
                }
                else
                {
                    TimeScale = 0;
                    Iced = true;
                }
            }
            else
            {
                TimeScale *= 0.5f;
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
            Debug.Log("??");
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
       
       // GameObject damagecount = null;

        if (unitstate.type == 0)
        {
            var item = Instantiate(damagenum, cam.WorldToScreenPoint(this.transform.position), Quaternion.identity).GetComponent<HpNum>();
            item.transform.SetParent(canvas);
            item.SetUp(this.transform.position.x, this.transform.position.y, this.transform.position.z, _damage, _Block);
        }
        else
        {
            var item = Instantiate(damagenum, cam.WorldToScreenPoint(this.GetComponentInChildren<FlyEnemy>().GetBody().position), Quaternion.identity).GetComponent<HpNum>();
            item.transform.SetParent(canvas);
            Vector3 pos = this.GetComponentInChildren<FlyEnemy>().GetBody().position;
            item.SetUp(pos.x, pos.y, pos.z, _damage, _Block);
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
