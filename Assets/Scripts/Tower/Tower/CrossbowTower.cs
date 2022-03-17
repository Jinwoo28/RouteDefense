using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossbowTower : Tower
{
    private ObjectPooling op = null;
    protected override void Start()
    {
        op = this.GetComponent<ObjectPooling>();
        base.Start();
    }


    protected override void Attack() 
    {
        var obj = op.GetObject(shootPos.position);
        obj.SetUp(FinalTarget, towerinfo.towerdamage, op, 10f);
        obj.MortarSetDestination(FinalTarget.position + new Vector3(0,FinalTarget.transform.localPosition.y/2,0), shootPos.transform.position);
        obj.SetArrowDir(shootPos.forward);
    }

}
