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

        
        if (FinalTarget == null)
        {
            AtkParticle.SetActive(false);
           HitEffect.SetActive(false);
        }
        else
        {
            Vector3 Insight = cam.transform.position - FinalTarget.position;
            HitEffect.SetActive(true);
            HitEffect.transform.position = FinalTarget.position+ Insight.normalized;
            //HitEffect.GetComponent<ParticleSystem>().Play();
        }

    }

    protected override void Attack()
    {
        if (FinalTarget != null)
        {
            FinalTarget.GetComponent<Enemy>().EnemyAttacked(towerinfo.towerdamage);
            AtkParticle.SetActive(true);
           
        }

    }

}
