using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatlingTower : Tower
{
    [SerializeField] Transform RotateBarral = null;

    [SerializeField] GameObject HitEffect = null;
  

    protected override void Start()
    {
        base.Start();
        Debug.Log(AtkParticle);
        
    }
    protected override void RotateTurret()
    {
        base.RotateTurret();
        RotateBarral.Rotate(0, 0, 720*Time.deltaTime);
    }

    protected override void Update()
    {
        base.Update();

        if (SetTowerCanWork&&!GetIced)
        {
            if (FinalTarget != null)
            {
                HitEffect.SetActive(true);
                Vector3 Insight = cam.transform.position - FinalTarget.position;
                HitEffect.transform.position = FinalTarget.position + Insight.normalized;
                //HitEffect.GetComponent<ParticleSystem>().Play();
            }
            else
            {
                HitEffect.SetActive(false);
                AtkParticle.SetActive(false);
            }
        }

    }

    protected override void Attack()
    {
        if (FinalTarget != null)
        {
            AtkParticle.SetActive(true);
            FinalTarget.GetComponent<Enemy>().EnemyAttacked(towerinfo.towerdamage);
           
        }

    }

}
