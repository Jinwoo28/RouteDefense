using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MortalTower : Tower
{
    [SerializeField] private bulletpolling bulletPool = null;
    [SerializeField] private bulletpolling patriclePool = null;

    protected override void Start()
    {
        base.Start();
        Debug.Log(AtkParticle);
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void Attack()
    {
        AtkParticle.GetComponent<ParticleSystem>().Play();
        
        var obj = bulletPool.GetObject(shootPos.position);
        obj.SetUp(FinalTarget, 3, bulletPool,5);
        obj.MortarSetDestination(FinalTarget.position,shootPos.position);
        obj.Attack();
        obj.SetMortarRange(GetStep + 1);

        obj.SetPool(patriclePool);

    }

 
}
