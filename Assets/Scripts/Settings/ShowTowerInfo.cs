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

    [SerializeField] private Button upgradebutton;
    [SerializeField] private Button sellbutton;

    [SerializeField] private TextMeshProUGUI atkdamage;
    [SerializeField] private TextMeshProUGUI atkrange;
    [SerializeField] private TextMeshProUGUI atkcritical;
    [SerializeField] private TextMeshProUGUI atkspeed;

    [SerializeField] private GameObject rangePrefab = null;
    private GameObject[] rangesprite = null;

    private void Start()
    {
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
        if (Input.GetMouseButtonDown(0))
        {
            //마우스위치가 UI가 아니었을 때만
            //using으로 EventSystem을 넣어야 사용 가능
            if(!EventSystem.current.IsPointerOverGameObject())
            ClickTower();
        }
    }

    public void ClickTower()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        
        if (Physics.Raycast(ray, out hit, 100f))
        {
            if (hit.collider.CompareTag("Tower"))
            {
                towerinfopanel.SetActive(true);
                tower = hit.collider.GetComponent<Tower>();
                ShowInof(tower);
                ShowRange(hit.collider.transform);
            }
            else
            {
                towerinfopanel.SetActive(false);
                RangeOff();
            }
        }
    }

    public void ShowInof(Tower tower)
    {
        atkdamage.text = tower.getatkdamage.ToString();
        atkrange.text = tower.getatkrange.ToString();
        atkcritical.text = tower.getatkcritical.ToString();
        atkspeed.text = tower.getatkdelay.ToString();
    }

    public void ShowRange(Transform _transform)
    {
        Transform towerpos = _transform;
        int rotation = 0;
       
    
        for(int i = 0; i < 72; i++)
        {
            towerpos.rotation = Quaternion.Euler(0, rotation, 0);
            Ray ray;
            RaycastHit hit;
            if (Physics.Raycast(towerpos.position + towerpos.forward + new Vector3(0, 10, 0), Vector3.down, out hit))
            {
                if (hit.collider.CompareTag("Tile"))
                {
                    rangesprite[i].transform.position = hit.point;
                    rangesprite[i].transform.rotation = Quaternion.Euler(90, rotation, 0);
                }
                else
                {
                    rangesprite[i].transform.position = towerpos.position + towerpos.forward;
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
    }

    public void OnClickUpgradeTower()
    {
        RangeOff();
        tower.TowerUpgrade();
    }





}
