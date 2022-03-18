using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MortalTower : Tower
{
    [SerializeField] private ObjectPooling bulletPool = null;
    [SerializeField] private ObjectPooling patriclePool = null;

    protected override void Start()
    {
        base.Start();
        Debug.Log(AtkParticle);
    }

    protected override void Update()
    {
        base.Update();
        Debug.Log(AtkParticle + " : 353513");
    }

    protected override void Attack()
    {
        Debug.Log(AtkParticle);
        AtkParticle.GetComponent<ParticleSystem>().Play();
        
        Debug.Log("count");
        var obj = bulletPool.GetObject(shootPos.position);
        obj.SetUp(FinalTarget, 3, bulletPool,5);
        obj.MortarSetDestination(FinalTarget.position,shootPos.position);
        obj.Attack();
        obj.SetMortarRange(GetStep + 1);

        obj.SetPool(patriclePool);

    }

 
}
