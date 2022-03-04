using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPreview : MonoBehaviour
{
    public string towername = null;

    //해당 타일에 이미 타워가 있을 경우
    private bool alreadytower = false;

    //타일 위인지 검사
    private bool ontile = false;

    //build할 수 있는지 최종 여부
    private bool canbuildable = false;

    //이동하는 길목인지 확인
    private bool checkOnroute = false;

    private bool CanCombination = false;

    private int towerstep = 0;

    public bool GetCanCombine => CanCombination;

    private Node towernode;

    private Tower tower = null;

    private void Start()
    {
        PreviewPos();
    }

    private void Update()
    {
        
    }

    private void PreviewPos()
    {

    }

    public bool CanBuildable()
    {
        if (Ontile() && !alreadytower&&!checkOnroute)
        {
            canbuildable = true;
        }
        else canbuildable = false;

            return canbuildable;
    }



    public bool Ontile()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        int layerMask = (-1) - (1 << LayerMask.NameToLayer("Tower"));

        if (Physics.Raycast(ray,out hit, Mathf.Infinity, layerMask))
        {
            if (hit.collider.CompareTag("Tile"))
            {
                ontile = true;
                int X = hit.collider.GetComponent<Node>().gridX;
                int Z = hit.collider.GetComponent<Node>().gridY;
                float Y = hit.collider.transform.localScale.y;

                this.transform.position = new Vector3(X, (Y/2), Z);
                checkOnroute = hit.collider.GetComponent<Node>().Getwalkable;
                //alreadytower = hit.collider.GetComponent<Node>().GetOnTower;
                towernode = hit.collider.GetComponent<Node>();
            }
            else 
            { 
                ontile = false;
                this.transform.position = new Vector3((int)hit.point.x, 1, (int)hit.point.z);
            }
        }
        return ontile;
    }

    public void CombinePreview()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if (hit.collider.CompareTag("Tile"))
            {
               
                int X = hit.collider.GetComponent<Node>().gridX;
                int Z = hit.collider.GetComponent<Node>().gridY;
                float Y = hit.collider.transform.localScale.y;

                this.transform.position = new Vector3(X, (Y / 2), Z);
               
            }
            else
            {
                
                this.transform.position = new Vector3((int)hit.point.x, 1, (int)hit.point.z);
            }
        }
    }

    public void SetUp(string _name,int _step)
    {
        towername = _name;
        towerstep = _step;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tower"))
        {
            if(other.GetComponent<Tower>().Getname == towername&& other.GetComponent<Tower>().gettowerstep==towerstep)
            { 
                CanCombination = true;
                tower = other.GetComponent<Tower>();
            }
            else
            {
            CanCombination = false;
                tower = null;
            }

            alreadytower = true;
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Tower"))
        {
            alreadytower = false;
            tower = null;
        }
    }

    public bool AlreadyTower => alreadytower;
    public Tower GetTower => tower;

    public Node GetTowerNode
    {
        get
        {
            return towernode;
        }
    }


}
