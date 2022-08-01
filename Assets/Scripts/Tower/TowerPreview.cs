using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TowerPreview : MonoBehaviour
{
    AudioSource AS;
    private GameObject[] buildstate = null;
    private bool[] Can = new bool[3];
    //0 = 업그레이드
    //1 = 이동가능
    //2 = 이동불가

    //해당 타일에 이미 타워가 있을 경우
    private bool alreadytower = false;

    //타일 위인지 검사
    private bool isOnTile = false;

    private bool isOnWater = false;
    public bool isSetWeater { set => isOnWater = value; }

    //이동하는 길목인지 확인
    private bool isCheckOnroute = false;
    
    //build할 수 있는지 최종 여부
    private bool isCanBuildAble = false;

    //합체가능 한지 여부
    private bool isCanCombination = false;
    
    [SerializeField] private LayerMask layermask;

    //지어질 타워
    private GameObject buildTower = null;

    //프리뷰가 가진 타워의 단계
    private int towerstep = 0;

    public string towername = null;

    public PlayerState playerstate = null;

    private bool thisActive = true;

    DetectObject detector = new DetectObject();

    private Node towernode;
    private Tower tower = null;
    private ShowTowerInfo showtowerinfo = null;
    private BuildManager buildmanager = null;
    private float range = 0;

    //이동할 때 원래 타워의 정보
    private GameObject Origintower = null;

    private void Start()
    {
        AS = GetComponent<AudioSource>();
    }
    private void UiStateChange(int _i)
    {
        for (int i = 0; i < 3; i++)
        {
            if (i == _i)
            {
               Can[i] = true;
               buildstate[i].SetActive(true);
            }
            else
            {
                Can[i] = false;
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

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Can[2])
            {
                AS.Play();
            }
        }

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
    }

    IEnumerator BuildTower()
    {
        while (true)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.NameToLayer("Tile")))
            {
                if (hit.collider.CompareTag("Tile"))
                {
                    var nodeInfo = hit.collider.GetComponent<Node>();

                    int X = nodeInfo.gridX;
                    int Z = nodeInfo.gridY;
                    float Y = hit.collider.transform.localScale.y;
                    this.transform.position = new Vector3(X, (Y / 2), Z);

                    if (thisActive)
                    {
                        showtowerinfo.ShowRange(this.gameObject.transform, range);
                    }
                    //타일 위에 있는지
                    isOnTile = true;
                    //이미 타워가 있는지
                    alreadytower = nodeInfo.GetOnTower;
                    //이동 길목인지
                    isCheckOnroute = nodeInfo.Getwalkable;

                    towernode = nodeInfo;
                }
                else
                {
                    isOnTile = false;
                }
            }

            if (!EventSystem.current.IsPointerOverGameObject())
            {
                if (thisActive)
                {
                    //현재 위치의 설치가능 여부에 따라 preview 위에 ui표시
                    //불가능
                    if ((alreadytower && !isCanCombination) || (isCanCombination && towerstep == 3) 
                        || (isCanCombination && tower.GetStep == 3) || isCheckOnroute || isOnWater
                        || ((towernode!=null)&&towernode.GetSetCheckNode) || (towernode!=null&&towernode.SetOnObstacle))
                    {
                        UiStateChange(2);
                        GetComponentInChildren<Renderer>().sharedMaterial.SetColor("_Color", new Color(1, 0, 0));
                    }
                    //합체
                    else if ((alreadytower && isCanCombination && towerstep != 3 && tower.GetStep != 3) && !isOnWater)
                    {
                        GetComponentInChildren<Renderer>().sharedMaterial.SetColor("_Color", new Color(1, 1, 0));
                        UiStateChange(0);
                    }
                    //가능
                    else
                    {
                        UiStateChange(1);
                        GetComponentInChildren<Renderer>().sharedMaterial.SetColor("_Color", new Color(0, 1, 0));
                    }
                }

                //타워 합체
                if (isOnTile && !isCheckOnroute && !isOnWater)
                {
                    if (isCanCombination && alreadytower)
                    {
                        if (tower != null)
                        {
                            //if (tower.GetStep != 3)
                            //{
                            if (Input.GetMouseButtonDown(0))
                            {
                                if (Can[2])
                                {
                                    AS.Play();
                                }

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
                                    buildmanager.IsGettowerpreviewActive = false;

                                tower.GetBuildState = buildstate;

                                Destroy(this.gameObject);
                                UiStateOff();
                            }
                            //}
                        }
                    }

                    //이동 혹은 새로 지을 때
                    else if (!alreadytower && ((towernode != null) && !towernode.SetOnObstacle) && !towernode.GetSetCheckNode)
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
                                var originTower = Origintower.GetComponent<Tower>();
                                Origintower.transform.position = this.transform.position;
                                originTower.SetNode = towernode;
                                originTower.ActiveOn();
                                showtowerinfo.ShowInfo(originTower);

                                Destroy(this.gameObject);
                            }
                            //새로운 타워 건설
                            //빌드매니저에서 생성한 프리뷰가 타일에 지어질 때
                            else
                            {
                                GameObject buildedtower = Instantiate(buildTower, this.transform.position, Quaternion.identity);

                                var tower = buildedtower.GetComponent<Tower>();
                                tower.SetNode = towernode;
                                tower.SetNode.GetOnTower = true;
                                tower.SetShowTower = showtowerinfo;
                                tower.SetUp(playerstate);
                                tower.buildstate = buildstate;

                                showtowerinfo.ShowInfo(buildedtower.GetComponent<Tower>());
                                showtowerinfo.ShowRange(buildedtower.transform, TowerDataSetUp.GetData(buildedtower.GetComponent<Tower>().GetTowerCode).range);
                            }

                            if (buildmanager != null)
                            {
                                buildmanager.IsGettowerpreviewActive = false;
                            }
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
        if (other.CompareTag("Tower"))
        {
            var towerCom = other.GetComponent<Tower>();
            if (towerCom.Getname == towername && towerCom.GetStep==towerstep && towerCom.GetStep !=3 && towerstep!=3 && towerCom.IsGetCanWork)
            {
                isCanCombination = true;
                tower = other.GetComponent<Tower>();
            }
            else
            {
                isCanCombination = false;
                tower = null;
            }
         //alreadytower = true;
        }
        //else
        //{
        //    CanCombination = false;
        //    //alreadytower = false;
        //}
    }

    //빌드매니저에서 프리뷰를 생성할 때 초기화함수
    public void FirstSetUp(GameObject _buildtower,BuildManager _buildmanager)
    {
        buildmanager = _buildmanager;
        buildTower = _buildtower;

        var towerinfo = TowerDataSetUp.GetData(_buildtower.GetComponent<Tower>().GetTowerCode);
        towername = towerinfo.name;
        towerstep = towerinfo.towerStep;
        StartCoroutine("BuildTower");
    }

    //타워에서 이동버튼을 눌렀을 때 초기화함수
    public void TowerMoveSetUp(GameObject _OriginTower)
    {
        Origintower = _OriginTower;

        var towerCompo = _OriginTower.GetComponent<Tower>();
        towername = towerCompo.Getname;
        towerstep = towerCompo.GetStep;
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
