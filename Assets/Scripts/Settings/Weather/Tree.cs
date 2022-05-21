using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : Obstacle
{
    private WeatherSetting WS = null;
    public WeatherSetting SetWs { set => WS = value; }

    private int NextLevel = 1;
    private int UpRate = 2;

    private int age = 0;
    // 0 1 3 6 10 15

    private void Start()
    {
        removePrice = 30;
        node.SetOnObstacle = true;
    }

    public void EvolveTree(int _age)
    {
        age+=_age;
        removePrice += _age * 10;



        if (age >= NextLevel)
            {
                NextLevel += UpRate;
                UpRate++;

            this.transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
            removePrice += 10;
            }

        Debug.Log("age" + age);
        Debug.Log("nextLevel" + NextLevel);
    }

    public void ReturnNode()
    {
        node.SetOnObstacle = false;
        WS.RemoveTree(this);
    }
}
