using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTower : Tower
{
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
            StopCoroutine("LaserShoot");
            OriginTarget = FinalTarget;
            atkdamage = towerinfo.towerdamage;
            isAtking = false;
        }


        if (FinalTarget != null)
        {
            //RaycastHit hit;
            //if(Physics.Raycast(shootPos.position, shootPos.forward,out hit))
            //{
            //    if (hit.collider.CompareTag("Enemy"))
            //    {
            if (Atking)
            {

                lR.enabled = true;

                lR.SetPosition(0, shootPos.position);
                lR.SetPosition(1, FinalTarget.position + new Vector3(0, FinalTarget.localScale.y / 2, 0));
                /* + shootPos.forward * hit.collider.transform.localScale.z / 2*/

                if (!isAtking)
                {
                    StartCoroutine("LaserShoot");
                }
                // }

                AtkParticle.transform.position = FinalTarget.position + new Vector3(0, FinalTarget.localScale.y / 2, 0);
                //  }
            }
            else
            {
                lR.enabled = false;
            }

        }
        else
        {
            AtkParticle.SetActive(false);

            StopCoroutine("LaserShoot");
            atkdamage = towerinfo.towerdamage;
            isAtking = false;
            lR.enabled = false;
        }
    }
    Transform target = null;
    private IEnumerator LaserShoot()
    {
        AtkParticle.SetActive(true);
        isAtking = true;
        if (target != FinalTarget)
        {
            AS.Play();
            target = FinalTarget;
        }

        while (FinalTarget != null)
        {
            yield return new WaitForSeconds(0.7f);
            if (FinalTarget != null)
            {
                FinalTarget.GetComponent<Enemy>().EnemyAttacked(atkdamage);
                atkdamage += (GetStep+1);
            }

        }
        
    }

}
