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
    [SerializeField] private SoundManager SM;
    [SerializeField] private GameObject[] buildstate = null;

    //string towername = null;


    [SerializeField] private ShowTowerInfo showtowerinfo = null;
    [SerializeField] private BuildTower[] buildtower = null;
    [SerializeField] private PlayerState playerstate= null;
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

    [SerializeField] private GameObject BuildInfoPanel = null;
    [SerializeField] private TextMeshProUGUI[] info = null;
    [SerializeField] private Image towerimage = null;
    private int TowerCode = 0;
    private int PrefabsNum = 0;
    private bool BTowerPanel = false;
    private bool BMouseOnPanel = false;

    public void TOnPanel()
    {
        BMouseOnPanel = true;
    }
    public void FOnPanel()
    {
        BMouseOnPanel = false;
    }

    public void ClickBtnCode(int Code)
    {
        TowerCode = Code;
        BTowerPanel = true;
    }

    public void OnMouseBuildTowerPanel(int TowerCode)
    {
        BuildInfoPanel.SetActive(true);

        info[0].text = TowerDataSetUp.GetData(TowerCode).name;
        info[1].text = TowerDataSetUp.GetData(TowerCode).towerInfo;
        info[2].text = "공격력 : " + TowerDataSetUp.GetData(TowerCode).damage;
        info[3].text = "공격속도 : "+TowerDataSetUp.GetData(TowerCode).delay;
        info[4].text = "비용 : " + TowerDataSetUp.GetData(TowerCode).towerPrice * SkillSettings.PassiveValue("SetTowerDown");
        towerimage.sprite = Resources.Load<Sprite>("Image/Tower/" + (TowerDataSetUp.GetData(TowerCode).name+ TowerDataSetUp.GetData(TowerCode).towerStep));
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
        BTowerPanel = false;
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


    //프리뷰 포탑이 활성화 되어있는지
    private bool towerpreviewActive = false;
    
    //private bool canbuild = false;


    Vector3 mousepos = Vector3.zero;

    public GameObject Testtile = null;

    private void Start()
    {
        
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.T))
        {
            SlotClick(0);
        }

        if (BTowerPanel)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                OffTowerPanel();
            }
        }

        if (!BMouseOnPanel)
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
       // Debug.Log(TowerDataSetUp.GetData(buildtower[_slotnum].builditem.GetComponent<Tower>().GetTowerCode).TowerPrice * SkillSettings.PassiveValue("SetTowerDown"));
        
            if (playercoin >= towerprice)
            {
                if (!towerpreviewActive)
                {
                SM.TurnOnSound(0);
                    towerpreviewActive = true;

                    preview = Instantiate(buildtower[_slotnum].preview, Vector3.zero, Quaternion.identity);

                    preview.GetComponent<TowerPreview>().TowerPreviewSetUp(showtowerinfo, buildstate, playerstate, TowerDataSetUp.GetData(buildtower[_slotnum].builditem.GetComponent<Tower>().GetTowerCode).range);
                    preview.GetComponent<TowerPreview>().FirstSetUp(buildtower[_slotnum].builditem,this);
                    playerstate.GetSetPlayerCoin = towerprice;
                }
            }
            else
            {
            SM.TurnOnSound(6);
            playerstate.ShowNotEnoughMoneyCor();
            }
    }




    public bool GettowerpreviewActive
    {
        set
        {
            towerpreviewActive = value;
        }
    }

    int towerprice;
    int upgradeprice;

    

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
