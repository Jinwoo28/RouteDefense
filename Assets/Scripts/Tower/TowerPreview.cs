using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TowerPreview : MonoBehaviour
{

    private GameObject[] buildstate = null;
    //0 = 업그레이드
    //1 = 이동가능
    //2 = 이동불가

    //해당 타일에 이미 타워가 있을 경우
    private bool alreadytower = false;

    //타일 위인지 검사
    private bool ontile = false;

    private bool OnWater = false;
    public bool SetWeater { set => OnWater = value; }

    //이동하는 길목인지 확인
    private bool checkOnroute = false;
    
    //build할 수 있는지 최종 여부
    private bool canbuildable = false;

    //합체가능 한지 여부
    private bool CanCombination = false;
    
    [SerializeField] private LayerMask layermask;

    //지어질 타워
    private GameObject buildTower = null;

    //프리뷰가 가진 타워의 단계
    private int towerstep = 0;

    public string towername = null;

    public PlayerState playerstate = null;

    private bool thisActive = true;


    DetectObject detector = new DetectObject();

    private void UiStateChange(int _i)
    {

            for (int i = 0; i < 3; i++)
            {
                if (i == _i)
                {
                    buildstate[i].SetActive(true);
                }
                else
                {
                    buildstate[i].SetActive(false);
                }
            }
        
    }

     private void UiStateOff()
    {
        for (int i = 0; i < 3; i++)
        {
            buildstate[i].SetActive(false);
        }
    }

    private Node towernode;

    private Tower tower = null;
    private ShowTowerInfo showtowerinfo = null;
    private BuildManager buildmanager = null;
    private float range = 0;

    //이동할 때 원래 타워의 정보
    private GameObject Origintower = null;

    private void Start()
    {

    }

    private void Update()
    {
        Camera cam = Camera.main;
        Vector3 thisPos = this.transform.position;
        thisPos.y += 2.0f;
        for (int i = 0; i < 3; i++)
        {
            buildstate[i].transform.position = cam.WorldToScreenPoint(thisPos);
        }

        if (Origintower != null)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                DestroyThis();
                Origintower.GetComponent<Tower>().ActiveOn();
            }
        }

       // Debug.Log("합체 가능 : "+CanCombination + " -- "+"타워있음 : " + alreadytower);
    }

    IEnumerator BuildTower()
    {
        while (true)
        {
            // Debug.Log(showtowerinfo);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layermask))
            {
                if (hit.collider.CompareTag("Tile"))
                {
                    int X = hit.collider.GetComponent<Node>().gridX;
                    int Z = hit.collider.GetComponent<Node>().gridY;
                    float Y = hit.collider.transform.localScale.y;
                    this.transform.position = new Vector3(X, (Y / 2), Z);

                    if (thisActive)
                    {
                        showtowerinfo.ShowRange(this.gameObject.transform, range);
                    }
                    //타일 위에 있는지
                    ontile = true;
                    //이미 타워가 있는지
                    alreadytower = hit.collider.GetComponent<Node>().GetOnTower;
                    //이동 길목인지
                    checkOnroute = hit.collider.GetComponent<Node>().Getwalkable;

                    towernode = hit.collider.GetComponent<Node>();
                }
                else
                {
                    ontile = false;
                }
            }

            if (!EventSystem.current.IsPointerOverGameObject())
            {
                if (thisActive)
                {
                    //현재 위치의 설치가능 여부에 따라 preview 위에 ui표시
                    //불가능
                    if ((alreadytower && !CanCombination) || (CanCombination && towerstep == 3) 
                        || (CanCombination && tower.GetStep == 3) || checkOnroute || OnWater
                        || ((towernode!=null)&&towernode.SetOnObstacle))
                    {
                        UiStateChange(2);
                    }

                    //합체
                    else if ((alreadytower && CanCombination && towerstep != 3 && tower.GetStep != 3) && !OnWater)
                    {
                        //  Debug.Log(alreadytower + " : " + CanCombination + " : " + (towerstep != 3 && tower.GetStep != 3));
                        UiStateChange(0);
                    }
                    //가능
                    else
                    {
                        UiStateChange(1);
                    }
                }


                //타워 합체
                if (ontile && !checkOnroute && !OnWater)
                {
                    if (CanCombination && alreadytower)
                    {
                        if (tower != null)
                        {
                            //if (tower.GetStep != 3)
                            //{
                            if (Input.GetMouseButtonDown(0))
                            {
                                if (Origintower != null)
                                {
                                    tower.TowerStepUp(Origintower.GetComponent<Tower>());
                                    Destroy(Origintower);
                                }
                                else if (buildTower != null)
                                {
                                    tower.TowerStepUp(buildTower.GetComponent<Tower>());

                                }

                                if (buildmanager != null)
                                    buildmanager.GettowerpreviewActive = false;

                                tower.GetBuildState = buildstate;

                                Destroy(this.gameObject);
                                UiStateOff();
                            }
                            //}
                        }
                    }

                    //이동일 때 혹은 새로 지을 때
                    else if (!alreadytower && ((towernode != null) && !towernode.SetOnObstacle))
                    {
                        if (Input.GetMouseButtonDown(0))
                        {
                            //이동인지 새로 짓는건지 검사
                            //이동의 경우 위치만 변경
                            //설치의 경우 instantiate

                            //위치변경
                            //기존의 타워를 이동할 때
                            if (Origintower != null)
                            {
                                Origintower.transform.position = this.transform.position;
                                Origintower.GetComponent<Tower>().SetNode = towernode;
                                Origintower.GetComponent<Tower>().ActiveOn();
                                showtowerinfo.ShowInfo(Origintower.GetComponent<Tower>());

                                Destroy(this.gameObject);
                            }

                            //새로운 타워 건설
                            //빌드매니저에서 생성한 프리뷰가 타일에 지어질 때
                            else
                            {
                                GameObject buildedtower = Instantiate(buildTower, this.transform.position, Quaternion.identity);
                                buildedtower.GetComponent<Tower>().SetNode = towernode;
                                buildedtower.GetComponent<Tower>().SetNode.GetOnTower = true;
                                buildedtower.GetComponent<Tower>().SetShowTower = showtowerinfo;
                                buildedtower.GetComponent<Tower>().SetUp(playerstate);
                                buildedtower.GetComponent<Tower>().buildstate = buildstate;
                                showtowerinfo.ShowInfo(buildedtower.GetComponent<Tower>());
                                showtowerinfo.ShowRange(buildedtower.transform, TowerDataSetUp.GetData(buildedtower.GetComponent<Tower>().GetTowerCode).Range);

                            }
                            if (buildmanager != null)
                                buildmanager.GettowerpreviewActive = false;
                            showtowerinfo.SetTowerinfo();
                            UiStateOff();
                            Destroy(this.gameObject);

                        }

                    }

                }
            }
                yield return null;
        }
    }

    public void RangeOff()
    {
        showtowerinfo.RangeOff();
    }

    //빈 타일 / 길X => 타워 짓기
    //타워가 있을 경우 같은 이름, 같은 step이면 바로 진화

    //타일 위, 길 X, 타워 X => 타워 짓기
    //타일 위, 길 X, 타워 O -- 형재 미리보기와 같은 이름 단게 => 진화타워짓기


    private void OnTriggerEnter(Collider other)
    {
       // Debug.Log(towerstep);
        if (other.CompareTag("Tower"))
        {
            Debug.Log((other.GetComponent<Tower>().Getname == towername) + "1");

            Debug.Log(other.GetComponent<Tower>().Getname + "name1");
            Debug.Log(towername + "name2");

            Debug.Log((other.GetComponent<Tower>().GetStep == towerstep) + "2");
            Debug.Log((other.GetComponent<Tower>().GetStep != 3 && towerstep != 3) + "3");
            Debug.Log((!other.GetComponent<Tower>().GetCanWork) + "4");


            //       Debug.Log("asdasdfasdfasdfsdfsdfaasdfasdfsdfasdfasfasdf");
            if (other.GetComponent<Tower>().Getname == towername&& other.GetComponent<Tower>().GetStep==towerstep && other.GetComponent<Tower>().GetStep !=3&&towerstep!=3&& other.GetComponent<Tower>().GetCanWork)
            {
               
                CanCombination = true;
               // Debug.Log(CanCombination + " : 합체 여부");
                tower = other.GetComponent<Tower>();
            }
            else
            {
         //       Debug.Log("4564548564856456456456456456456456");
                CanCombination = false;
                tower = null;
            }

         //alreadytower = true;
        }
        //else
        //{
        //    Debug.Log("4564548564856456456456456456456456 : 456456456456456");
        //    CanCombination = false;
        //    //alreadytower = false;
        //}
     //   Debug.Log(CanCombination + " : 합체 여부222");
    }

    //빌드매니저에서 프리뷰를 생성할 때 초기화함수
    public void FirstSetUp(GameObject _buildtower,BuildManager _buildmanager)
    {
        buildmanager = _buildmanager;
        buildTower = _buildtower;
        towername = TowerDataSetUp.GetData(_buildtower.GetComponent<Tower>().GetTowerCode).Name;
        towerstep = TowerDataSetUp.GetData(_buildtower.GetComponent<Tower>().GetTowerCode).TowerStep;
        StartCoroutine("BuildTower");
    }

    //타워에서 이동버튼을 눌렀을 때 초기화함수
    public void TowerMoveSetUp(GameObject _OriginTower)
    {
        Origintower = _OriginTower;
        towername = _OriginTower.GetComponent<Tower>().Getname;
        towerstep = _OriginTower.GetComponent<Tower>().GetStep;
        StartCoroutine("BuildTower");
    }

    //preview의 ui관련 공통 함수
    public void TowerPreviewSetUp(ShowTowerInfo _showtowerinfo, GameObject[] _buildstate, PlayerState _playerstate,float _range)
    {
        range = _range;
        showtowerinfo = _showtowerinfo;
        buildstate = _buildstate;
        playerstate = _playerstate;
    }

    public void DestroyThis()
    {
        thisActive = false;
        showtowerinfo.RangeOff();
        for (int i = 0; i < 3; i++)
        {
            buildstate[i].SetActive(false);
        }
        Destroy(this.gameObject);
    }

}
