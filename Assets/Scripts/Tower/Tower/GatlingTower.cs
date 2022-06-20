using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatlingTower : AtkTower
{
    [SerializeField] Transform RotateBarral = null;

    [SerializeField] GameObject HitEffect = null;

    bool isatk = false;
    bool soundPlay = false;

    protected override void Start()
    {
        base.Start();
        AS.Play();
    }

    protected override void RotateTurret()
    {
        base.RotateTurret();
        RotateBarral.Rotate(0, 0, 720*Time.deltaTime);
    }

    protected override void Update()
    {
        base.Update();

        if (SetTowerCanWork)
        {
            if (FinalTarget != null)
            {
                
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

    Transform target = null;
    protected override void Attack()
    {
        if (target != FinalTarget)
        {
            AS.Play();
            target = FinalTarget;
        }

        if (FinalTarget != null)
        {
            AtkParticle.SetActive(true);
            HitEffect.SetActive(true);
            Debug.Log(FinalTarget.gameObject.name);
            FinalTarget.GetComponent<Enemy>().EnemyAttacked(towerinfo.towerdamage);
        }

    }

}
