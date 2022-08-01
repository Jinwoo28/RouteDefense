using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//타워의 업그레이드 정보

public class UpgradeValue
{
    //타워 업그레이드 비용
    public int priceUprate = 0;
    public int upgradeprice = 0;
    public float UpdamageValue = 0;
    public float UpcriticalValue = 0;
}


//타워의 정보
public class TowerInfo
{
    //타워의 가격
    public int towerprice = 0;
    public string towername = null;
    public int towerstep = 0;
    public float towerdamage = 0;
    public float towercritical = 0;
    public float towerrange = 0;
    public float atkdelay = 0;
    public int CanAtk = 0;
}

[RequireComponent(typeof(AudioSource))]

public class Tower : MonoBehaviour
{
    [SerializeField] GameObject SM;

    [SerializeField] private int TowerCode = 0;

    //진화 할 상위 타워
    [SerializeField] private GameObject uppertower = null;

    //미리보기 타워 프리펩
    [SerializeField] private GameObject towerpreview = null;

    //적 방향으로 돌아갈 포신
    //y축 회전
    [SerializeField] protected Transform towerBody;
    //x축 회전
    [SerializeField] protected Transform towerTurret;

    //타워별 업그레이드 수치
    private UpgradeValue upgradeValue = new UpgradeValue();

    protected bool TowerCanWork = true;

    protected AudioSource AS = null;

    //주위를 검사할 레이어
    protected int SearchLayer;

    //타워별 수치
    protected TowerInfo towerinfo = new TowerInfo();

    //tower prefab에게 넘겨줄 상태 Ui
    public GameObject[] buildstate = null;

    //플레이어 coin값을 가져올 playstate
    private PlayerState playerstate = null;

    //포탄을 발사 후 공격속도 값을 구할 떄 사용할 변수
    protected float atkDelay = 0;

    //타워의 업그레이드 수준
    private int towerlevel = 0;

    //포신이 회전할 속도
    protected float rotationspeed = 720;

    //타겟 Transform
    protected Transform FinalTarget = null;

    //현재 타워 아래에 있는 타일의 노드
    private Node node = null;


    //타워의 공격범위
    private ShowTowerInfo showtowerinfo = null;

    private bulletpolling objectPooling = null;

    protected virtual void Awake()
    {
       //this.transform.rotation = Quaternion.Euler(0, 180,0);
       TowerSetUp(TowerDataSetUp.GetData(TowerCode));
    }
    private void TowerSetUp(TowerDataFrame towerdata)
    {
        towerinfo.atkdelay = towerdata.delay;
        towerinfo.towerrange = towerdata.range;
        towerinfo.towercritical = towerdata.critical;
        towerinfo.towerdamage = towerdata.damage;
        towerinfo.towername = towerdata.name;
        towerinfo.towerprice = towerdata.towerPrice;
        towerinfo.towerstep = towerdata.towerStep;
        towerinfo.CanAtk = towerdata.atkType;

        upgradeValue.UpdamageValue = towerdata.upgradAtk;
        upgradeValue.UpcriticalValue = towerdata.upgradCri;
        upgradeValue.upgradeprice = towerdata.upgradPrice;
        upgradeValue.priceUprate = 0;
    }

    protected virtual void Start()
    {
        AS = this.GetComponent<AudioSource>();
        Instantiate(SM, this.transform.position, Quaternion.identity).GetComponent<SoundManager>().InsthisObj(1);
        atkDelay = towerinfo.atkdelay;
        StartCoroutine("AutoSearch");
        node.GetOnTower = true;
        MultipleSpeed.speedup += SpeedUP;
        AS.volume = PlayerPrefs.GetFloat("ESound");
        SoundSettings.effectsound += SoundChange;

        upgradeValue.upgradeprice = (int)(upgradeValue.upgradeprice*SkillSettings.PassiveValue("UpTowerDown"));
        towerinfo.towerprice = (int)(towerinfo.towerprice * SkillSettings.PassiveValue("SetTowerDown"));

        sellprice += (int)(towerinfo.towerprice * SkillSettings.PassiveValue("SellTowerUp"));
    }

    private void SpeedUP(int x)
    {
        Time.timeScale = x;
    }

    private void OnDestroy()
    {
        SoundSettings.effectsound -= SoundChange;
    }

    private void SoundChange(float x)
    {
        if (AS != null)
        {
            AS.volume = x;
        }
    }

    protected bool SoundStop = true;

    public void SetUp(PlayerState _playerstate)
    {
        playerstate = _playerstate;
    }

    
    //타워 자동 탐색 함수
    protected virtual IEnumerator AutoSearch()
    {
        yield return null;
    }

    protected bool Atking = false;

    public void TowerUpgrade()
    {
        int upgradeprice = (int)(upgradeValue.upgradeprice * SkillSettings.PassiveValue("UpTowerDown"));

        if (playerstate.GetSetPlayerCoin >= upgradeprice)
        {
            Instantiate(SM, this.transform.position, Quaternion.identity).GetComponent<SoundManager>().InsthisObj(2);
            sellprice += upgradeprice;

            towerlevel++;
            towerinfo.towerdamage += upgradeValue.UpdamageValue;
            if (towerinfo.towercritical + upgradeValue.UpcriticalValue >= 100)
            {
                towerinfo.towercritical = 100;
            }
            else
            {
                towerinfo.towercritical += upgradeValue.UpcriticalValue;
            }

            playerstate.GetSetPlayerCoin = (int)(upgradeValue.upgradeprice * SkillSettings.PassiveValue("UpTowerDown"));
            upgradeValue.upgradeprice += (int)(upgradeValue.priceUprate * SkillSettings.PassiveValue("UpTowerDown"));
        }
        else
        {
            Instantiate(SM, this.transform.position, Quaternion.identity).GetComponent<SoundManager>().InsthisObj(0);
        }
    }

    public void SellTower()
    {
        GameManager.buttonOff();

        playerstate.GetSetPlayerCoin = (-sellprice);
        node.GetOnTower = false;
        Destroy(this.gameObject);
        Instantiate(SM, this.transform.position, Quaternion.identity).GetComponent<SoundManager>().InsthisObj(3);
    }

    private int sellprice = 0;
    public int GetSetSellPrice { get => sellprice; set => sellprice = value; }

    public void TowerMove()
    {
        GameManager.buttonOff();
        if (TowerCanWork)
        {
            Instantiate(SM, this.transform.position, Quaternion.identity).GetComponent<SoundManager>().InsthisObj(2);
            TowerCanWork = true;

            GameObject preview = Instantiate(towerpreview, this.transform.position, Quaternion.identity);
            showtowerinfo.SetTowerinfoOff();
            //preview.GetComponent<TowerPreview>().SetBuildState = buildstate;
            //preview.GetComponent<TowerPreview>().SetShowTowerInfo(showtowerinfo, towerinfo.towerrange);

            preview.GetComponent<TowerPreview>().TowerPreviewSetUp(showtowerinfo, buildstate, playerstate, towerinfo.towerrange);

            preview.GetComponent<TowerPreview>().TowerMoveSetUp(this.gameObject);

            ActiveOff();
        }
        else
        {
            Instantiate(SM, this.transform.position, Quaternion.identity).GetComponent<SoundManager>().InsthisObj(1);
        }
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
        showtowerinfo.ShowInfo(this);
        showtowerinfo.ShowRange(this.gameObject.transform, towerinfo.towerrange);
        this.gameObject.SetActive(true);
        node.GetOnTower = true;
        StartCoroutine("AutoSearch");
    }


    public void SetState(int _lev1, int _lev2)
    {
        int lev = (_lev1 + _lev2) / 2;
        towerlevel = lev;
        towerinfo.towerdamage += (lev * upgradeValue.UpdamageValue);
        towerinfo.towercritical += (lev * (float)upgradeValue.UpcriticalValue);
        upgradeValue.upgradeprice += towerlevel * upgradeValue.priceUprate;
       
    }

    public void TowerStepUp(Tower _tower)
    {
        GameObject buildedtower = Instantiate(uppertower, this.transform.position, Quaternion.identity);

        buildedtower.GetComponent<Tower>().TowerSetUp(node, showtowerinfo, buildstate, playerstate);
        buildedtower.GetComponent<Tower>().SetState(_tower.GetTowerLevel, towerlevel);
        buildedtower.GetComponent<Tower>().towerinfo.towername = towerinfo.towername;

        buildedtower.GetComponent<Tower>().GetSetSellPrice += 
            ((sellprice - towerinfo.towerprice) + (_tower.sellprice-_tower.towerinfo.towerprice));



        showtowerinfo.ShowInfo(buildedtower.GetComponent<Tower>());
        showtowerinfo.ClickTower();

        Destroy(this.gameObject);
    }

    public void TowerUpSkill()
    {
        GameObject buildedtower = Instantiate(uppertower, this.transform.position, Quaternion.identity);
        buildedtower.GetComponent<Tower>().TowerSetUp(node, showtowerinfo, buildstate, playerstate);
        buildedtower.GetComponent<Tower>().SetState(1, towerlevel);
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

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Tile"))
        {
            other.GetComponent<Node>().GetOnTower = true;
        }
    }

    #region TowerProperties

    public bool SetTowerCanWork
    {
        get => TowerCanWork;
        set => TowerCanWork = value;
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


    public bool GetCanWork { get => TowerCanWork; set => TowerCanWork = value; }

    //타워 이름
    public string Getname => towerinfo.towername;
    //타워 단계
    public int GetStep { get => towerinfo.towerstep; }
    //타워레벨
    public int GetTowerLevel => towerlevel;
    //타워 데미지
    public float GetDamage => towerinfo.towerdamage;
    //크리티컬 확률
    public float GetCritical => towerinfo.towercritical;
    //타워 공격 속도
    public float GetSpeed => atkDelay;
    //타워 공격범위
    public float GetRange => towerinfo.towerrange;
    //타워가격
    public int Gettowerprice => towerinfo.towerprice;

    public int Gettowerupgradeprice => (int)(upgradeValue.upgradeprice * SkillSettings.PassiveValue("UpTowerDown"));

    public float GetTowerUPDamage => upgradeValue.UpdamageValue;
    public float GetTowerUpCri => upgradeValue.UpcriticalValue;
    public int GetTowerCode => TowerCode;

    #endregion
}
