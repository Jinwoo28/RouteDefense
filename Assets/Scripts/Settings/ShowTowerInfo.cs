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
    [SerializeField] private TextMeshProUGUI atkrange = null;
    [SerializeField] private TextMeshProUGUI atkspeed = null;

    //공격 범위 표시에 사용할 Sprite이미지
    [SerializeField] private GameObject rangePrefab = null;

    private GameObject[] rangesprite = null;

    private void Start()
    {
        detectob = this.GetComponent<DetectObject>();
        rangesprite = new GameObject[72];
        towerinfopanel.SetActive(false);
        for (int i = 0; i < 72; i++)
        {
            rangesprite[i] = Instantiate(rangePrefab);
            rangesprite[i].SetActive(false);
        }
    }

    private void Update()
    {
        ClickTower();
    }

    public void ClickTower()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Transform towertransform = detectob.ReturnTransform();
            if (towertransform != null)
            {
                if (towertransform.CompareTag("Tower"))
                {
                    towerinfopanel.SetActive(true);
                    tower = towertransform.GetComponent<Tower>();
                    ShowInfo(tower);
                    ShowRange(towertransform.transform, towertransform.GetComponent<Tower>().GetRange);
                }
                else
                {
                    towerinfopanel.SetActive(false);
                    RangeOff();
                }
            }
        }

        if (tower != null)
        {
            towername.text = $"{tower.Getname} {tower.GetStep}Step";
            towerlevel.text = "Level : " + tower.GetTowerLevel.ToString();
            atkdamage.text = "Damage : "+tower.GetDamage.ToString();
            atkcritical.text = "Critical : "+tower.GetCritical.ToString();
            atkspeed.text = "Speed : " +tower.GetSpeed.ToString();
            atkrange.text = "Range : " + tower.GetRange.ToString();
        }
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
        tower.TowerMove();
        RangeOff();
    }





}
