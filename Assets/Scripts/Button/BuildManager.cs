using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//포탑과 타일을 직렬화 시켜 하이라키 창에서 관리
[System.Serializable]
public class BuildTower
{
    public GameObject preview = null;
    public GameObject builditem = null;
}


public class BuildManager : MonoBehaviour
{
    string towername = null;

    [SerializeField] private BuildTower[] buildtower = null;
    [SerializeField] private PlayerState playerstate= null;
    int playercoin = 0;

    //타워 미리보기 프리펩
    private GameObject preview = null;
    //타워 프리펩
    private GameObject craft = null;

    //타일위에 다른 포탑이 있는지 확인
    private bool alreadyontile = false;

    //현재 타일 위에 프리뷰 포탑이 위치해있는지
    private bool ontile = false;

    //프리뷰 포탑이 활성화 되어있는지
    private bool towerpreviewActive = false;
    
    
    private bool canbuild = false;


    Vector3 mousepos = Vector3.zero;

    public GameObject Testtile = null;

    private MapManager mapmanager = null;
    private bool addtileactive = false;

    private void Start()
    {
        mapmanager = this.GetComponent<MapManager>();
    }

    private void Update()
    {
        addtileactive = mapmanager.GetSetAddTile;

        playercoin = playerstate.GetSetPlayerCoin;

        if (towerpreviewActive)
        {
            TowerPos();
        }
    }


    //포탑 짓기
    //타일이 없는 곳, 이미 포탑이 있는 곳, 길이 있는 곳은 포탑 짓기 불가

    //검사항목
    //1. 타일이 있는 곳인가.
    //2. 놓으려는 위치에 현재 포탑이 있는가
    //3. 길찾기를 위해 walkable을 true로 바꾼 곳인가.

    public void SlotClick(int _slotnum)
    {
        int towerprice = buildtower[_slotnum].builditem.GetComponent<Tower>().Gettowerprice;

        if (!addtileactive)
        {
            if (playercoin >= towerprice)
            {
                if (!towerpreviewActive)
                {
                    mapmanager.GetSetTileChange = false;
                    towerpreviewActive = true;
                    preview = Instantiate(buildtower[_slotnum].preview, Vector3.zero, Quaternion.identity);
                    craft = buildtower[_slotnum].builditem;

                    playerstate.GetSetPlayerCoin = towerprice;
                }
            }
        }
    }

    int towerprice;
    int upgradeprice;


    private void TowerPos()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            mousepos = hit.point;
        }

        if (preview.GetComponent<TowerPreview>().CanBuildable())
        {
            if (Input.GetMouseButtonDown(0))
            {
                GameObject buildtower = Instantiate(craft, preview.transform.position,Quaternion.identity);
                Node node = preview.GetComponent<TowerPreview>().GetTowerNode;
                node.GetOnTower = true;
                buildtower.GetComponent<Tower>().SetUp(playerstate);
                Destroy(preview);
                towerpreviewActive = false;
                
                
            }
        }  
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            towerpreviewActive = false;
            playerstate.GetSetPlayerCoin = -towerprice;
            Destroy(preview);
        }
    }

}
