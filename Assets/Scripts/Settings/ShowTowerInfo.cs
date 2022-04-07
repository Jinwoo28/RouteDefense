using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShowTowerInfo : MonoBehaviour
{
    private Tower tower;

    [SerializeField] private GameObject towerinfopanel = null;

    private DetectObject detectob = null;

    [SerializeField] private Button upgradebutton;

    [SerializeField] private TextMeshProUGUI towername = null;
    [SerializeField] private TextMeshProUGUI towerlevel = null;
    [SerializeField] private TextMeshProUGUI atkdamage = null;
    [SerializeField] private TextMeshProUGUI atkcritical = null;
    [SerializeField] private TextMeshProUGUI atkspeed = null;
    [SerializeField] private TextMeshProUGUI upgradeprice = null;

    //공격 범위 표시에 사용할 Sprite이미지
    [SerializeField] private GameObject rangePrefab = null;

    [SerializeField] private EnemyManager enemymanager = null;

    private GameObject RangeParent = null;

    private int towerupgradeprice = 0;

    private GameObject[] rangesprite = null;

    private Transform towertransform = null;


    public Transform GetTowerTransform
    {
        set
        {
            towertransform = value;
        }
    }

    private void Start()
    {
        RangeParent = new GameObject("Range");
        detectob = this.GetComponent<DetectObject>();
        rangesprite = new GameObject[72];
        towerinfopanel.SetActive(false);
        for (int i = 0; i < 72; i++)
        {
            rangesprite[i] = Instantiate(rangePrefab, RangeParent.transform);
            rangesprite[i].SetActive(false);
        }
    }

    private void Update()
    {
        
        ClickTower();

        if (tower != null)
        {
            towername.text = $"{tower.Getname}";
            towerlevel.text = "업그레이드 + " + tower.GetTowerLevel.ToString();
            atkdamage.text = "데미지 : " + tower.GetDamage.ToString();
            atkcritical.text = "크리티컬 : " + tower.GetCritical.ToString();
            atkspeed.text =  "레벨 "+tower.GetStep.ToString();
           

            switch (tower.GetStep)
            {
                case 1:
                    if (tower.GetTowerLevel >= 10)
                    {
                        upgradebutton.interactable = false;
                        upgradeprice.fontSize = 50;
                        upgradeprice.text = "업그레이드 불가";
                    }
                    else
                    {
                        upgradebutton.interactable = true;
                        upgradeprice.fontSize = 50;
                        upgradeprice.text = "업그레이드 : " + tower.Gettowerupgradeprice.ToString();

                    }
                    break;

                case 2:
                    if (tower.GetTowerLevel >= 15)
                    {
                        upgradebutton.interactable = false;
                        upgradeprice.fontSize = 50;
                        upgradeprice.text = "업그레이드 불가";
                    }
                    else
                    {
                        upgradebutton.interactable = true;
                        upgradeprice.fontSize = 50;
                        upgradeprice.text = "업그레이드 : " + tower.Gettowerupgradeprice.ToString();
                    }
                    break;
                case 3:
                    upgradebutton.interactable = true;
                    upgradeprice.text = "업그레이드 : " + tower.Gettowerupgradeprice.ToString();
                    upgradeprice.fontSize = 50;
                    break;
            }
        }
    }

    

    public void ClickTower()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                //towertransform = detectob.ReturnTransform();
                //towertransform = DetectObject.GetNodeInfo();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 100f))
                {

                    if (hit.collider.CompareTag("Tower"))
                    {

                        tower = hit.collider.GetComponent<Tower>();
                        ShowInfo(tower);
                        Debug.Log(tower);

                        if (tower != null)
                        {
                            SetTowerinfo();
                        ShowRange(tower.gameObject.transform, tower.GetRange);
                        }
                    }
                    else
                    {
                        towerinfopanel.SetActive(false);
                        RangeOff();
                    }
                }
                
            }
        }
    }

    public void SetTowerinfo()
    {
        towerinfopanel.SetActive(true);
    }
    public void SetTowerinfoOff()
    {
        towerinfopanel.SetActive(false);
    }


    public void ShowInfo(Tower _tower)
    {
        tower = _tower;
    }

    public void ShowRange(Transform _transform,float _range)
    {
        Transform towerpos = _transform;
        int rotation = 0;
        float range = _range;
        Vector3 DD = towerpos.forward * range;

        for(int i = 0; i < 72; i++)
        {
            towerpos.rotation = Quaternion.Euler(0, rotation, 0);
            Ray ray;
            RaycastHit hit;
            if (Physics.Raycast(towerpos.position + towerpos.forward* range + new Vector3(0, 10, 0), Vector3.down, out hit))
            {
               
                if (hit.collider.CompareTag("Tile"))
                {
                    rangesprite[i].transform.position = hit.point;
                    rangesprite[i].transform.rotation = Quaternion.Euler(90, rotation, 0);
                }
                else
                {
                    rangesprite[i].transform.position = towerpos.position + towerpos.forward* range;
                    rangesprite[i].transform.rotation = Quaternion.Euler(90, rotation, 0);
                }
            }
            rangesprite[i].SetActive(true);
            rotation += 5;
        }

    }

    public void RangeOff()
    {

        for (int i = 0; i < 72; i++)
        {

            rangesprite[i].SetActive(false);
        }
    }
    public void OnClickSellTower()
    {
        RangeOff();
        tower.SellTower();
        towerinfopanel.SetActive(false);

    }

    public void OnClickUpgradeTower()
    {
        tower.TowerUpgrade();
    }

    public void OnClickTowerMove()
    {
        if (!enemymanager.GetGameOnGoing)
        {
            if (tower != null)
            {
                tower.TowerMove();
                RangeOff();
            }
        }
    }



}
