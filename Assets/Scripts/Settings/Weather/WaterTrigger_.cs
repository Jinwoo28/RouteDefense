using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTrigger_ : MonoBehaviour
{
    //private List<Fruit> fruit = new List<Fruit>();

    private HashSet<Tower> towerlist = new HashSet<Tower>();
    private HashSet<Node> nodelist = new HashSet<Node>();


    private bool iced = false;
    public bool Geticed { set => iced = value; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tower"))
        {
            Debug.Log("¹Ù´Ù");
            other.GetComponent<Tower>().IsSetTowerCanWork = false;
            towerlist.Add(other.GetComponent<Tower>());
        }

        if (other.CompareTag("Tile"))
        {
            nodelist.Add(other.GetComponent<Node>());
        }

        if (other.CompareTag("TowerPreview"))
        {
            other.GetComponent<TowerPreview>().isSetWeater = true;
        }

        if (other.CompareTag("Fruit"))
        {
          //  fruit.Add(other.GetComponent<Fruit>());
        }
    }

    private void OnTriggerStay(Collider other)
    {
        
    }
    public void watericed()
    {
        Debug.Log(nodelist.Count);
        foreach(Node node in nodelist)
        {
            if (node.gameObject.transform.localScale.y <= this.transform.localScale.y+0.5f)
            {
                node.OnBranch();
            }
        }

        foreach (Tower tower in towerlist)
        {
            tower.IsGetCanWork = true;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Tower"))
        {
            other.GetComponent<Tower>().IsSetTowerCanWork = true;
            towerlist.Remove(other.GetComponent<Tower>());
        }
        if (other.CompareTag("TowerPreview"))
        {
            other.GetComponent<TowerPreview>().isSetWeater = false;
        }
        //if (other.CompareTag("Fruit"))
        //{
        //    other.GetComponent<Fruit>().ReturnPos();
        //}
    }
}
