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

    public bool GetShock()
    {
        return enemy.GetShock();
    }

    private void Update()
    {

    }
        

    public Transform GetPos()
    {
        return GetComponentInParent<Enemy>().gameObject.transform;
        
    }

    private void Awake()
    {
        enemy = this.GetComponentInParent<Enemy>();
        this.gameObject.tag = "Enemy";
    }

}
