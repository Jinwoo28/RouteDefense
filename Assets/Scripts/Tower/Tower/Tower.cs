using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UpgradeValue
{
    //타워 업그레이드 비용
    public int priceincrease = 0;
    public int upgradeprice = 0;
    public float damagevalue = 0;
    public float upcriticalvalue = 0;
}

[System.Serializable]
public class TowerInfo
{
    //타워의 가격
    public int towerprice = 0;
    public string towername = null;
    public float towerdamage = 0;
    public float towercritical = 0;
    public float towerrange = 0;
    public float atkdelay = 0;
}

public class Tower : MonoBehaviour
{
    //진화 할 상위 타워
    [SerializeField] private GameObject uppertower = null;

    //타워별 업그레이드 수치
    [SerializeField] UpgradeValue upgradevalue = null;
    //타워별 수치
    [SerializeField] protected TowerInfo towerinfo = null;

    //tower prefab에게 넘겨줄 상태 ui
    public GameObject[] buildstate = null;
    //미리보기 타워 프리펩
    [SerializeField] private GameObject towerpreview = null;

    //미리보기 타워 생성
    //private GameObject preview = null;

    //적 방향으로 돌아갈 포신
    //y축 회전
    [SerializeField] protected Transform towerBody;
    //x축 회전
    [SerializeField] protected Transform towerTurret;

    [SerializeField] protected Transform shootPos = null;

    //적을 검사할 레이어
    [SerializeField] private LayerMask enemylayer;

    //플레이어 coin값을 가져올 playstate
    private PlayerState playerstate = null;

    //포탄을 발사 후 공격속도 값을 구할 떄 사용할 변수
    protected float atkspeed = 0;

    //타워의 업그레이드 수준
    private int towerlevel = 0;
    
    //타워의 합체 단계 1,2,3단계
    [SerializeField] private int towerstep = 1;

    //포신이 회전할 속도
    protected float rotationspeed = 720;

    //적 타겟 Transform
    protected Transform FinalTarget = null;

    //현재 타워 아래에 있는 타일의 노드
    private Node node = null;

    //타워의 공격범위
    private ShowTowerInfo showtowerinfo = null;

    private ObjectPooling objectPooling = null;

    protected Camera cam = null;

    [SerializeField] protected GameObject AtkParticle = null;
    protected virtual void Start()
    {

        atkspeed = towerinfo.atkdelay;
        StartCoroutine(AutoSearch());
        //node.GetOnTower = true;
        MultipleSpeed.speedup += SpeedUP;
        cam = Camera.main;
    }

private void SpeedUP(int x)
{
    Time.timeScale = x;
}

    protected virtual void Update()
    {


        if (FinalTarget == null)
        {

        }
        else
        {
            RotateTurret();
        }

        //if (towermove)
        //{
        //    if (Input.GetKeyDown(KeyCode.Escape))
        //    {
        //        towermove = false;
        //        Base.SetActive(true);
        //        towercollider.enabled = true;
        //        Destroy(preview);
        //        showtowerinfo.ShowRange(this.transform,towerinfo.towerrange);

        //    }
        //}


    }

    public void SetUp(PlayerState _playerstate)
    {
        playerstate = _playerstate;
    }

    
    //타워 자동 탐색 함수
    IEnumerator AutoSearch()
    {
        while (true)
        {
            if (FinalTarget != null)
            {
                Vector3 GetDistance = FinalTarget.position - this.transform.position;
                if (Vector3.Magnitude(GetDistance)>towerinfo.towerrange)
                {
                    FinalTarget = null;
                }
            }

            //OverlapSphere : 객체 주변의 Collider를 검출
            //검출한 collider를 배열형 변수에 저장
            Collider[] E_collider = Physics.OverlapSphere(this.transform.position, towerinfo.towerrange, enemylayer);

            //가장 짧은 거리의 오브젝트 위치를 담을 변수
            Transform ShortestTarget = null;

            if (FinalTarget == null)
            {
                if (E_collider.Length > 0)
                {
                    float S_ShortestTarget = Mathf.Infinity;
                    // 거리계산에 사용할 변수 선언.

                    foreach (Collider EC in E_collider)
                    {
                        float CalDistance = Vector3.SqrMagnitude(EC.transform.position - this.transform.position);
                        // 터렛과 검출된 collider와의 거리를 담을 변수선언
                        // Vector3.Distance와 Vector3.magnitude도 거리비교를 할 수 있지만 이 둘은 Root을 통해 실제 거리를 계산하기 때문에 연산이 더 들어간다.
                        //SqrMagnitude는 실제거리*실제거리로 Root가 계산되지 않는 함수로 단순 거리비교일 때는 이것을 쓰는 게 연산 속도가 빠르다.

                        if (CalDistance < S_ShortestTarget)
                        {
                            S_ShortestTarget = CalDistance;
                            ShortestTarget = EC.transform;
                        }
                    }
                    FinalTarget = ShortestTarget;
                    //가장 거리가 짧은 대상을 최종 타겟으로 설정.
                }
            }
            yield return null;
        }
    }

    

    //공격방식을 다르게 만들 수 있는 함수

    //Y축 회전 함수
    //x축 회전 함수

    //X축 Y축 동시 회전 함수
   

    //protected void RotateXY()
    //{
    //    Vector3 relativePos = FinalTarget.position - transform.position;
    //    //현재 위치에서 타겟위치로의 방향값
    //    Quaternion rotationtotarget = Quaternion.LookRotation(relativePos);

    //    //현재의 rotation값을 타겟위치로의 방향값으로 변환 후 Vector3로 형태로 저장
    //    Vector3 TowerDir = Quaternion.RotateTowards(towerBody.rotation, rotationtotarget, rotationspeed * Time.deltaTime).eulerAngles;

    //    //현재의 rotation값에 Vector3형태로 저장한 값 사용
    //    towerBody.rotation = Quaternion.Euler(TowerDir.x, TowerDir.y, 0);

    //    if (Quaternion.Angle(towerBody.rotation, rotationtotarget) < 3.0f)
    //    {
    //        atkspeed -= Time.deltaTime;
    //        if (atkspeed <= 0)
    //        {
    //            atkspeed = towerinfo.atkdelay;
    //            int critical = Random.Range(1, 101);

    //            Debug.Log("Attack");
    //            //var BT = objectPooling.GetObject(bulletpos.position);

    //            //if (critical > towerinfo.towercritical)
    //            //    BT.SetBulletTest(FinalTarget, towerinfo.towerdamage, objectPooling);

    //            //else if (critical < towerinfo.towercritical)
    //            //    BT.SetBulletTest(FinalTarget, towerinfo.towerdamage * 2, objectPooling);
    //        }
    //    }
    //}

    protected virtual void RotateTurret()
    {
        Vector3 relativePos = FinalTarget.position - transform.position;
        //현재 위치에서 타겟위치로의 방향값
        Quaternion rotationtotarget = Quaternion.LookRotation(relativePos);

        //현재의 rotation값을 타겟위치로의 방향값으로 변환 후 Vector3로 형태로 저장
        Vector3 TowerDir = Quaternion.RotateTowards(towerBody.rotation, rotationtotarget, rotationspeed * Time.deltaTime).eulerAngles;
        Vector3 TowerDir2 = Quaternion.RotateTowards(towerTurret.rotation, rotationtotarget, rotationspeed * Time.deltaTime).eulerAngles;

        //현재의 rotation값에 Vector3형태로 저장한 값 사용
        towerBody.rotation = Quaternion.Euler(0, TowerDir.y, 0);
        towerTurret.rotation = Quaternion.Euler(TowerDir2.x+(FinalTarget.localScale.y/2), TowerDir2.y, 0);

        if (Quaternion.Angle(towerTurret.rotation, rotationtotarget) < 1.0f)
        {

            atkspeed -= Time.deltaTime;
            if (atkspeed <= 0)
            {
                atkspeed = towerinfo.atkdelay;
                int critical = Random.Range(1, 101);

                Attack();
            }
        }


    }

  

    protected virtual void Attack() { }



    public void TowerUpgrade()
    {
        
        if (playerstate.GetSetPlayerCoin >= upgradevalue.upgradeprice)
        {
            towerlevel++;
            towerinfo.towerdamage += upgradevalue.damagevalue;
            if (towerinfo.towercritical + upgradevalue.upcriticalvalue >= 100)
            {
                towerinfo.towercritical = 100;
            }
            else
            {
                towerinfo.towercritical += upgradevalue.upcriticalvalue;
            }
            playerstate.GetSetPlayerCoin = upgradevalue.upgradeprice;
            upgradevalue.upgradeprice += upgradevalue.priceincrease;
        }
    }

    public void SellTower()
    {
        playerstate.GetSetPlayerCoin = -(int)((towerinfo.towerprice + upgradevalue.upgradeprice * towerstep) * 0.7f);
        node.GetOnTower = false;
        Destroy(this.gameObject);
    }



    //public void Combine()
    //{
    //    StartCoroutine("TowerCombination");
    //}

    //IEnumerator TowerCombination()
    //{
    //    Base.SetActive(false);
    //    towercollider.enabled = false;
    //    GameObject preview = Instantiate(towerpreview);
    //    preview.GetComponent<TowerPreview>().SetUp(this.gameObject);

    //    while (true)
    //    {

    //        if (preview.GetComponent<TowerPreview>().GetCanCombine && preview.GetComponent<TowerPreview>().AlreadyTower)
    //        {
    //            Debug.Log(preview.GetComponent<TowerPreview>().GetCanCombine);
    //            Debug.Log(preview.GetComponent<TowerPreview>().AlreadyTower);
    //            if (Input.GetMouseButtonDown(0))
    //            {
    //                GameObject UpperTower = Instantiate(uppertower, preview.transform.position, Quaternion.identity);
    //                if (preview.GetComponent<TowerPreview>().GetTower != null)
    //                {
    //                    Tower tower = preview.GetComponent<TowerPreview>().GetTower;
    //                    Destroy(tower.gameObject);
    //                    UpperTower.GetComponent<Tower>().SetState(towerlevel, tower.GetTowerLevel);
    //                    UpperTower.GetComponent<Tower>().SetUp(playerstate);
    //                }

    //                Destroy(preview);
    //                Destroy(this.gameObject);
    //            }
    //        }

    //        if (Input.GetKeyDown(KeyCode.Escape))
    //        {
    //            Destroy(preview);
    //            Base.SetActive(true);
    //            towercollider.enabled = true;
    //            break;
    //        }

    //        yield return null;
    //    }

    //}

    public void TowerMove()
    {
        Debug.Log(towerpreview);
        GameObject preview = Instantiate(towerpreview,this.transform.position,Quaternion.identity);

        //preview.GetComponent<TowerPreview>().SetBuildState = buildstate;
        //preview.GetComponent<TowerPreview>().SetShowTowerInfo(showtowerinfo, towerinfo.towerrange);

        preview.GetComponent<TowerPreview>().TowerPreviewSetUp(showtowerinfo, buildstate, playerstate, towerinfo.towerrange);

        preview.GetComponent<TowerPreview>().TowerMoveSetUp(this.gameObject);

        ActiveOff();
    }

    public ShowTowerInfo SetShowTower
    {
        set
        {
            showtowerinfo = value;
        }
    }

    public void ActiveOff()
    {
 
        this.gameObject.SetActive(false);
        node.GetOnTower = false;
    }

    public void ActiveOn()
    {

        this.gameObject.SetActive(true);
        node.GetOnTower = true;
    }


    public void SetState(int _lev1, int _lev2)
    {
        int lev = (_lev1 + _lev2) / 2;
        towerlevel = lev;
        towerinfo.towerdamage += (lev * upgradevalue.damagevalue);
        towerinfo.towercritical += (lev * (float)upgradevalue.upcriticalvalue);
    }

    public void TowerStepUp(Tower _tower)
    {
        GameObject buildedtower = Instantiate(uppertower, this.transform.position, Quaternion.identity);
        buildedtower.GetComponent<Tower>().TowerSetUp(node, showtowerinfo, buildstate, playerstate);
        buildedtower.GetComponent<Tower>().SetState(_tower.GetTowerLevel, towerlevel);
        buildedtower.GetComponent<Tower>().towerinfo.towername = towerinfo.towername;
        showtowerinfo.ShowInfo(buildedtower.GetComponent<Tower>());
        showtowerinfo.ClickTower();

        Destroy(this.gameObject);
    }


    //프리뷰가 타워에게 넘겨줄 정보
    //타워가 다음 타워에게 넘겨줄 정보
    public void TowerSetUp(Node _node, ShowTowerInfo _showtowerinfo,GameObject[] _buildstate,PlayerState _playerstate)
    {
        node = _node;
        showtowerinfo = _showtowerinfo;
        buildstate = _buildstate;
        playerstate = _playerstate;
    }
    public GameObject[] GetBuildState
    {
        set
        {
            buildstate = value;
        }
    }



    public Node SetNode
    {
        get
        {
            return node;
        }
        set
        {
            node = value;
        }
    }
         
    //타워 이름
    public string Getname => towerinfo.towername;
    //타워 단계
    public int GetStep {
        get
        {
         return   towerstep;
        }
    }
    
    //타워레벨
    public int GetTowerLevel => towerlevel;
    //타워 데미지
    public float GetDamage => towerinfo.towerdamage;
    //크리티컬 확률
    public float GetCritical => towerinfo.towercritical;
    //타워 공격 속도
    public float GetSpeed => atkspeed;
    //타워 공격범위
    public float GetRange => towerinfo.towerrange;
    //타워가격
    public int Gettowerprice => towerinfo.towerprice;

    public int Gettowerupgradeprice => upgradevalue.upgradeprice;

}
