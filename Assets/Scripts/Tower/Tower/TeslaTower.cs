using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeslaTower : AtkTower
{

    [SerializeField] private ParticleSystem Effect = null;
    [SerializeField] private GameObject ElectricBulet = null;

    private bool isAtking = false;
    private bool isWetTower = false;
    protected override void Start()
    {
        SoundStop = false;
        base.Start();
        ElectricBulet.GetComponent<TeslaBullet>().InitSetUp(GetStep + 2, this,GetStep+1);
        BuildManager.ActiveTowerTesla(this);
    }

    protected override void Update()
    {
        base.Update();
        
        if(FinalTarget == null)
        {
            Effect.Stop();
        }
    }

    public void ChangeWet(bool wet)
    {
        isWetTower = wet;
    }

    private void OnDestroy()
    {
        BuildManager.ReturnActiveTowerTesla(this);
    }

    protected override void Attack()
    {
        float giveDamage = isWetTower ? towerinfo.towerdamage / 2 : towerinfo.towerdamage;

        AS.Play();
       
        Effect.Play();

        ElectricBulet.GetComponent<TeslaBullet>().SetUp(giveDamage, FinalTarget,this.transform);   
    }

    public Transform GetShootPos() => shootPos;

}
