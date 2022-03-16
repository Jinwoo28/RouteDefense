using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MortalTower : Tower
{
    private ObjectPooling Op = null;



    protected override void Start()
    {
        Op = this.GetComponent<ObjectPooling>();
        base.Start();
    }
    


    protected override void Attack()
    {
        Debug.Log(Op);
        var obj = Op.GetObject(shootPos.position);
        obj.SetUp(FinalTarget, 3, Op,3);
        obj.MortarSetDestination(FinalTarget.position,shootPos.position);
        obj.Attack();

    }

 
}
