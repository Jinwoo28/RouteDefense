using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatlingTower : Tower
{
    [SerializeField] Transform RotateBarral = null;
    [SerializeField] private int atkNum = 0;

    protected override void RotateTurret()
    {
        base.RotateTurret();
        RotateBarral.Rotate(0, 0, 720*Time.deltaTime);
    }

    protected override void Attack()
    {
        FinalTarget.GetComponent<Enemy>().EnemyAttacked(towerinfo.towerdamage);
    }



    //IEnumerator GatlingAtk()
    //{
    //    for(int i = 0; i < atkNum; i++)
    //    {
    //        Debug.Log("АјАн333");

    //        if(FinalTarget!=null)
    //        FinalTarget.GetComponent<Enemy>().EnemyAttacked(towerinfo.towerdamage);
    //        yield return new WaitForSeconds(0.2f);
    //    }
    //}
}
