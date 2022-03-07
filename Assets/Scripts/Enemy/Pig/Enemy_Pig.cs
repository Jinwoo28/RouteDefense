using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Pig : Enemy
{
    float timevalue = 0;

    void Start()
    {
        StartMove();
    }

    // Update is called once per frame
    void Update()
    {
        UnitCharacteristic();
    }

    protected override void UnitCharacteristic()
    {
        timevalue += Time.deltaTime;

        if (timevalue >= 3.0f)
        {
            timevalue = 0;
            GetHp = 1;
        }
    }
}
