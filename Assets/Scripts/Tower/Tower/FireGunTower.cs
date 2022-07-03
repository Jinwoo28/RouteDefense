using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireGunTower : AtkTower
{
    [SerializeField] private GameObject FireBullet = null;
    private ParticleSystem FireEffect = null;
    private BoxCollider BulletCollider = null;
    private FireGunBullet fireGunBullet = null;
    private bool isfired = false;

    private bool isWetTower = false;

    protected override void Start()
    {
        //FireBullet.GetComponent<FireGunBullet>().SetUp(towerinfo.towerdamage,towerinfo.atkdelay);
        base.Start();
        FireEffect = FireBullet.GetComponent<ParticleSystem>();
        BulletCollider = FireBullet.GetComponent<BoxCollider>();
        fireGunBullet = FireBullet.GetComponent<FireGunBullet>();
        BuildManager.ActiveTower(this);
        BulletCollider.enabled = false;
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
            AS.Stop();
            isfired = false;
            BulletCollider.enabled = false;
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
        Debug.Log("???");
        fireGunBullet.SetUp(giveDamage);

        if (!isfired)
        {
            BulletCollider.enabled = true;
            AS.Play();
            isfired = true;
            FireEffect.Play();
        }

    }


}
