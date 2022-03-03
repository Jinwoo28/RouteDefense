using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPreview : MonoBehaviour
{
    [SerializeField] private GameObject uppertower = null;

    private bool alreadytower = false;
    private bool ontile = false;
    private bool canbuildable = false;
    private bool checkOnroute = false;
    private bool startendnode = false;

    private Node towernode;

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
        if (Ontile() && !alreadytower&&!checkOnroute&& startendnode)
        {
            canbuildable = true;
        }
        else canbuildable = false;

            return canbuildable;
    }

    private bool Ontile()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray,out hit, Mathf.Infinity))
        {
            if (hit.collider.CompareTag("Tile"))
            {
                ontile = true;
                int X = hit.collider.GetComponent<Node>().gridX;
                int Z = hit.collider.GetComponent<Node>().gridY;
                float Y = hit.collider.transform.localScale.y;

                this.transform.position = new Vector3(X, (Y/2), Z);
                checkOnroute = hit.collider.GetComponent<Node>().Getwalkable;
               // startendnode = hit.collider.GetComponent<Node>().GetStartEnd;
                alreadytower = hit.collider.GetComponent<Node>().GetOnTower;
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tower"))
        {
            alreadytower = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Tower"))
        {
            alreadytower = false;
        }
    }

    public Node GetTowerNode
    {
        get
        {
            return towernode;
        }
    }


}
