using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeslaTower : Tower
{
    [SerializeField] private GameObject ElectricBulet = null;


    protected override void Start()
    {
        base.Start();
        ElectricBulet.GetComponent<TeslaBullet>().InitSetUp(GetStep + 2, this,GetStep+4);
        ElectricBulet.transform.position = shootPos.position;
        ElectricBulet.SetActive(false);
    }

    protected override void Attack()
    {

            ElectricBulet.SetActive(true);
            ElectricBulet.GetComponent<TeslaBullet>().SetUp(towerinfo.towerdamage, FinalTarget);

    }

    public void ReturnBullet(TeslaBullet bullet)
    {
        bullet.gameObject.SetActive(false);
        bullet.transform.position = shootPos.position;
    }

    public Transform GetShootPos() => shootPos;

}
