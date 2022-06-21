using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class BirdHitBox : MonoBehaviour,IEnumyAttacked
{
    private Enemy enemy = null;

    public void Attacked(float damage)
    {
        enemy.Attacked(damage);
    }

    private void Awake()
    {
        enemy = this.GetComponentInParent<Enemy>();
    }

}
