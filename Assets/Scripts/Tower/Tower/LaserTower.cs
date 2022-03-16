using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTower : Tower
{
    private LineRenderer lR = null;
    private Transform OriginTarget = null;
    protected override void Start()
    {
        base.Start();
        lR = this.GetComponent<LineRenderer>();
    }

}
