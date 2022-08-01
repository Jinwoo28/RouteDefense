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
    //0 = ���׷��̵�
    //1 = �̵�����
    //2 = �̵��Ұ�

    //�ش� Ÿ�Ͽ� �̹� Ÿ���� ���� ���
    private bool alreadytower = false;

    //Ÿ�� ������ �˻�
    private bool isOnTile = false;

    private bool isOnWater = false;
    public bool isSetWeater { set => isOnWater = value; }

    //�̵��ϴ� ������� Ȯ��
    private bool isCheckOnroute = false;
    
    //build�� �� �ִ��� ���� ����
    private bool isCanBuildAble = false;

    //��ü���� ���� ����
    private bool isCanCombination = false;
    
    [SerializeField] private LayerMask layermask;

    //������ Ÿ��
    private GameObject buildTower = null;

    //�����䰡 ���� Ÿ���� �ܰ�
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

    //�̵��� �� ���� Ÿ���� ����
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
                    //Ÿ�� ���� �ִ���
                    isOnTile = true;
                    //�̹� Ÿ���� �ִ���
                    alreadytower = nodeInfo.GetOnTower;
                    //�̵� �������
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
                    //���� ��ġ�� ��ġ���� ���ο� ���� preview ���� uiǥ��
                    //�Ұ���
                    if ((alreadytower && !isCanCombination) || (isCanCombination && towerstep == 3) 
                        || (isCanCombination && tower.GetStep == 3) || isCheckOnroute || isOnWater
                        || ((towernode!=null)&&towernode.GetSetCheckNode) || (towernode!=null&&towernode.SetOnObstacle))
                    {
                        UiStateChange(2);
                        GetComponentInChildren<Renderer>().sharedMaterial.SetColor("_Color", new Color(1, 0, 0));
                    }
                    //��ü
                    else if ((alreadytower && isCanCombination && towerstep != 3 && tower.GetStep != 3) && !isOnWater)
                    {
                        GetComponentInChildren<Renderer>().sharedMaterial.SetColor("_Color", new Color(1, 1, 0));
                        UiStateChange(0);
                    }
                    //����
                    else
                    {
                        UiStateChange(1);
                        GetComponentInChildren<Renderer>().sharedMaterial.SetColor("_Color", new Color(0, 1, 0));
                    }
                }

                //Ÿ�� ��ü
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

                    //�̵� Ȥ�� ���� ���� ��
                    else if (!alreadytower && ((towernode != null) && !towernode.SetOnObstacle) && !towernode.GetSetCheckNode)
                    {
                        if (Input.GetMouseButtonDown(0))
                        {
                            //�̵����� ���� ���°��� �˻�
                            //�̵��� ��� ��ġ�� ����
                            //��ġ�� ��� instantiate

                            //��ġ����
                            //������ Ÿ���� �̵��� ��

                            if (Origintower != null)
                            {
                                var originTower = Origintower.GetComponent<Tower>();
                                Origintower.transform.position = this.transform.position;
                                originTower.SetNode = towernode;
                                originTower.ActiveOn();
                                showtowerinfo.ShowInfo(originTower);

                                Destroy(this.gameObject);
                            }
                            //���ο� Ÿ�� �Ǽ�
                            //����Ŵ������� ������ �����䰡 Ÿ�Ͽ� ������ ��
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

    //�� Ÿ�� / ��X => Ÿ�� ����
    //Ÿ���� ���� ��� ���� �̸�, ���� step�̸� �ٷ� ��ȭ

    //Ÿ�� ��, �� X, Ÿ�� X => Ÿ�� ����
    //Ÿ�� ��, �� X, Ÿ�� O -- ���� �̸������ ���� �̸� �ܰ� => ��ȭŸ������

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

    //����Ŵ������� �����並 ������ �� �ʱ�ȭ�Լ�
    public void FirstSetUp(GameObject _buildtower,BuildManager _buildmanager)
    {
        buildmanager = _buildmanager;
        buildTower = _buildtower;

        var towerinfo = TowerDataSetUp.GetData(_buildtower.GetComponent<Tower>().GetTowerCode);
        towername = towerinfo.name;
        towerstep = towerinfo.towerStep;
        StartCoroutine("BuildTower");
    }

    //Ÿ������ �̵���ư�� ������ �� �ʱ�ȭ�Լ�
    public void TowerMoveSetUp(GameObject _OriginTower)
    {
        Origintower = _OriginTower;

        var towerCompo = _OriginTower.GetComponent<Tower>();
        towername = towerCompo.Getname;
        towerstep = towerCompo.GetStep;
        StartCoroutine("BuildTower");
    }

    //preview�� ui���� ���� �Լ�
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
