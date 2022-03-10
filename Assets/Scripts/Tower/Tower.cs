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
    public float towerspeed = 0;
}



public class Tower : MonoBehaviour
{
    //진화할 상위 타워
    [SerializeField] private GameObject uppertower = null;
    //미리보기 타워
    [SerializeField] private GameObject towerpreview = null;
    //타워 미리보기 프리펩
    private GameObject preview = null;

    //합체할 때 잠시 사라질 오브젝트와 collider
    [SerializeField] private GameObject Base = null;
    private BoxCollider towercollider = null;

    //적 방향으로 돌아갈 포신
    [SerializeField] private Transform TowerBody;

    //총알을 발사할 위치와 총알 프리펩
    [SerializeField] private Transform bulletpos;
    [SerializeField] private GameObject bullet;

    //적을 검사할 레이어
    [SerializeField] private LayerMask enemylayer;

    private PlayerState playerstate = null;

    [SerializeField] UpgradeValue upgradevalue = null;
    [SerializeField] TowerInfo towerinfo = null;

    //포탄을 발사 후 공격속도 값을 구할 떄 사용할 변수
    private float atkspeed = 0;

    //타워의 업그레이드 수준
    private int towerlevel = 1;
    
    //타워의 합체 단계 1,2,3단계
    private int towerstep = 1;

    //포신이 회전할 속도
    private float rotationspeed = 1080;

    //적 타겟 Transform
    private Transform FinalTarget = null;

    private bool towermove = false;

    private Node node = null;

    public ShowTowerInfo showtowerinfo = null;
   

    void Start()
    {
        towercollider = this.GetComponent<BoxCollider>();
        atkspeed = towerinfo.towerspeed;
        StartCoroutine("AutoSearch");
        node.GetComponent<Node>().GetOnTower = true;

        Base.SetActive(true);
        towercollider.enabled = true;
        MultipleSpeed.speedup += SpeedUP;
    }

    

private void SpeedUP(int x)
{
    Time.timeScale = x;
}
public void SetActiveOn()
    {

        Base.SetActive(true);
        towercollider.enabled = true;
    }

    private void Update()
    {
        if (FinalTarget == null)
        {
 
        }
        else
        {
             RotateToTarget();
        }

        if (towermove)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                towermove = false;
                Base.SetActive(true);
                towercollider.enabled = true;
                Destroy(preview);
                showtowerinfo.ShowRange(this.transform,towerinfo.towerrange);
                Debug.Log("2222");
            }
        }


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

    

    private void RotateToTarget()
    {
        Vector3 relativePos = FinalTarget.position - transform.position;
        //현재 위치에서 타겟위치로의 방향값
        Quaternion rotationtotarget = Quaternion.LookRotation(relativePos);

        
        //현재의 rotation값을 타겟위치로의 방향값으로 변환 후 Vector3로 형태로 저장
        Vector3 TowerDir = Quaternion.RotateTowards(TowerBody.rotation, rotationtotarget, rotationspeed * Time.deltaTime).eulerAngles;

        Debug.DrawLine(this.transform.position, FinalTarget.position, Color.red);


        //현재의 rotation값에 Vector3형태로 저장한 값 사용
        TowerBody.rotation = Quaternion.Euler(TowerDir.x, TowerDir.y, 0);




        if (Quaternion.Angle(TowerBody.rotation, rotationtotarget) < 3.0f)
        {
            atkspeed -= Time.deltaTime;
            if (atkspeed <= 0)
            {
                atkspeed = towerinfo.towerspeed;
                int critical = Random.Range(1, 101);
                
                GameObject BT = Instantiate(bullet, bulletpos.position, Quaternion.identity);
                
                if (critical > towerinfo.towercritical) 
                BT.GetComponent<BulletTest>().SetBulletTest(FinalTarget, towerinfo.towerdamage);

                else if(critical<towerinfo.towercritical)
                    BT.GetComponent<BulletTest>().SetBulletTest(FinalTarget, towerinfo.towerdamage*2);
            }
        }

    }

    public void TowerUpgrade()
    {
        
        if (playerstate.GetSetPlayerCoin >= upgradevalue.upgradeprice)
        {
            towerlevel++;
            towerinfo.towerdamage += upgradevalue.damagevalue;
            towerinfo.towercritical += upgradevalue.upcriticalvalue;
            playerstate.GetSetPlayerCoin = upgradevalue.upgradeprice;
            upgradevalue.upgradeprice += upgradevalue.priceincrease;
        }
    }

    public void SellTower()
    {
        playerstate.GetSetPlayerCoin = -(int)((towerinfo.towerprice + upgradevalue.upgradeprice * towerstep) * 0.7f);
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

        towermove = true;
        Base.SetActive(false);
        towercollider.enabled = false;
        preview = Instantiate(towerpreview,this.transform.position,Quaternion.identity);
        preview.GetComponent<TowerPreview>().SetShowTowerInfo(showtowerinfo, towerinfo.towerrange);
        preview.GetComponent<TowerPreview>().SetUp(this.gameObject);
        preview.GetComponent<TowerPreview>().SetOriginTower=this.gameObject;

    }


    public void SetState(int _lev1, int _lev2)
    {
        int lev = (_lev1 + _lev2) / 2;
        Debug.Log(lev);
        towerlevel = lev;
        towerinfo.towerdamage += (lev * upgradevalue.damagevalue);
        towerinfo.towercritical += (lev * (float)upgradevalue.upcriticalvalue);
    }

    public void TowerStepUp(Tower _tower)
    {
        GameObject buildedtower =Instantiate(uppertower, this.transform.position, Quaternion.identity);
        buildedtower.GetComponent<Tower>().SetNode = node;
        buildedtower.GetComponent<Tower>().SetState(_tower.GetTowerLevel, towerlevel);
        buildedtower.GetComponent<Tower>().GetStep = 1;
        Debug.Log(_tower.GetTowerLevel + " : " + towerlevel);
        buildedtower.GetComponent<Tower>().SetUp(playerstate);
        buildedtower.GetComponent<Tower>().showtowerinfo = showtowerinfo;

        showtowerinfo.ShowInfo(buildedtower.GetComponent<Tower>());
        showtowerinfo.SetTowerinfo();

        Destroy(this.gameObject);
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

    public void TowerDestroy()
    {
        Destroy(this.gameObject);
    }
        
    
    //타워 이름
    public string Getname => towerinfo.towername;
    //타워 단계
    public int GetStep {
        get
        {
         return   towerstep;
        }
        set
        {
            towerstep += 1;
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
