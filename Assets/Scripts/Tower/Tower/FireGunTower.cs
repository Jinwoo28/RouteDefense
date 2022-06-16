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

    private bool isWetTower = false;

    protected override void Start()
    {
        FireBullet.GetComponent<FireGunBullet>().SetUp(towerinfo.towerdamage,towerinfo.atkdelay);
        base.Start();
        FireEffect = FireBullet.GetComponent<ParticleSystem>();
        Bullet = FireBullet.GetComponent<BoxCollider>();
        fireGunBullet = FireBullet.GetComponent<FireGunBullet>();
        BuildManager.ActiveTower(this);
    }

    public void ChangeWet(bool wet)
    {
        isWetTower = wet;
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
        Timer += Time.deltaTime;
    }


    private float Timer = 0;

    private void OnDestroy()
    {
        BuildManager.ReturnActiveTower(this);
    }

    protected override void Attack()
    {

        float giveDamage = isWetTower ? towerinfo.towerdamage / 2 : towerinfo.towerdamage;

        fireGunBullet.SetUp(giveDamage, towerinfo.atkdelay);

        if (!isfired)
        {
            isfired = true;
            FireEffect.Play();
        }

    }


}
