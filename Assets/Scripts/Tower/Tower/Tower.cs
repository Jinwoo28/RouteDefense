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
    [SerializeField] private int TowerCode = 0;
    
    [SerializeField] private GameObject alterSound;
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
    private int sellprice = 0;

    //포신이 회전할 속도
    protected float rotationspeed = 720;

    //타겟 Transform
    protected Transform FinalTarget = null;

    //현재 타워 아래에 있는 타일의 노드
    private Node node = null;

    protected bool isAtking = false;

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
        var sound = Instantiate(alterSound, this.transform.position, Quaternion.identity).GetComponent<AlertSetting>();
        sound.PlaySound(AlertKind.Build,sound.gameObject);
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

    //타워 업그레이드
    public void TowerUpgrade()
    {
        int upgradeprice = (int)(upgradeValue.upgradeprice * SkillSettings.PassiveValue("UpTowerDown"));

        if (playerstate.GetSetPlayerCoin >= upgradeprice)
        {
            var sound = Instantiate(alterSound, this.transform.position, Quaternion.identity).GetComponent<AlertSetting>();
            sound.PlaySound(AlertKind.OK, sound.gameObject);
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
            var sound = Instantiate(alterSound, this.transform.position, Quaternion.identity).GetComponent<AlertSetting>();
            sound.PlaySound(AlertKind.Cant, sound.gameObject);
        }
    }

    //타워 판매
    public void SellTower()
    {
        GameManager.buttonOff();

        playerstate.GetSetPlayerCoin = (-sellprice);
        node.GetOnTower = false;
        var sound = Instantiate(alterSound, this.transform.position, Quaternion.identity).GetComponent<AlertSetting>();
        sound.PlaySound(AlertKind.OK, sound.gameObject);
        Destroy(this.gameObject);
    }

    //타워 이동
    public void TowerMove()
    {
        GameManager.buttonOff();
        if (TowerCanWork)
        {
            var sound = Instantiate(alterSound, this.transform.position, Quaternion.identity).GetComponent<AlertSetting>();
            sound.PlaySound(AlertKind.OK, sound.gameObject);
            TowerCanWork = true;

            GameObject preview = Instantiate(towerpreview, this.transform.position, Quaternion.identity);
            showtowerinfo.SetTowerinfoOff();

            var towerPreview = preview.GetComponent<TowerPreview>();
            towerPreview.TowerPreviewSetUp(showtowerinfo, buildstate, playerstate, towerinfo.towerrange);
            towerPreview.TowerMoveSetUp(this.gameObject);

            ActiveOff();
        }
        else
        {
            var sound = Instantiate(alterSound, this.transform.position, Quaternion.identity).GetComponent<AlertSetting>();
            sound.PlaySound(AlertKind.Cant, sound.gameObject);
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

    //타워 업그레이드 수치
    public void SetState(int _lev1, int _lev2)
    {
        int lev = (_lev1 + _lev2) / 2;
        towerlevel = lev;
        towerinfo.towerdamage += (lev * upgradeValue.UpdamageValue);
        towerinfo.towercritical += (lev * (float)upgradeValue.UpcriticalValue);
        upgradeValue.upgradeprice += towerlevel * upgradeValue.priceUprate;
    }

    //타워 진화
    public void TowerStepUp(Tower _tower)
    {
        GameObject buildedtower = Instantiate(uppertower, this.transform.position, Quaternion.identity);

        var tower = buildedtower.GetComponent<Tower>();

        tower.TowerSetUp(node, showtowerinfo, buildstate, playerstate);
        tower.SetState(_tower.GetTowerLevel, towerlevel);
        tower.towerinfo.towername = towerinfo.towername;
        tower.GetSetSellPrice += 
            ((sellprice - towerinfo.towerprice) + (_tower.sellprice-_tower.towerinfo.towerprice));

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

    public bool IsSetTowerCanWork
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

    public int GetSetSellPrice { get => sellprice; set => sellprice = value; }
    public bool IsGetCanWork { get => TowerCanWork; set => TowerCanWork = value; }

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
