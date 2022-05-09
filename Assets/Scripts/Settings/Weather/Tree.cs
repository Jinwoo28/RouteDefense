using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    private WeatherSetting WS = null;
    public WeatherSetting SetWs { set => WS = value; }

    [SerializeField]
    private GameObject[] TreeObj = null;
    private int currentTree = 1;

    private int NextLevel = 1;
    private int UpRate = 2;

    private int removePrice = 50;
    public int GetRemovePrice() => removePrice;

    private Node node;
    public Node SetNode { set => node = value; }

    private int age = 0;

    private void Start()
    {
        node.SetOnObstacle = true;
    }

    public void EvolveTree(int _age)
    {
        age+=_age;
        removePrice += age * 20;

        if (currentTree <= TreeObj.Length)
        {
            if (age >= NextLevel)
            {
                NextLevel += UpRate;
                UpRate++;
                TreeObj[currentTree - 1].SetActive(false);
                TreeObj[currentTree].SetActive(true);
            }
        }
    }

    public void ReturnNode()
    {
        node.SetOnObstacle = false;
        WS.RemoveTree(this);
    }
}
