using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splash : Bullet
{ 
    [SerializeField] private LayerMask enemylayer;
    [SerializeField] private float Range = 3.0f;
    private void Start()
    {
        bullspeed = 10f;
    }
    protected override void AtkCharactor()
    {
        Collider[] E_collider = Physics.OverlapSphere(this.transform.position, Range, enemylayer);

        foreach(Collider EC in E_collider)
        {
            if (Vector3.Magnitude(EC.transform.position - this.transform.position) < 1.0f)
            {
                Debug.Log("ddd");
                EC.GetComponent<Enemy>().EnemyAttacked(damage);
            }
            else if(Vector3.Magnitude(EC.transform.position - this.transform.position) < 2.0f)
            {
                float decrease = damage - 1;
                if (decrease <= 0) continue;
                EC.GetComponent<Enemy>().EnemyAttacked(decrease);
            }
            else
            {
                float decrease = damage - 2;
                if (decrease <= 0) continue;
                EC.GetComponent<Enemy>().EnemyAttacked(decrease);
            }
        }
        Destroy(this.gameObject);


    }



}
