using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTower : Tower
{
    [SerializeField] private float X = 0;
    private LineRenderer lR = null;
    private Transform OriginTarget = null;
    private float atkdamage = 0;
    private bool isAtking = false;
    protected override void Start()
    {
        base.Start();
        lR = this.GetComponent<LineRenderer>();
        lR.enabled = false;
    }

    protected override void Update()
    {
        base.Update();

        if (OriginTarget != FinalTarget)
        {
            Debug.Log("dd");
            StopCoroutine("LaserShoot");
            OriginTarget = FinalTarget;
            atkdamage = towerinfo.towerdamage;
            isAtking = false;
        }


        if (FinalTarget != null)
        {
            RaycastHit hit;
            if(Physics.Raycast(shootPos.position, shootPos.forward,out hit))
            {
                if (hit.collider.CompareTag("Enemy"))
                {
                    lR.enabled = true;
                    lR.SetPosition(0, shootPos.position);
                    lR.SetPosition(1, hit.point + shootPos.forward*hit.collider.transform.localScale.z/2);
                    if (!isAtking)
                    {
                        Debug.Log("АјАн");
                        StartCoroutine("LaserShoot");
                    }
                }
            }

        }
        else
        {
            StopCoroutine("LaserShoot");
            atkdamage = towerinfo.towerdamage;
            isAtking = false;
            lR.enabled = false;
        }
    }

    private IEnumerator LaserShoot()
    {
        isAtking = true;
        while (FinalTarget != null)
        {
            yield return new WaitForSeconds(0.7f);
               
            FinalTarget.GetComponent<Enemy>().EnemyAttacked(atkdamage);
                atkdamage += 1;

        }
        
    }

}
