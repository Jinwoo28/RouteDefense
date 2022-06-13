using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireGunTower : Tower
{
    [SerializeField] private GameObject FireBullet = null;
    private ParticleSystem FireEffect = null;
    private BoxCollider Bullet = null;
    private FireGunBullet fireGunBullet = null;
    private bool isfired = false;

    protected override void Start()
    {
        base.Start();
        FireEffect = FireBullet.GetComponent<ParticleSystem>();
        Bullet = FireBullet.GetComponent<BoxCollider>();
        fireGunBullet = FireBullet.GetComponent<FireGunBullet>();
    }

    protected override void Update()
    {
        base.Update();
        if (FinalTarget == null)
        {
            FireEffect.Stop();
            isfired = false;
            //Bullet.enabled = false;
        }
        else
        {
            Debug.DrawLine(shootPos.position, FinalTarget.position);
            //Bullet.enabled = true;
        }
    }



    protected override void Attack()
    {
        fireGunBullet.SetUp(towerinfo.towerdamage,towerinfo.atkdelay);
        if (!isfired)
        {
            //Bullet.enabled = true;
            isfired = true;
            FireEffect.Play();
        }
    }


}
