using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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
            ClickTower();
        }
    }

    public void ClickTower()
    {
        Debug.Log("2");
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
                for (int i = 0; i < 72; i++)
                {
                    rangesprite[i].SetActive(false);
                }
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
                    
                }
            }
            else
            {
                rangesprite[i].transform.position = towerpos.position + towerpos.forward;
            }
            rangesprite[i].SetActive(true);
            rotation += 5;
        }
    }





}
