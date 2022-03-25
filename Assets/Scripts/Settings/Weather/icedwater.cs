using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class icedwater : MonoBehaviour
{
    [SerializeField] private GameObject icecube = null;
    [SerializeField] private MapManager mapmanager = null;
    public void iced()
    {
        Debug.Log("Dddd");
        for (int i = 0; i < 30; i++)
        {
            for(int j = 0; j < 30; j++)
            {
               GameObject icedcube =  Instantiate(icecube, new Vector3(j, this.transform.position.y - (icecube.transform.localScale.y / 2), i), Quaternion.identity);
                icedcube.GetComponent<Obstacle>().SetNode = mapmanager.GetGrid[i, j];
                icedcube.transform.parent = this.transform;
            }
        }
    }
}
