using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShowTowerInfo : MonoBehaviour
{
    private Tower tower;

    public GameObject towerinfopanel = null;

    private DetectObject detectob = null;

    [SerializeField] private Button upgradebutton;

    [SerializeField] private TextMeshProUGUI atkdamage;
    [SerializeField] private TextMeshProUGUI atkrange;
    [SerializeField] private TextMeshProUGUI atkcritical;
    [SerializeField] private TextMeshProUGUI atkspeed;

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
                    ShowRange(towertransform.transform);
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
            atkdamage.text = "Towerlev : " + tower.GetTowerLevel.ToString();
            atkrange.text = "AtkRange : "+tower.getatkrange.ToString();
            atkcritical.text = "AtkCritical : "+tower.getatkcritical.ToString();
            atkspeed.text = "Towerlev : " +tower.GetTowerLevel.ToString();
        }
    }

    public void ShowInfo(Tower _tower)
    {
        tower = _tower;
    }

    public void ShowRange(Transform _transform)
    {
        Transform towerpos = _transform;
        int rotation = 0;
        float range = towerpos.GetComponent<Tower>().getatkrange;
        Vector3 DD = towerpos.forward * range;
        Debug.Log(DD);
    
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

    private void RangeOff()
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

    public void OnClickCombine()
    {
        tower.Combine();
    }





}
