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

    //������ ����
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

    private Camera cam = null;

    //�̵��� �� ���� Ÿ���� ����
    private GameObject Origintower = null;

    private void Start()
    {
        AS = GetComponent<AudioSource>();
        cam = Camera.main;
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


    public void RangeOff()
    {
        showtowerinfo.RangeOff();
    }

    //�� Ÿ�� / ��X => Ÿ�� ����
    //Ÿ���� ���� ��� ���� �̸�, ���� step�̸� �ٷ� ��ȭ

    //Ÿ�� ��, �� X, Ÿ�� X => Ÿ�� ����
    //Ÿ�� ��, �� X, Ÿ�� O -- ���� �̸������ ���� �̸� �ܰ� => ��ȭŸ������

    private bool isHitObject = false;
    private Tower hitObject = null;

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

    private bool IsCanBuildTower()
    {
        return (!isOnWater && !GetTile().GetOnTower&&!GetTile().SetOnObstacle) ? true : false;
    }

    IEnumerator BuildTower()
    {
        while (true)
        {
            Node underNode = GetTile();

            if (!EventSystem.current.IsPointerOverGameObject())
            {
                if (thisActive)
                {
                    if (underNode != null && !underNode.Getwalkable)
                    {
                        //Ÿ�� ����
                        if (IsCanBuildTower())
                        {
                            UiStateChange(1);
                            GetComponentInChildren<Renderer>().sharedMaterial.SetColor("_Color", new Color(0, 1, 0));
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
                        //Ÿ�� ��ü
                        else if (isCanCombination)
                        {
                            GetComponentInChildren<Renderer>().sharedMaterial.SetColor("_Color", new Color(1, 1, 0));
                            UiStateChange(0);
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
                        }
                        //�Ұ���
                        else
                        {
                            UiStateChange(2);
                            GetComponentInChildren<Renderer>().sharedMaterial.SetColor("_Color", new Color(1, 0, 0));
                        }
                    }
                    else
                    {
                        UiStateChange(2);
                        GetComponentInChildren<Renderer>().sharedMaterial.SetColor("_Color", new Color(1, 0, 0));
                    }
                }
            }
                yield return null;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag("Tile"))
        {
            isHitObject = true;
            if (other.CompareTag("Tower"))
            {
                var towerCom = other.GetComponent<Tower>();
                hitObject = towerCom;
                if (IsCanCombination(towername, towerstep, towerCom) && towerCom.IsGetCanWork)
                {
                    isCanCombination = true;
                    tower = other.GetComponent<Tower>();
                }
                else
                {
                    isCanCombination = false;
                    tower = null;
                }
            }
        }
    }

    private bool IsCanCombination(string _preViewName, int _preViewStep, Tower _hitTower)
    {
        return ((_preViewName == _hitTower.Getname) && (_preViewStep == _hitTower.GetStep)&&(_preViewStep != 3||_hitTower.GetStep!=3))?true:false;
    }

    private Node GetTile()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layermask))
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
                return nodeInfo;
            }
            else
            {
                isOnTile = false;
                return null;
            }
        }
        else
        {
            return null;
        }
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
