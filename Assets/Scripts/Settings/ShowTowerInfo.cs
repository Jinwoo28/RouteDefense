using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[System.Serializable]
public class towerTextinfo
{
    public string towername;
    public int towercode;
    //public TextMeshProUGUI showinfo;
}

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

    [SerializeField] private towerTextinfo[] textinfo;



    //공격 범위 표시에 사용할 Sprite이미지
    [SerializeField] private GameObject rangePrefab = null;

    [SerializeField] private EnemyManager enemymanager = null;

    private GameObject RangeParent = null;

    private int towerupgradeprice = 0;

    private GameObject[] rangesprite = null;

    private Transform towertransform = null;

    [SerializeField] private GameObject SellPanel = null;
    [SerializeField] private TextMeshProUGUI SellPrice = null;

    private bool ShowUpgradeInfo = false;

    public void ShowUPInfo()
    {
        ShowUpgradeInfo = !ShowUpgradeInfo;
    }

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

        //for(int i = 0; i< textinfo.Length; i++)
        //{
        //    textinfo[i].showinfo.text = textinfo[i].towername + "\n" + TowerDataSetUp.GetData(textinfo[i].towercode).TowerPrice * SkillSettings.PassiveValue("SetTowerDown");

        //}

    }

    private void Update()
    {
        
        ClickTower();

        if (tower != null)
        {
            towername.text = $"{tower.Getname}";
            atkspeed.text =  "레벨 "+tower.GetStep.ToString();

            if (!ShowUpgradeInfo)
            {
                towerlevel.text = "업그레이드 : " + tower.GetTowerLevel.ToString();
                towerlevel.color = Color.black;
                atkdamage.text = "데미지 : " + tower.GetDamage.ToString();
                atkdamage.color = Color.black;
                atkcritical.text = "크리티컬 : " + (tower.GetCritical*100).ToString() + "%";
                atkcritical.color = Color.black;
            }
            else
            {
                towerlevel.text = "업그레이드 : " + tower.GetTowerLevel.ToString() + " -> " + (tower.GetTowerLevel + 1).ToString();
                towerlevel.color = Color.red;
                atkdamage.text = "데미지 : " + tower.GetDamage.ToString() + " -> " + (tower.GetDamage + tower.GetTowerUPDamage);
                atkdamage.color = Color.red;
                atkcritical.text = "크리티컬 : " + (tower.GetCritical * 100).ToString() + "%" + " -> " + (tower.GetCritical + tower.GetTowerUpCri)*100 + "%";
                atkcritical.color = Color.red;
            }

            switch (tower.GetStep)
            {
                case 1:
                    if (tower.GetTowerLevel >= 3)
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
                    if (tower.GetTowerLevel >= 5)
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
                    if (tower.GetTowerLevel >= 7)
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
            
            RaycastHit hit;
            if (Physics.Raycast(towerpos.position + towerpos.forward* range + new Vector3(0, 10, 0), Vector3.down, out hit))
            {
                Debug.DrawLine(towerpos.position + towerpos.forward * range + new Vector3(0, 10, 0), towerpos.position + towerpos.forward + new Vector3(0,-10,0));
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
        GameManager.buttonOff(); RangeOff();
        tower.SellTower();
        towerinfopanel.SetActive(false);
        OffSellPrice();

    }

    public void OnClickUpgradeTower()
    {
        GameManager.buttonOff();
        tower.TowerUpgrade();
    }

    public void OnClickTowerMove()
    {
        GameManager.buttonOff();
        if (!enemymanager.GetGameOnGoing)
        {
            if (tower != null)
            {
                tower.TowerMove();
                RangeOff();
            }
        }
    }

    public void ShowSellPrice()
    {
        SellPrice.text = "판매 금액 : " + (tower.GetSetSellPrice).ToString();
        SellPanel.SetActive(true);
    }

    public void OffSellPrice()
    {
        SellPanel.SetActive(false);
    }


}
