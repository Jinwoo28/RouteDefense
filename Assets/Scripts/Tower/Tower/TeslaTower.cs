using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeslaTower : AtkTower
{

    [SerializeField] private ParticleSystem Effect = null;
    [SerializeField] private GameObject ElectricBulet = null;

    private bool isAtking = false;

    protected override void Start()
    {
        SoundStop = false;
        base.Start();
        ElectricBulet.GetComponent<TeslaBullet>().InitSetUp(GetStep + 2, this,GetStep+4);
    }

    protected override void Update()
    {
        base.Update();
        
        if(FinalTarget == null)
        {
            Effect.Stop();
        }
    }

    protected override void Attack()
    {
        AS.Play();
        Debug.Log("АјАн");
        Effect.Play();

        ElectricBulet.GetComponent<TeslaBullet>().SetUp(towerinfo.towerdamage, FinalTarget,this.transform);   
    }

    public Transform GetShootPos() => shootPos;

}
