using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


//포탑과 타일을 직렬화 시켜 하이라키 창에서 관리
[System.Serializable]
public class BuildTower
{
    public GameObject preview = null;
    public GameObject builditem = null;
}

public class BuildManager : MonoBehaviour
{
    private const string ATK_POWER = "공격력 : ";
    private const string ATK_SPEED = "공격속도 : ";
    private const string TOWER_COST = "비용 : ";

    [SerializeField] private GameObject[] buildstate = null;
    [SerializeField] private ShowTowerInfo showtowerinfo = null;
    [SerializeField] private BuildTower[] buildtower = null;
    [SerializeField] private PlayerState playerstate= null;

    [SerializeField] private GameObject BuildInfoPanel = null;
    [SerializeField] private TextMeshProUGUI[] info = null;
    [SerializeField] private Image towerimage = null;
    
    private AlertSetting alertSetting = new AlertSetting();
    int playercoin = 0;

    //타워 미리보기 프리펩
    private GameObject preview = null;
    //타워 프리펩
    private GameObject craft = null;

    //타일위에 다른 포탑이 있는지 확인
    //private bool alreadyontile = false;

    //현재 타일 위에 프리뷰 포탑이 위치해있는지
    //private bool ontile = false;

    private static List<FireGunTower> firetowerlist = new List<FireGunTower>();
    private static List<TeslaTower> teslatowerlist = new List<TeslaTower>();

    private static bool iswet = false;

    private int TowerCode = 0;
    private int PrefabsNum = 0;
    private bool IsBTowerPanel = false;
    private bool IsBMouseOnPanel = false;

    private int towerprice;
    private int upgradeprice;

    //프리뷰 포탑이 활성화 되어있는지
    private bool towerpreviewActive = false;

    //private bool canbuild = false;

    Vector3 mousepos = Vector3.zero;

    public GameObject Testtile = null;

    public void TOnPanel()
    {
        IsBMouseOnPanel = true;
    }
    public void FOnPanel()
    {
        IsBMouseOnPanel = false;
    }

    public void ClickBtnCode(int Code)
    {
        TowerCode = Code;
        IsBTowerPanel = true;
    }

    public void OnMouseBuildTowerPanel(int TowerCode)
    {
        BuildInfoPanel.SetActive(true);

        var towerCode = TowerDataSetUp.GetData(TowerCode);
        info[0].text = towerCode.name;
        info[1].text = towerCode.towerInfo;
        info[2].text = ATK_POWER + towerCode.damage;
        info[3].text = ATK_SPEED + towerCode.delay;
        info[4].text = TOWER_COST + towerCode.towerPrice * SkillSettings.PassiveValue("SetTowerDown");
        towerimage.sprite = Resources.Load<Sprite>("Image/Tower/" + (towerCode.name+ towerCode.towerStep));
    }
    public void ExitMouseBUildTowerPanel()
    {
        BuildInfoPanel.SetActive(false);
    }

    public void ClickBuild()
    {
        SlotClick(PrefabsNum);
        OffTowerPanel();
    }

    private void OffTowerPanel()
    {
        IsBTowerPanel = false;
        FOnPanel();
        BuildInfoPanel.SetActive(false);
    }

    public static void ActiveTower(FireGunTower firetower)
    {
        firetowerlist.Add(firetower);
        firetower.ChangeWet(iswet);
    }
    public static void ReturnActiveTower(FireGunTower firetower)
    {
        firetowerlist.Remove(firetower);
    }

    public static void ActiveTowerTesla(TeslaTower tesla)
    {
        teslatowerlist.Add(tesla);
    }
    public static void ReturnActiveTowerTesla(TeslaTower tesla)
    {
        teslatowerlist.Remove(tesla);
    }


    public static void Rained(bool wet)
    {
        iswet = wet;
        for (int i = 0; i < firetowerlist.Count; i++)
        {
            firetowerlist[i].ChangeWet(wet);
        }
        for (int i = 0; i < teslatowerlist.Count; i++)
        {
            teslatowerlist[i].ChangeWet(wet);
        }
    }


    private void OnDestroy()
    {
        iswet = false;
        firetowerlist.Clear();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            SlotClick(0);
        }

        if (IsBTowerPanel)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                OffTowerPanel();
            }
        }

        if (!IsBMouseOnPanel)
        {
            if (Input.GetMouseButtonDown(0))
            {
                OffTowerPanel();
            }
        }

        playercoin = playerstate.GetSetPlayerCoin;

        // if (preview == null) towerpreviewActive = false;

        if (towerpreviewActive)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                CancelTower();
            }
        }
    }

    public void CancelTower()
    {
        towerpreviewActive = false;
        playerstate.GetSetPlayerCoin = -towerprice;
        preview.GetComponent<TowerPreview>().DestroyThis();
    }

    //포탑 짓기
    //타일이 없는 곳, 이미 포탑이 있는 곳, 길이 있는 곳은 포탑 짓기 불가

    //검사항목
    //1. 타일이 있는 곳인가.
    //2. 놓으려는 위치에 현재 포탑이 있는가
    //3. 길찾기를 위해 walkable을 true로 바꾼 곳인가.

    public void SlotClick(int _slotnum)
    {
        GameManager.buttonOff();

        if (towerpreviewActive)
        {
            playerstate.GetSetPlayerCoin = -towerprice;
            preview.GetComponent<TowerPreview>().DestroyThis();
            towerpreviewActive = false;
        }

        showtowerinfo.SetTowerinfoOff();

        float price = TowerDataSetUp.GetData(buildtower[_slotnum].builditem.GetComponent<Tower>().GetTowerCode).towerPrice * SkillSettings.PassiveValue("SetTowerDown");
        towerprice = (int)price;
   
        if (playercoin >= towerprice)
        {
            if (!towerpreviewActive)
            {
                alertSetting.PlaySound(AlertKind.Click,this.gameObject);
                towerpreviewActive = true;

                preview = Instantiate(buildtower[_slotnum].preview, Vector3.zero, Quaternion.identity);

                var towerPreview = preview.GetComponent<TowerPreview>();
                towerPreview.TowerPreviewSetUp(showtowerinfo, buildstate, playerstate, TowerDataSetUp.GetData(buildtower[_slotnum].builditem.GetComponent<Tower>().GetTowerCode).range);
                towerPreview.FirstSetUp(buildtower[_slotnum].builditem,this);
                playerstate.GetSetPlayerCoin = towerprice;
            }
        }
        else
        {
            alertSetting.PlaySound(AlertKind.Cant, this.gameObject);
            playerstate.ShowNotEnoughMoneyCor();
        }
    }




    public bool IsGettowerpreviewActive
    {
        set
        {
            towerpreviewActive = value;
        }
    }


    

    //private void TowerPos()
    //{
    //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //    RaycastHit hit;
    //    if (Physics.Raycast(ray, out hit, Mathf.Infinity))
    //    {
    //        mousepos = hit.point;
    //    }

    //    if (preview.GetComponent<TowerPreview>().CanBuildable())
    //    {
            
    //        if (Input.GetMouseButtonDown(0))
    //        {
    //            GameObject buildtower = Instantiate(craft, preview.transform.position,Quaternion.identity);
    //            Node node = preview.GetComponent<TowerPreview>().GetTowerNode;
    //            node.GetOnTower = true;
    //            buildtower.GetComponent<Tower>().SetUp(playerstate);
    //            buildtower.GetComponent<Tower>().SetNode=node;
    //            Destroy(preview);
    //            towerpreviewActive = false;

    //            showtowerinfo.ShowRange(buildtower.transform, buildtower.GetComponent<Tower>().GetRange);
    //            showtowerinfo.ClickTower();
                
    //        }
    //    }  
        
    //    if (Input.GetKeyDown(KeyCode.Escape))
    //    {
    //        towerpreviewActive = false;
    //        playerstate.GetSetPlayerCoin = -towerprice;
    //        showtowerinfo.RangeOff();
    //        Destroy(preview);
    //    }
    //}

}
